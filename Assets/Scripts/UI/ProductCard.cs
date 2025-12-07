using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SmartRetailAR.Data;
using SmartRetailAR.Utils;

namespace SmartRetailAR.UI
{
    /// <summary>
    /// Reusable product card component
    /// Used in recommendations and lists
    /// </summary>
    public class ProductCard : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI productNameText;
        public TextMeshProUGUI brandText;
        public TextMeshProUGUI priceText;
        public RawImage productImage;
        public TextMeshProUGUI healthScoreText;
        public TextMeshProUGUI ecoScoreText;
        public Button selectButton;
        
        [Header("Score Indicators")]
        public Image healthScoreIndicator;
        public Image ecoScoreIndicator;
        
        private ProductData _product;
        
        private void Start()
        {
            // Setup button listener
            if (selectButton != null)
            {
                selectButton.onClick.AddListener(OnSelectButtonClicked);
            }
        }
        
        /// <summary>
        /// Set product data for this card
        /// </summary>
        public void SetProduct(ProductData product)
        {
            _product = product;
            UpdateDisplay();
        }
        
        /// <summary>
        /// Update card display with product data
        /// </summary>
        private void UpdateDisplay()
        {
            if (_product == null) return;
            
            // Basic info
            if (productNameText != null)
            {
                productNameText.text = _product.name;
            }
            
            if (brandText != null)
            {
                brandText.text = _product.brand;
            }
            
            if (priceText != null)
            {
                priceText.text = $"{_product.price:F2} €";
            }
            
            // Health score
            if (healthScoreText != null)
            {
                healthScoreText.text = $"Santé: {_product.healthScore:F1}/10";
            }
            
            if (healthScoreIndicator != null)
            {
                healthScoreIndicator.color = GetScoreColor(_product.healthScore);
                healthScoreIndicator.fillAmount = _product.healthScore / 10f;
            }
            
            // Eco score
            if (ecoScoreText != null)
            {
                ecoScoreText.text = $"Éco: {_product.ecoScore:F1}/10";
            }
            
            if (ecoScoreIndicator != null)
            {
                ecoScoreIndicator.color = GetScoreColor(_product.ecoScore);
                ecoScoreIndicator.fillAmount = _product.ecoScore / 10f;
            }
            
            // Product image (placeholder)
            if (productImage != null)
            {
                productImage.color = new Color(0.85f, 0.85f, 0.85f, 1f);
            }
        }
        
        /// <summary>
        /// Get color based on score
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
        /// Handle select button click
        /// </summary>
        private void OnSelectButtonClicked()
        {
            if (_product == null) return;
            
            Debug.Log($"Product selected: {_product.name}");
            
            // Set as current product and navigate
            QRCodeManager.Instance.SetCurrentProductId(_product.id);
            NavigationManager.Instance.GoToProductInfo();
        }
        
        private void OnDestroy()
        {
            if (selectButton != null)
            {
                selectButton.onClick.RemoveListener(OnSelectButtonClicked);
            }
        }
    }
}
