using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SmartRetailAR.Data;
using SmartRetailAR.Sprint3.Recommendations;
using System.Collections.Generic;

namespace SmartRetailAR.Sprint3.UI
{
    /// <summary>
    /// UI for displaying product recommendations and alternatives
    /// </summary>
    public class RecommendationsUI : MonoBehaviour
    {
        [Header("Current Product")]
        [SerializeField] private TextMeshProUGUI currentProductName;
        [SerializeField] private TextMeshProUGUI currentProductPrice;
        
        [Header("Recommendations Container")]
        [SerializeField] private Transform recommendationsContainer;
        [SerializeField] private GameObject productCardPrefab;
        
        [Header("Filter Toggles")]
        [SerializeField] private Toggle bioToggle;
        [SerializeField] private Toggle localToggle;
        [SerializeField] private Toggle veganToggle;
        [SerializeField] private Toggle glutenFreeToggle;
        [SerializeField] private Toggle fairTradeToggle;
        
        [Header("Filter Sliders")]
        [SerializeField] private Slider maxPriceSlider;
        [SerializeField] private TextMeshProUGUI maxPriceText;
        [SerializeField] private Slider minEcoScoreSlider;
        [SerializeField] private TextMeshProUGUI minEcoScoreText;
        [SerializeField] private Slider minHealthScoreSlider;
        [SerializeField] private TextMeshProUGUI minHealthScoreText;
        
        [Header("Tab Buttons")]
        [SerializeField] private Button allRecommendationsButton;
        [SerializeField] private Button ecoFriendlyButton;
        [SerializeField] private Button healthyButton;
        [SerializeField] private Button budgetButton;
        
        [Header("Buttons")]
        [SerializeField] private Button applyFiltersButton;
        [SerializeField] private Button resetFiltersButton;
        [SerializeField] private Button backButton;
        
        private ProductData _currentProduct;
        private List<ProductData> _currentRecommendations;
        
        void Start()
        {
            SetupUI();
            LoadCurrentProduct();
        }
        
        void SetupUI()
        {
            // Setup filter listeners
            if (bioToggle != null)
                bioToggle.onValueChanged.AddListener(_ => OnFilterChanged());
            
            if (localToggle != null)
                localToggle.onValueChanged.AddListener(_ => OnFilterChanged());
            
            if (veganToggle != null)
                veganToggle.onValueChanged.AddListener(_ => OnFilterChanged());
            
            if (glutenFreeToggle != null)
                glutenFreeToggle.onValueChanged.AddListener(_ => OnFilterChanged());
            
            if (fairTradeToggle != null)
                fairTradeToggle.onValueChanged.AddListener(_ => OnFilterChanged());
            
            if (maxPriceSlider != null)
                maxPriceSlider.onValueChanged.AddListener(OnMaxPriceChanged);
            
            if (minEcoScoreSlider != null)
                minEcoScoreSlider.onValueChanged.AddListener(OnMinEcoScoreChanged);
            
            if (minHealthScoreSlider != null)
                minHealthScoreSlider.onValueChanged.AddListener(OnMinHealthScoreChanged);
            
            // Setup tab buttons
            if (allRecommendationsButton != null)
                allRecommendationsButton.onClick.AddListener(() => ShowAllRecommendations());
            
            if (ecoFriendlyButton != null)
                ecoFriendlyButton.onClick.AddListener(() => ShowEcoFriendly());
            
            if (healthyButton != null)
                healthyButton.onClick.AddListener(() => ShowHealthy());
            
            if (budgetButton != null)
                budgetButton.onClick.AddListener(() => ShowBudget());
            
            // Setup action buttons
            if (applyFiltersButton != null)
                applyFiltersButton.onClick.AddListener(ApplyFilters);
            
            if (resetFiltersButton != null)
                resetFiltersButton.onClick.AddListener(ResetFilters);
            
            if (backButton != null)
                backButton.onClick.AddListener(OnBackClicked);
        }
        
        void LoadCurrentProduct()
        {
            _currentProduct = NavigationManager.Instance.GetCurrentProduct();
            
            if (_currentProduct != null)
            {
                DisplayCurrentProduct();
                ShowAllRecommendations();
            }
        }
        
