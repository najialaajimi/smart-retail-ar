using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecommendationsController : MonoBehaviour
{
    [Header("UI References")]
    public Transform recommendationsContainer;
    public GameObject productCardPrefab;
    public Button backButton;
    public TextMeshProUGUI titleText;

    [Header("Filter UI")]
    public Toggle bioFilter;
    public Toggle ecoFilter;
    public Toggle priceFilter;
    public Toggle healthFilter;
    public TMP_Dropdown sortDropdown;

    [Header("Filter Settings")]
    public float minHealthScoreFilter = 70f;
    public float maxPriceMultiplier = 1.5f;

    private ProductData currentProduct;
    private List<ProductData> alternatives;
    private List<ProductData> filteredAlternatives;

    void Start()
    {
        SetupUI();
        LoadAlternatives();
        SetupFilters();
    }

    void SetupUI()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }

        if (titleText != null)
        {
            titleText.text = "Alternatives Recommandées";
        }
    }

    void LoadAlternatives()
    {
        string productId = QRCodeManager.Instance.GetCurrentProductId();
        
        if (string.IsNullOrEmpty(productId))
        {
            Debug.LogError("No product ID available");
            return;
        }

        currentProduct = ProductDatabase.Instance.GetProductById(productId);
        
        if (currentProduct == null)
        {
            Debug.LogError($"Product not found: {productId}");
            return;
        }

        alternatives = ProductDatabase.Instance.GetAlternatives(productId);
        filteredAlternatives = new List<ProductData>(alternatives);

        DisplayRecommendations();
    }

    void SetupFilters()
    {
        if (bioFilter != null)
        {
            bioFilter.onValueChanged.AddListener(delegate { OnFilterChanged(); });
        }

        if (ecoFilter != null)
        {
            ecoFilter.onValueChanged.AddListener(delegate { OnFilterChanged(); });
        }

        if (priceFilter != null)
        {
            priceFilter.onValueChanged.AddListener(delegate { OnFilterChanged(); });
        }

        if (healthFilter != null)
        {
            healthFilter.onValueChanged.AddListener(delegate { OnFilterChanged(); });
        }

        if (sortDropdown != null)
        {
            sortDropdown.onValueChanged.AddListener(delegate { OnSortChanged(); });
        }
    }

    void OnFilterChanged()
    {
        ApplyFilters();
        DisplayRecommendations();
    }

    void ApplyFilters()
    {
        filteredAlternatives = new List<ProductData>(alternatives);

        // Bio filter
        if (bioFilter != null && bioFilter.isOn)
        {
            filteredAlternatives = filteredAlternatives.Where(p => 
                p.tags != null && p.tags.Contains("Bio")).ToList();
        }

        // Eco filter
        if (ecoFilter != null && ecoFilter.isOn)
        {
            filteredAlternatives = filteredAlternatives.Where(p => 
                p.tags != null && p.tags.Contains("Éco-responsable")).ToList();
        }

        // Price filter
        if (priceFilter != null && priceFilter.isOn && currentProduct != null)
        {
            float maxPrice = currentProduct.price * maxPriceMultiplier;
            filteredAlternatives = filteredAlternatives.Where(p => 
                p.price <= maxPrice).ToList();
        }

        // Health score filter
        if (healthFilter != null && healthFilter.isOn)
        {
            filteredAlternatives = filteredAlternatives.Where(p => 
                p.scores != null && p.scores.healthScore >= minHealthScoreFilter).ToList();
        }
    }

    void OnSortChanged()
    {
        if (sortDropdown == null) return;

        switch (sortDropdown.value)
        {
            case 0: // Prix croissant
                filteredAlternatives = filteredAlternatives.OrderBy(p => p.price).ToList();
                break;
            case 1: // Prix décroissant
                filteredAlternatives = filteredAlternatives.OrderByDescending(p => p.price).ToList();
                break;
            case 2: // Score santé
                filteredAlternatives = filteredAlternatives.OrderByDescending(p => 
                    p.scores?.healthScore ?? 0).ToList();
                break;
            case 3: // Score écologique
                filteredAlternatives = filteredAlternatives.OrderByDescending(p => 
                    p.scores?.ecoScore ?? 0).ToList();
                break;
        }

        DisplayRecommendations();
    }

    void DisplayRecommendations()
    {
        if (recommendationsContainer == null || productCardPrefab == null)
        {
            Debug.LogError("Missing UI references");
            return;
        }

        // Clear existing cards
        foreach (Transform child in recommendationsContainer)
        {
            Destroy(child.gameObject);
        }

        // Create cards for filtered alternatives
        foreach (ProductData product in filteredAlternatives)
        {
            CreateProductCard(product);
        }

        // Show message if no alternatives
        if (filteredAlternatives.Count == 0)
        {
            Debug.Log("No alternatives match the current filters");
            // TODO: Show "no results" message
        }
    }

    void CreateProductCard(ProductData product)
    {
        GameObject card = Instantiate(productCardPrefab, recommendationsContainer);
        ProductCard cardComponent = card.GetComponent<ProductCard>();

        if (cardComponent != null)
        {
            cardComponent.SetupCard(product, currentProduct);
        }
        else
        {
            // Fallback if ProductCard component doesn't exist
            SetupCardManually(card, product);
        }
    }

    void SetupCardManually(GameObject card, ProductData product)
    {
        // Find and set text fields
        TextMeshProUGUI nameText = card.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
        if (nameText != null) nameText.text = product.name;

        TextMeshProUGUI brandText = card.transform.Find("BrandText")?.GetComponent<TextMeshProUGUI>();
        if (brandText != null) brandText.text = product.brand;

        TextMeshProUGUI priceText = card.transform.Find("PriceText")?.GetComponent<TextMeshProUGUI>();
        if (priceText != null) priceText.text = $"{product.price:F2}€";

        TextMeshProUGUI healthScoreText = card.transform.Find("HealthScoreText")?.GetComponent<TextMeshProUGUI>();
        if (healthScoreText != null && product.scores != null)
        {
            healthScoreText.text = $"Santé: {product.scores.healthScore:F0}";
        }

        TextMeshProUGUI ecoScoreText = card.transform.Find("EcoScoreText")?.GetComponent<TextMeshProUGUI>();
        if (ecoScoreText != null && product.scores != null)
        {
            ecoScoreText.text = $"Éco: {product.scores.ecoScore:F0}";
        }

        // Add click listener
        Button cardButton = card.GetComponent<Button>();
        if (cardButton != null)
        {
            cardButton.onClick.AddListener(() => OnProductCardClicked(product));
        }
    }

    void OnProductCardClicked(ProductData product)
    {
        Debug.Log($"Selected alternative: {product.name}");
        QRCodeManager.Instance.SetCurrentProductId(product.productId);
        NavigationManager.Instance.NavigateToProductInfo();
    }

    void OnBackButtonClicked()
    {
        NavigationManager.Instance.NavigateBack();
    }

    void OnDestroy()
    {
        if (backButton != null)
        {
            backButton.onClick.RemoveListener(OnBackButtonClicked);
        }

        if (bioFilter != null)
        {
            bioFilter.onValueChanged.RemoveAllListeners();
        }

        if (ecoFilter != null)
        {
            ecoFilter.onValueChanged.RemoveAllListeners();
        }

        if (priceFilter != null)
        {
            priceFilter.onValueChanged.RemoveAllListeners();
        }

        if (healthFilter != null)
        {
            healthFilter.onValueChanged.RemoveAllListeners();
        }

        if (sortDropdown != null)
        {
            sortDropdown.onValueChanged.RemoveAllListeners();
        }
    }
}
