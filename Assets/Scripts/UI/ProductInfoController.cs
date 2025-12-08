using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SmartRetailAR.Data;
using System.Collections.Generic;

namespace SmartRetailAR.UI
{
    /// <summary>
    /// Controller for the Product Info scene - displays detailed product information
    /// </summary>
    public class ProductInfoController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button backButton;
        [SerializeField] private Button viewInARButton;
        [SerializeField] private Button compareButton;
        [SerializeField] private TextMeshProUGUI productNameText;
        [SerializeField] private TextMeshProUGUI brandText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Image productImage;

        [Header("Nutritional Info")]
        [SerializeField] private TextMeshProUGUI caloriesText;
        [SerializeField] private TextMeshProUGUI proteinText;
        [SerializeField] private TextMeshProUGUI carbsText;
        [SerializeField] private TextMeshProUGUI fatText;
        [SerializeField] private TextMeshProUGUI nutriScoreText;

        [Header("Ecological Info")]
        [SerializeField] private TextMeshProUGUI carbonFootprintText;
        [SerializeField] private TextMeshProUGUI packagingText;
        [SerializeField] private TextMeshProUGUI originText;
        [SerializeField] private TextMeshProUGUI ecoScoreText;

        [Header("Recommendation Panel")]
        [SerializeField] private GameObject alternativesPanel;
        [SerializeField] private Transform alternativesContainer;
        [SerializeField] private GameObject productCardPrefab;

        private ProductData currentProduct;

        private void Start()
        {
            InitializeUI();
            LoadProductData();
        }

        private void InitializeUI()
        {
            if (backButton != null)
                backButton.onClick.AddListener(OnBackButtonClicked);

            if (viewInARButton != null)
                viewInARButton.onClick.AddListener(OnViewInARButtonClicked);

            if (compareButton != null)
                compareButton.onClick.AddListener(OnCompareButtonClicked);
        }

        private void LoadProductData()
        {
            string productId = PlayerPrefs.GetString("CurrentProductId", "");
            if (string.IsNullOrEmpty(productId))
            {
                Debug.LogError("No product ID found");
                return;
            }

            currentProduct = ProductDatabase.Instance.GetProductById(productId);
            if (currentProduct == null)
            {
                Debug.LogError($"Product not found: {productId}");
                return;
            }

            DisplayProductInfo();
            LoadRecommendations();
        }

        private void DisplayProductInfo()
        {
            if (productNameText != null)
                productNameText.text = currentProduct.name;

            if (brandText != null)
                brandText.text = currentProduct.brand;

            if (priceText != null)
                priceText.text = $"{currentProduct.price:F2} €";

            if (descriptionText != null)
                descriptionText.text = currentProduct.description;

            // Nutritional info
            if (caloriesText != null)
                caloriesText.text = $"{currentProduct.nutritionalInfo.calories} kcal";

            if (proteinText != null)
                proteinText.text = $"Protéines: {currentProduct.nutritionalInfo.protein}g";

            if (carbsText != null)
                carbsText.text = $"Glucides: {currentProduct.nutritionalInfo.carbs}g";

            if (fatText != null)
                fatText.text = $"Lipides: {currentProduct.nutritionalInfo.fat}g";

            if (nutriScoreText != null)
                nutriScoreText.text = $"Nutri-Score: {currentProduct.nutritionalInfo.nutriScore}";

            // Ecological info
            if (carbonFootprintText != null)
                carbonFootprintText.text = $"Empreinte carbone: {currentProduct.ecologicalInfo.carbonFootprint} kg CO2";

            if (packagingText != null)
                packagingText.text = $"Emballage: {currentProduct.ecologicalInfo.packaging}";

            if (originText != null)
                originText.text = $"Origine: {currentProduct.ecologicalInfo.origin}";

            if (ecoScoreText != null)
                ecoScoreText.text = $"Éco-Score: {currentProduct.ecologicalInfo.ecoScore}";
        }

        private void LoadRecommendations()
        {
            if (alternativesPanel != null && alternativesContainer != null && currentProduct != null)
            {
                // Get similar products from recommendation engine
                List<ProductData> alternatives = Recommendations.RecommendationEngine.Instance
                    .GetSimilarProducts(currentProduct, 3);

                foreach (var alternative in alternatives)
                {
                    if (productCardPrefab != null)
                    {
                        GameObject card = Instantiate(productCardPrefab, alternativesContainer);
                        ProductCard cardComponent = card.GetComponent<ProductCard>();
                        if (cardComponent != null)
                        {
                            cardComponent.SetProductData(alternative);
                        }
                    }
                }

                alternativesPanel.SetActive(alternatives.Count > 0);
            }
        }

        private void OnBackButtonClicked()
        {
            Utils.NavigationManager.Instance.LoadScene("HomeScene");
        }

        private void OnViewInARButtonClicked()
        {
            PlayerPrefs.SetString("ARProductId", currentProduct.id);
            Utils.NavigationManager.Instance.LoadScene("ARProductViewScene");
        }

        private void OnCompareButtonClicked()
        {
            PlayerPrefs.SetString("CompareProductId", currentProduct.id);
            Utils.NavigationManager.Instance.LoadScene("ComparisonScene");
        }

        private void OnDestroy()
        {
            if (backButton != null)
                backButton.onClick.RemoveListener(OnBackButtonClicked);

            if (viewInARButton != null)
                viewInARButton.onClick.RemoveListener(OnViewInARButtonClicked);

            if (compareButton != null)
                compareButton.onClick.RemoveListener(OnCompareButtonClicked);
        }
    }
}
