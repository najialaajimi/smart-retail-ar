using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SmartRetailAR.Data;

namespace SmartRetailAR.Sprint3.UI
{
    /// <summary>
    /// Product card component for displaying product information in lists
    /// </summary>
    public class ProductCard : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private TextMeshProUGUI productNameText;
        [SerializeField] private TextMeshProUGUI brandText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI priceDifferenceText;
        [SerializeField] private Image productImage;
        
        [Header("Scores")]
        [SerializeField] private TextMeshProUGUI ecoScoreText;
        [SerializeField] private TextMeshProUGUI healthScoreText;
        [SerializeField] private Image ecoScoreBar;
        [SerializeField] private Image healthScoreBar;
        
        [Header("Tags")]
        [SerializeField] private GameObject bioTag;
        [SerializeField] private GameObject localTag;
        [SerializeField] private GameObject veganTag;
        [SerializeField] private GameObject glutenFreeTag;
        
        [Header("Button")]
        [SerializeField] private Button viewDetailsButton;
        
        private ProductData _product;
        
        void Start()
        {
            if (viewDetailsButton != null)
            {
                viewDetailsButton.onClick.AddListener(OnViewDetailsClicked);
            }
        }
        
        public void SetProduct(ProductData product)
        {
            _product = product;
            UpdateUI();
        }
        
        void UpdateUI()
        {
            if (_product == null) return;
            
            // Basic info
            if (productNameText != null)
                productNameText.text = _product.name;
            
            if (brandText != null)
                brandText.text = _product.brand;
            
            if (priceText != null)
                priceText.text = $"{_product.price:F2} €";
            
            // Scores
            if (ecoScoreText != null)
            {
                ecoScoreText.text = $"{_product.ecoScore:F0}";
                ecoScoreText.color = GetScoreColor(_product.ecoScore);
            }
            
            if (healthScoreText != null)
            {
                healthScoreText.text = $"{_product.healthScore:F0}";
                healthScoreText.color = GetScoreColor(_product.healthScore);
            }
            
            if (ecoScoreBar != null)
            {
                ecoScoreBar.fillAmount = _product.ecoScore / 100f;
                ecoScoreBar.color = GetScoreColor(_product.ecoScore);
            }
            
            if (healthScoreBar != null)
            {
                healthScoreBar.fillAmount = _product.healthScore / 100f;
                healthScoreBar.color = GetScoreColor(_product.healthScore);
            }
            
            // Tags
            if (bioTag != null)
                bioTag.SetActive(_product.isBio);
            
            if (localTag != null)
                localTag.SetActive(_product.isLocal);
            
            if (veganTag != null)
                veganTag.SetActive(_product.isVegan);
            
            if (glutenFreeTag != null)
                glutenFreeTag.SetActive(_product.isGlutenFree);
        }
        
        Color GetScoreColor(float score)
        {
            if (score >= 75f)
                return new Color(0.2f, 0.8f, 0.2f); // Green
            else if (score >= 50f)
                return new Color(1f, 0.8f, 0f); // Yellow
            else
                return new Color(0.9f, 0.2f, 0.2f); // Red
        }
        
        void OnViewDetailsClicked()
        {
            if (_product != null)
            {
                NavigationManager.Instance.ShowProductDetails(_product);
            }
        }
        
        void OnDestroy()
        {
            if (viewDetailsButton != null)
            {
                viewDetailsButton.onClick.RemoveAllListeners();
            }
        }
    }
}
