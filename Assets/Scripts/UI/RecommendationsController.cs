using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using SmartRetailAR.Utils;
using SmartRetailAR.Data;

namespace SmartRetailAR.UI
{
    /// <summary>
    /// Controller for product recommendations/alternatives
    /// Handles filtering and displaying alternative products
    /// </summary>
    public class RecommendationsController : MonoBehaviour
    {
        [Header("UI References")]
        public Transform productListContainer;
        public GameObject productCardPrefab;
        public Button backButton;
        
        [Header("Filter Toggles")]
        public Toggle bioToggle;
        public Toggle ecoToggle;
        public Toggle priceToggle;
        public Toggle healthToggle;
        
        [Header("Filter Settings")]
        public float maxPriceFilter = 20f;
        public float minHealthScoreFilter = 6f;
        
        private List<ProductData> _alternatives;
        private List<GameObject> _productCards = new List<GameObject>();
        
        private void Start()
        {
            // Setup button listeners
            if (backButton != null)
            {
                backButton.onClick.AddListener(OnBackButtonClicked);
            }
            
            // Setup filter toggles
            SetupFilterToggles();
            
            // Load alternatives
            LoadAlternatives();
        }
        
        /// <summary>
        /// Setup filter toggle listeners
        /// </summary>
        private void SetupFilterToggles()
        {
            if (bioToggle != null)
            {
                bioToggle.onValueChanged.AddListener((value) => ApplyFilters());
            }
            
            if (ecoToggle != null)
            {
                ecoToggle.onValueChanged.AddListener((value) => ApplyFilters());
            }
            
            if (priceToggle != null)
            {
                priceToggle.onValueChanged.AddListener((value) => ApplyFilters());
            }
            
            if (healthToggle != null)
            {
                healthToggle.onValueChanged.AddListener((value) => ApplyFilters());
            }
        }
        
        /// <summary>
        /// Load alternative products
        /// </summary>
        private void LoadAlternatives()
        {
            string productId = QRCodeManager.Instance.GetCurrentProductId();
            
            if (string.IsNullOrEmpty(productId))
            {
                Debug.LogError("No product ID available");
                return;
            }
            
            // Get alternatives from database
            _alternatives = ProductDatabase.Instance.GetAlternatives(productId);
            
            if (_alternatives == null || _alternatives.Count == 0)
            {
                Debug.Log($"No alternatives found for product {productId}");
                _alternatives = new List<ProductData>();
            }
            
            // Display alternatives
            DisplayAlternatives(_alternatives);
        }
        
        /// <summary>
        /// Apply filters to alternatives
        /// </summary>
        private void ApplyFilters()
        {
            if (_alternatives == null) return;
            
            var filtered = _alternatives.AsEnumerable();
            
            // Bio filter
            if (bioToggle != null && bioToggle.isOn)
            {
                filtered = filtered.Where(p => p.tags != null && p.tags.Contains("Bio"));
            }
            
            // Eco-friendly filter
            if (ecoToggle != null && ecoToggle.isOn)
            {
                filtered = filtered.Where(p => p.ecoScore >= 7f);
            }
            
            // Price filter
            if (priceToggle != null && priceToggle.isOn)
            {
                filtered = filtered.Where(p => p.price <= maxPriceFilter);
            }
            
            // Health score filter
            if (healthToggle != null && healthToggle.isOn)
            {
                filtered = filtered.Where(p => p.healthScore >= minHealthScoreFilter);
            }
            
            var filteredList = filtered.ToList();
            DisplayAlternatives(filteredList);
        }
        
        /// <summary>
        /// Display alternatives in UI
        /// </summary>
        private void DisplayAlternatives(List<ProductData> products)
        {
            // Clear existing cards
            ClearProductCards();
            
            if (products == null || products.Count == 0)
            {
                Debug.Log("No products to display");
                return;
            }
            
            // Create product cards
            foreach (var product in products)
            {
                CreateProductCard(product);
            }
        }
        
        /// <summary>
        /// Create a product card for an alternative
        /// </summary>
        private void CreateProductCard(ProductData product)
        {
            if (productListContainer == null)
            {
                Debug.LogError("Product list container is not assigned");
                return;
            }
            
            GameObject card;
            
            // Create card from prefab or simple UI element
            if (productCardPrefab != null)
            {
                card = Instantiate(productCardPrefab, productListContainer);
            }
            else
            {
                // Create simple card if prefab not available
                card = CreateSimpleCard();
            }
            
            // Setup card with product data
            var productCard = card.GetComponent<ProductCard>();
            if (productCard != null)
            {
                productCard.SetProduct(product);
            }
            else
            {
                // Fallback: set data directly
                SetupSimpleCard(card, product);
            }
            
            _productCards.Add(card);
        }
        
        /// <summary>
        /// Create simple product card without prefab
        /// </summary>
        private GameObject CreateSimpleCard()
        {
            GameObject card = new GameObject("ProductCard");
            card.transform.SetParent(productListContainer);
            
            // Add layout components
            var rectTransform = card.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(400, 120);
            
            var image = card.AddComponent<Image>();
            image.color = new Color(0.9f, 0.9f, 0.9f, 1f);
            
            return card;
        }
        
        /// <summary>
        /// Setup simple card with product data
        /// </summary>
        private void SetupSimpleCard(GameObject card, ProductData product)
        {
            // This is a placeholder - cards should use ProductCard component
            card.name = $"Card_{product.id}";
        }
        
        /// <summary>
        /// Clear all product cards
        /// </summary>
        private void ClearProductCards()
        {
            foreach (var card in _productCards)
            {
                if (card != null)
                {
                    Destroy(card);
                }
            }
            
            _productCards.Clear();
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
            // Cleanup
            ClearProductCards();
            
            if (backButton != null)
            {
                backButton.onClick.RemoveListener(OnBackButtonClicked);
            }
        }
    }
}
