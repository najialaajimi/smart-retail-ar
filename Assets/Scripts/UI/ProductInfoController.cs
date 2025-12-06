using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductInfoController : MonoBehaviour
{
    [Header("UI References - Product Info")]
    public TextMeshProUGUI productNameText;
    public TextMeshProUGUI brandText;
    public Image productImage;
    public Button alternativesButton;
    public Button backButton;

    [Header("UI References - Nutritional Info")]
    public TextMeshProUGUI caloriesText;
    public TextMeshProUGUI proteinsText;
    public TextMeshProUGUI carbohydratesText;
    public TextMeshProUGUI fatsText;
    public TextMeshProUGUI fiberText;
    public TextMeshProUGUI sugarText;
    public TextMeshProUGUI saltText;

    [Header("UI References - Origin")]
    public TextMeshProUGUI countryText;
    public TextMeshProUGUI regionText;
    public TextMeshProUGUI producerText;

    [Header("UI References - Scores")]
    public TextMeshProUGUI healthScoreText;
    public TextMeshProUGUI ecoScoreText;
    public TextMeshProUGUI nutritionGradeText;
    public Image healthScoreFill;
    public Image ecoScoreFill;

    [Header("UI References - Tags")]
    public Transform tagsContainer;
    public GameObject tagPrefab;

    [Header("Score Colors")]
    public Color excellentScoreColor = Color.green;
    public Color goodScoreColor = Color.yellow;
    public Color averageScoreColor = Color.yellow;
    public Color poorScoreColor = Color.red;

    private ProductData currentProduct;

    void Start()
    {
        SetupButtons();
        LoadProductData();
    }

    void SetupButtons()
    {
        if (alternativesButton != null)
        {
            alternativesButton.onClick.AddListener(OnAlternativesButtonClicked);
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }
    }

    void LoadProductData()
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

        DisplayProductInfo();
    }

    void DisplayProductInfo()
    {
        // Basic Info
        if (productNameText != null)
        {
            productNameText.text = currentProduct.name;
        }

        if (brandText != null)
        {
            brandText.text = currentProduct.brand;
        }

        // TODO: Load product image from URL
        // if (productImage != null && !string.IsNullOrEmpty(currentProduct.imageUrl))
        // {
        //     LoadImageFromUrl(currentProduct.imageUrl);
        // }

        // Nutritional Info
        DisplayNutritionalInfo();

        // Origin Info
        DisplayOriginInfo();

        // Scores
        DisplayScores();

        // Tags
        DisplayTags();
    }

    void DisplayNutritionalInfo()
    {
        if (currentProduct.nutritionalInfo == null) return;

        if (caloriesText != null)
        {
            caloriesText.text = $"{currentProduct.nutritionalInfo.calories} kcal";
        }

        if (proteinsText != null)
        {
            proteinsText.text = $"{currentProduct.nutritionalInfo.proteins}g";
        }

        if (carbohydratesText != null)
        {
            carbohydratesText.text = $"{currentProduct.nutritionalInfo.carbohydrates}g";
        }

        if (fatsText != null)
        {
            fatsText.text = $"{currentProduct.nutritionalInfo.fats}g";
        }

        if (fiberText != null)
        {
            fiberText.text = $"{currentProduct.nutritionalInfo.fiber}g";
        }

        if (sugarText != null)
        {
            sugarText.text = $"{currentProduct.nutritionalInfo.sugar}g";
        }

        if (saltText != null)
        {
            saltText.text = $"{currentProduct.nutritionalInfo.salt}g";
        }
    }

    void DisplayOriginInfo()
    {
        if (currentProduct.origin == null) return;

        if (countryText != null)
        {
            countryText.text = currentProduct.origin.country;
        }

        if (regionText != null)
        {
            regionText.text = currentProduct.origin.region;
        }

        if (producerText != null)
        {
            producerText.text = currentProduct.origin.producer;
        }
    }

    void DisplayScores()
    {
        if (currentProduct.scores == null) return;

        // Health Score
        if (healthScoreText != null)
        {
            healthScoreText.text = $"{currentProduct.scores.healthScore:F0}/100";
        }

        if (healthScoreFill != null)
        {
            healthScoreFill.fillAmount = currentProduct.scores.healthScore / 100f;
            healthScoreFill.color = GetScoreColor(currentProduct.scores.healthScore);
        }

        // Eco Score
        if (ecoScoreText != null)
        {
            ecoScoreText.text = $"{currentProduct.scores.ecoScore:F0}/100";
        }

        if (ecoScoreFill != null)
        {
            ecoScoreFill.fillAmount = currentProduct.scores.ecoScore / 100f;
            ecoScoreFill.color = GetScoreColor(currentProduct.scores.ecoScore);
        }

        // Nutrition Grade
        if (nutritionGradeText != null)
        {
            nutritionGradeText.text = currentProduct.scores.nutritionGrade;
        }
    }

    Color GetScoreColor(float score)
    {
        if (score >= 80) return excellentScoreColor;
        if (score >= 60) return goodScoreColor;
        if (score >= 40) return averageScoreColor;
        return poorScoreColor;
    }

    void DisplayTags()
    {
        if (tagsContainer == null || tagPrefab == null || currentProduct.tags == null) return;

        // Clear existing tags
        foreach (Transform child in tagsContainer)
        {
            Destroy(child.gameObject);
        }

        // Create new tags
        foreach (string tag in currentProduct.tags)
        {
            GameObject tagObject = Instantiate(tagPrefab, tagsContainer);
            TextMeshProUGUI tagText = tagObject.GetComponentInChildren<TextMeshProUGUI>();
            if (tagText != null)
            {
                tagText.text = tag;
            }
        }
    }

    void OnAlternativesButtonClicked()
    {
        Debug.Log("Navigate to Recommendations");
        NavigationManager.Instance.NavigateToRecommendations();
    }

    void OnBackButtonClicked()
    {
        NavigationManager.Instance.NavigateBack();
    }

    void OnDestroy()
    {
        if (alternativesButton != null)
        {
            alternativesButton.onClick.RemoveListener(OnAlternativesButtonClicked);
        }

        if (backButton != null)
        {
            backButton.onClick.RemoveListener(OnBackButtonClicked);
        }
    }
}