        void DisplayCurrentProduct()
        {
            if (currentProductName != null)
                currentProductName.text = _currentProduct.name;
            
            if (currentProductPrice != null)
                currentProductPrice.text = $"{_currentProduct.price:F2} €";
        }
        
        void ShowAllRecommendations()
        {
            _currentRecommendations = RecommendationEngine.Instance.GetRecommendations(_currentProduct);
            DisplayRecommendations(_currentRecommendations);
        }
        
        void ShowEcoFriendly()
        {
            _currentRecommendations = RecommendationEngine.Instance.GetEcoFriendlyAlternatives(_currentProduct);
            DisplayRecommendations(_currentRecommendations);
        }
        
        void ShowHealthy()
        {
            _currentRecommendations = RecommendationEngine.Instance.GetHealthyAlternatives(_currentProduct);
            DisplayRecommendations(_currentRecommendations);
        }
        
        void ShowBudget()
        {
            _currentRecommendations = RecommendationEngine.Instance.GetBudgetAlternatives(_currentProduct);
            DisplayRecommendations(_currentRecommendations);
        }
        
        void DisplayRecommendations(List<ProductData> products)
        {
            // Clear existing cards
            foreach (Transform child in recommendationsContainer)
            {
                Destroy(child.gameObject);
            }
            
            // Create new cards
            foreach (var product in products)
            {
                GameObject card = Instantiate(productCardPrefab, recommendationsContainer);
                var cardScript = card.GetComponent<ProductCard>();
                if (cardScript != null)
                {
                    cardScript.SetProduct(product);
                }
            }
            
            Debug.Log($"Displaying {products.Count} recommendations");
        }
        
        void OnFilterChanged()
        {
            // Auto-apply filters on change (optional)
        }
        
        void OnMaxPriceChanged(float value)
        {
            if (maxPriceText != null)
                maxPriceText.text = $"{value:F2} €";
        }
        
        void OnMinEcoScoreChanged(float value)
        {
            if (minEcoScoreText != null)
                minEcoScoreText.text = $"{value:F0}";
        }
        
        void OnMinHealthScoreChanged(float value)
        {
            if (minHealthScoreText != null)
                minHealthScoreText.text = $"{value:F0}";
        }
        
        void ApplyFilters()
        {
            var criteria = new RecommendationEngine.RecommendationCriteria
            {
                preferBio = bioToggle != null && bioToggle.isOn,
                preferLocal = localToggle != null && localToggle.isOn,
                preferVegan = veganToggle != null && veganToggle.isOn,
                preferGlutenFree = glutenFreeToggle != null && glutenFreeToggle.isOn,
                preferFairTrade = fairTradeToggle != null && fairTradeToggle.isOn,
                maxPrice = maxPriceSlider != null ? maxPriceSlider.value : float.MaxValue,
                minEcoScore = minEcoScoreSlider != null ? minEcoScoreSlider.value : 0f,
                minHealthScore = minHealthScoreSlider != null ? minHealthScoreSlider.value : 0f
            };
            
            RecommendationEngine.Instance.SetCriteria(criteria);
            ShowAllRecommendations();
        }
        
        void ResetFilters()
        {
            if (bioToggle != null) bioToggle.isOn = false;
            if (localToggle != null) localToggle.isOn = false;
            if (veganToggle != null) veganToggle.isOn = false;
            if (glutenFreeToggle != null) glutenFreeToggle.isOn = false;
            if (fairTradeToggle != null) fairTradeToggle.isOn = false;
            
            if (maxPriceSlider != null) maxPriceSlider.value = maxPriceSlider.maxValue;
            if (minEcoScoreSlider != null) minEcoScoreSlider.value = 0f;
            if (minHealthScoreSlider != null) minHealthScoreSlider.value = 0f;
            
            ApplyFilters();
        }
        
        void OnBackClicked()
        {
            NavigationManager.Instance.GoBack();
        }
        
        void OnDestroy()
        {
            // Cleanup listeners
            if (applyFiltersButton != null)
                applyFiltersButton.onClick.RemoveAllListeners();
            
            if (resetFiltersButton != null)
                resetFiltersButton.onClick.RemoveAllListeners();
            
            if (backButton != null)
                backButton.onClick.RemoveAllListeners();
        }
    }
}
