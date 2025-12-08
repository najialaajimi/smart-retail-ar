using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SmartRetailAR.Data;
using System.Collections.Generic;

namespace SmartRetailAR.Sprint1.UI
{
    /// <summary>
    /// Displays detailed product information including nutritional data and certifications
    /// </summary>
    public class ProductInfoUI : MonoBehaviour
    {
        [Header("Product Info")]
        [SerializeField] private TextMeshProUGUI productNameText;
        [SerializeField] private TextMeshProUGUI brandText;
        [SerializeField] private TextMeshProUGUI categoryText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI originText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Image productImage;
        
        [Header("Scores")]
        [SerializeField] private Slider ecoScoreSlider;
        [SerializeField] private TextMeshProUGUI ecoScoreText;
        [SerializeField] private Slider healthScoreSlider;
        [SerializeField] private TextMeshProUGUI healthScoreText;
        [SerializeField] private Slider qualityScoreSlider;
        [SerializeField] private TextMeshProUGUI qualityScoreText;
        
        [Header("Nutritional Info")]
        [SerializeField] private TextMeshProUGUI caloriesText;
        [SerializeField] private TextMeshProUGUI proteinText;
        [SerializeField] private TextMeshProUGUI carbsText;
        [SerializeField] private TextMeshProUGUI sugarsText;
        [SerializeField] private TextMeshProUGUI fatsText;
        [SerializeField] private TextMeshProUGUI fiberText;
        [SerializeField] private TextMeshProUGUI sodiumText;
        
        [Header("Tags")]
        [SerializeField] private GameObject bioTag;
        [SerializeField] private GameObject localTag;
        [SerializeField] private GameObject veganTag;
        [SerializeField] private GameObject glutenFreeTag;
        [SerializeField] private GameObject fairTradeTag;
        
        [Header("Certifications")]
        [SerializeField] private Transform certificationsContainer;
        [SerializeField] private GameObject certificationBadgePrefab;
        
        [Header("Buttons")]
        [SerializeField] private Button viewARButton;
        [SerializeField] private Button viewAlternativesButton;
        [SerializeField] private Button backButton;
        
        private ProductData _currentProduct;
        
        void Start()
        {
            SetupButtons();
        }
        
        void SetupButtons()
        {
            if (viewARButton != null)
                viewARButton.onClick.AddListener(OnViewARClicked);
            
            if (viewAlternativesButton != null)
                viewAlternativesButton.onClick.AddListener(OnViewAlternativesClicked);
            
            if (backButton != null)
                backButton.onClick.AddListener(OnBackClicked);
        }
        
        public void DisplayProduct(ProductData product)
        {
            _currentProduct = product;
            
            // Basic info
            if (productNameText != null)
                productNameText.text = product.name;
            
            if (brandText != null)
                brandText.text = product.brand;
            
            if (categoryText != null)
                categoryText.text = product.category;
            
            if (priceText != null)
                priceText.text = $"{product.price:F2} €";
            
            if (originText != null)
                originText.text = $"Origin: {product.origin}";
            
            if (descriptionText != null)
                descriptionText.text = product.description;
            
            // Scores
            DisplayScores(product);
            
            // Nutritional info
            DisplayNutritionalInfo(product.nutritionalInfo);
            
            // Tags
            DisplayTags(product);
            
            // Certifications
            DisplayCertifications(product.certifications);
        }
        
        void DisplayScores(ProductData product)
        {
            // Eco Score
            if (ecoScoreSlider != null)
            {
                ecoScoreSlider.value = product.ecoScore / 100f;
            }
            if (ecoScoreText != null)
            {
                ecoScoreText.text = $"{product.ecoScore:F0}/100";
                ecoScoreText.color = GetScoreColor(product.ecoScore);
            }
            
            // Health Score
            if (healthScoreSlider != null)
            {
                healthScoreSlider.value = product.healthScore / 100f;
            }
            if (healthScoreText != null)
            {
                healthScoreText.text = $"{product.healthScore:F0}/100";
                healthScoreText.color = GetScoreColor(product.healthScore);
            }
            
            // Quality Score
            if (qualityScoreSlider != null)
            {
                qualityScoreSlider.value = product.qualityScore / 100f;
            }
            if (qualityScoreText != null)
            {
                qualityScoreText.text = $"{product.qualityScore:F0}/100";
                qualityScoreText.color = GetScoreColor(product.qualityScore);
            }
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
        
        void DisplayNutritionalInfo(NutritionalInfo info)
        {
            if (info == null) return;
            
            if (caloriesText != null)
                caloriesText.text = $"{info.calories:F0} kcal";
            
            if (proteinText != null)
                proteinText.text = $"{info.protein:F1}g";
            
            if (carbsText != null)
                carbsText.text = $"{info.carbohydrates:F1}g";
            
            if (sugarsText != null)
                sugarsText.text = $"{info.sugars:F1}g";
            
            if (fatsText != null)
                fatsText.text = $"{info.fats:F1}g";
            
            if (fiberText != null)
                fiberText.text = $"{info.fiber:F1}g";
            
            if (sodiumText != null)
                sodiumText.text = $"{info.sodium:F0}mg";
        }
        
        void DisplayTags(ProductData product)
        {
            if (bioTag != null)
                bioTag.SetActive(product.isBio);
            
            if (localTag != null)
                localTag.SetActive(product.isLocal);
            
            if (veganTag != null)
                veganTag.SetActive(product.isVegan);
            
            if (glutenFreeTag != null)
                glutenFreeTag.SetActive(product.isGlutenFree);
            
            if (fairTradeTag != null)
                fairTradeTag.SetActive(product.isFairTrade);
        }
        
        void DisplayCertifications(List<string> certifications)
        {
            if (certificationsContainer == null) return;
            
            // Clear existing certifications
            foreach (Transform child in certificationsContainer)
            {
                Destroy(child.gameObject);
            }
            
            // Add certification badges
            if (certifications != null && certificationBadgePrefab != null)
            {
                foreach (string cert in certifications)
                {
                    GameObject badge = Instantiate(certificationBadgePrefab, certificationsContainer);
                    TextMeshProUGUI badgeText = badge.GetComponentInChildren<TextMeshProUGUI>();
                    if (badgeText != null)
                    {
                        badgeText.text = cert;
                    }
                }
            }
        }
        
        void OnViewARClicked()
        {
            if (_currentProduct != null)
            {
                NavigationManager.Instance.ShowARView(_currentProduct);
            }
        }
        
        void OnViewAlternativesClicked()
        {
            if (_currentProduct != null)
            {
                NavigationManager.Instance.ShowRecommendations(_currentProduct);
            }
        }
        
        void OnBackClicked()
        {
            NavigationManager.Instance.GoBack();
        }
        
        void OnDestroy()
        {
            if (viewARButton != null)
                viewARButton.onClick.RemoveAllListeners();
            
            if (viewAlternativesButton != null)
                viewAlternativesButton.onClick.RemoveAllListeners();
            
            if (backButton != null)
                backButton.onClick.RemoveAllListeners();
        }
    }
}
