using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SmartRetailAR.Data
{
    /// <summary>
    /// Singleton manager for product database
    /// Handles loading and querying product data
    /// </summary>
    public class ProductDatabase : MonoBehaviour
    {
        private static ProductDatabase _instance;
        public static ProductDatabase Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("ProductDatabase");
                    _instance = go.AddComponent<ProductDatabase>();
                    DontDestroyOnLoad(go);
                    _instance.LoadDatabase();
                }
                return _instance;
            }
        }
        
        private Dictionary<string, ProductData> _products;
        private List<ProductData> _productList;
        private bool _isLoaded = false;
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject);
            LoadDatabase();
        }
        
        /// <summary>
        /// Load product database from Resources
        /// </summary>
        private void LoadDatabase()
        {
            if (_isLoaded) return;
            
            TextAsset jsonFile = Resources.Load<TextAsset>("Data/products_database");
            if (jsonFile == null)
            {
                Debug.LogError("Could not load products_database.json from Resources/Data/");
                _products = new Dictionary<string, ProductData>();
                _productList = new List<ProductData>();
                _isLoaded = true;
                return;
            }
            
            try
            {
                Data.ProductDatabase database = JsonUtility.FromJson<Data.ProductDatabase>(jsonFile.text);
                _productList = database.products ?? new List<ProductData>();
                
                _products = new Dictionary<string, ProductData>();
                foreach (var product in _productList)
                {
                    if (!string.IsNullOrEmpty(product.id))
                    {
                        _products[product.id] = product;
                    }
                }
                
                _isLoaded = true;
                Debug.Log($"Loaded {_products.Count} products from database");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error parsing products_database.json: {e.Message}");
                _products = new Dictionary<string, ProductData>();
                _productList = new List<ProductData>();
                _isLoaded = true;
            }
        }
        
        /// <summary>
        /// Get product by ID
        /// </summary>
        public ProductData GetProduct(string productId)
        {
            if (!_isLoaded) LoadDatabase();
            
            if (_products.TryGetValue(productId, out ProductData product))
            {
                return product;
            }
            
            Debug.LogWarning($"Product with ID '{productId}' not found");
            return null;
        }
        
        /// <summary>
        /// Get all products
        /// </summary>
        public List<ProductData> GetAllProducts()
        {
            if (!_isLoaded) LoadDatabase();
            return new List<ProductData>(_productList);
        }
        
        /// <summary>
        /// Get products by category
        /// </summary>
        public List<ProductData> GetProductsByCategory(string category)
        {
            if (!_isLoaded) LoadDatabase();
            
            return _productList
                .Where(p => p.categories != null && p.categories.Contains(category))
                .ToList();
        }
        
        /// <summary>
        /// Get products by tag
        /// </summary>
        public List<ProductData> GetProductsByTag(string tag)
        {
            if (!_isLoaded) LoadDatabase();
            
            return _productList
                .Where(p => p.tags != null && p.tags.Contains(tag))
                .ToList();
        }
        
        /// <summary>
        /// Filter products by criteria
        /// </summary>
        public List<ProductData> FilterProducts(
            bool? isBio = null,
            bool? isEcoFriendly = null,
            float? maxPrice = null,
            float? minHealthScore = null)
        {
            if (!_isLoaded) LoadDatabase();
            
            var filtered = _productList.AsEnumerable();
            
            if (isBio.HasValue && isBio.Value)
            {
                filtered = filtered.Where(p => p.tags != null && p.tags.Contains("Bio"));
            }
            
            if (isEcoFriendly.HasValue && isEcoFriendly.Value)
            {
                filtered = filtered.Where(p => p.ecoScore >= 7.0f);
            }
            
            if (maxPrice.HasValue)
            {
                filtered = filtered.Where(p => p.price <= maxPrice.Value);
            }
            
            if (minHealthScore.HasValue)
            {
                filtered = filtered.Where(p => p.healthScore >= minHealthScore.Value);
            }
            
            return filtered.ToList();
        }
        
        /// <summary>
        /// Get alternative products for a given product
        /// </summary>
        public List<ProductData> GetAlternatives(string productId)
        {
            var product = GetProduct(productId);
            if (product == null || product.alternativeIds == null || product.alternativeIds.Count == 0)
            {
                return new List<ProductData>();
            }
            
            List<ProductData> alternatives = new List<ProductData>();
            foreach (var altId in product.alternativeIds)
            {
                var altProduct = GetProduct(altId);
                if (altProduct != null)
                {
                    alternatives.Add(altProduct);
                }
            }
            
            return alternatives;
        }
    }
}
