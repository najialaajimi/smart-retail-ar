using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SmartRetailAR.Utils;
using SmartRetailAR.Data;

namespace SmartRetailAR.UI
{
    /// <summary>
    /// Controller for product information display
    /// Shows detailed product data and scores
    /// </summary>
    public class ProductInfoController : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI productNameText;
        public TextMeshProUGUI brandText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI originText;
        public TextMeshProUGUI priceText;
        public RawImage productImage;
        
        [Header("Nutritional Info")]
        public TextMeshProUGUI caloriesText;
        public TextMeshProUGUI proteinsText;
        public TextMeshProUGUI carbohydratesText;
        public TextMeshProUGUI fatsText;
        
        [Header("Scores")]
        public Slider healthScoreSlider;
        public TextMeshProUGUI healthScoreText;
        public Slider ecoScoreSlider;
        public TextMeshProUGUI ecoScoreText;
        
        [Header("Buttons")]
        public Button alternativesButton;
        public Button backButton;
        
        private ProductData _currentProduct;
        
        private void Start()
        {
            // Setup button listeners
            if (alternativesButton != null)
            {
                alternativesButton.onClick.AddListener(OnAlternativesButtonClicked);
            }
            
            if (backButton != null)
            {
                backButton.onClick.AddListener(OnBackButtonClicked);
            }
            
            // Load and display product
            LoadProduct();
        }
        
        /// <summary>
        /// Load product data
        /// </summary>
        private void LoadProduct()
        {
            string productId = QRCodeManager.Instance.GetCurrentProductId();
            
            if (string.IsNullOrEmpty(productId))
            {
                Debug.LogError("No product ID available");
                return;
            }
            
            _currentProduct = ProductDatabase.Instance.GetProduct(productId);
            
            if (_currentProduct == null)
            {
                Debug.LogError($"Product not found: {productId}");
                return;
            }
            
            // Add to user history
            var preferences = UserPreferences.Load();
            preferences.AddToHistory(productId);
            preferences.Save();
            
            // Display product information
            DisplayProduct();
        }
        
        /// <summary>
        /// Display product information
        /// </summary>
        private void DisplayProduct()
        {
            if (_currentProduct == null) return;
            
            // Basic info
            if (productNameText != null)
                productNameText.text = _currentProduct.name;
            
            if (brandText != null)
                brandText.text = _currentProduct.brand;
            
            if (descriptionText != null)
                descriptionText.text = _currentProduct.description;
            
            if (originText != null)
            {
                string originInfo = _currentProduct.origin;
                if (!string.IsNullOrEmpty(_currentProduct.originRegion))
                {
                    originInfo += $", {_currentProduct.originRegion}";
                }
                originText.text = $"Origine: {originInfo}";
            }
            
            if (priceText != null)
                priceText.text = $"{_currentProduct.price:F2} €";
            
            // Nutritional info
            if (_currentProduct.nutritionalInfo != null)
            {
                if (caloriesText != null)
                    caloriesText.text = $"Calories: {_currentProduct.nutritionalInfo.calories} kcal";
                
                if (proteinsText != null)
                    proteinsText.text = $"Protéines: {_currentProduct.nutritionalInfo.proteins}g";
                
                if (carbohydratesText != null)
                    carbohydratesText.text = $"Glucides: {_currentProduct.nutritionalInfo.carbohydrates}g";
                
                if (fatsText != null)
                    fatsText.text = $"Lipides: {_currentProduct.nutritionalInfo.fats}g";
            }
            
            // Scores
            DisplayScores();
            
            // Product image (placeholder for now)
            if (productImage != null)
            {
                productImage.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            }
        }
        
        /// <summary>
        /// Display health and eco scores
        /// </summary>
        private void DisplayScores()
        {
            // Health score (0-10 scale)
            if (healthScoreSlider != null)
            {
                healthScoreSlider.value = _currentProduct.healthScore / 10f;
            }
            
            if (healthScoreText != null)
            {
                healthScoreText.text = $"Score Santé: {_currentProduct.healthScore:F1}/10";
                healthScoreText.color = GetScoreColor(_currentProduct.healthScore);
            }
            
            // Eco score (0-10 scale)
            if (ecoScoreSlider != null)
            {
                ecoScoreSlider.value = _currentProduct.ecoScore / 10f;
            }
            
            if (ecoScoreText != null)
            {
                ecoScoreText.text = $"Score Écologique: {_currentProduct.ecoScore:F1}/10";
                ecoScoreText.color = GetScoreColor(_currentProduct.ecoScore);
            }
        }
        
        /// <summary>
        /// Get color based on score value
        /// </summary>
        private Color GetScoreColor(float score)
        {
            if (score >= 7f)
                return new Color(0.2f, 0.8f, 0.2f); // Green
            else if (score >= 4f)
                return new Color(1f, 0.8f, 0f); // Orange
            else
                return new Color(0.9f, 0.2f, 0.2f); // Red
        }
        
        /// <summary>
        /// Handle alternatives button click
        /// </summary>
        private void OnAlternativesButtonClicked()
        {
            Debug.Log("Navigating to recommendations");
            NavigationManager.Instance.GoToRecommendations();
        }
        
        /// <summary>
        /// Handle back button click
        /// </summary>
        private void OnBackButtonClicked()
        {
            NavigationManager.Instance.NavigateBack();
        }
        
        private void OnDestroy()
        {
            // Cleanup button listeners
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
}
