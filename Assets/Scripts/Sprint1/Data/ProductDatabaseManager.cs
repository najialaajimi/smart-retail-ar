using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SmartRetailAR.Data
{
    /// <summary>
    /// Singleton manager for product database operations.
    /// Handles loading, searching, and filtering products.
    /// </summary>
    public class ProductDatabaseManager : MonoBehaviour
    {
        private static ProductDatabaseManager _instance;
        public static ProductDatabaseManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("ProductDatabaseManager");
                    _instance = go.AddComponent<ProductDatabaseManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }
        
        private ProductDatabase _database;
        private Dictionary<string, ProductData> _productLookup;
        private bool _isLoaded = false;
        
        void Awake()
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
        /// Loads the product database from Resources/Data/products_database.json
        /// </summary>
        public void LoadDatabase()
        {
            if (_isLoaded) return;
            
            TextAsset jsonFile = Resources.Load<TextAsset>("Data/products_database");
            if (jsonFile != null)
            {
                _database = JsonUtility.FromJson<ProductDatabase>(jsonFile.text);
                BuildLookupDictionary();
                _isLoaded = true;
                Debug.Log($"Product database loaded: {_database.products.Count} products");
            }
            else
            {
                Debug.LogError("Failed to load product database from Resources/Data/products_database.json");
                _database = new ProductDatabase();
            }
        }
        
        private void BuildLookupDictionary()
        {
            _productLookup = new Dictionary<string, ProductData>();
            foreach (var product in _database.products)
            {
                if (!_productLookup.ContainsKey(product.id))
                {
                    _productLookup.Add(product.id, product);
                }
            }
        }
        
        /// <summary>
        /// Gets a product by its ID
        /// </summary>
        public ProductData GetProductById(string id)
        {
            if (_productLookup != null && _productLookup.ContainsKey(id))
            {
                return _productLookup[id];
            }
            return null;
        }
        
        /// <summary>
        /// Gets a product by QR code
        /// </summary>
        public ProductData GetProductByQRCode(string qrCode)
        {
            return _database.products.FirstOrDefault(p => p.qrCode == qrCode);
        }
        
        /// <summary>
        /// Gets a product by barcode
        /// </summary>
        public ProductData GetProductByBarcode(string barcode)
        {
            return _database.products.FirstOrDefault(p => p.barcode == barcode);
        }
        
        /// <summary>
        /// Searches products by name or brand
        /// </summary>
        public List<ProductData> SearchProducts(string query)
        {
            query = query.ToLower();
            return _database.products
                .Where(p => p.name.ToLower().Contains(query) || 
                           p.brand.ToLower().Contains(query) ||
                           p.category.ToLower().Contains(query))
                .ToList();
        }
        
        /// <summary>
        /// Gets products by category
        /// </summary>
        public List<ProductData> GetProductsByCategory(string category)
        {
            return _database.products
                .Where(p => p.category.Equals(category, System.StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        
        /// <summary>
        /// Gets all products
        /// </summary>
        public List<ProductData> GetAllProducts()
        {
            return _database.products;
        }
        
        /// <summary>
        /// Gets alternative products for a given product
        /// </summary>
        public List<ProductData> GetAlternatives(string productId)
        {
            var product = GetProductById(productId);
            if (product == null || product.alternativeIds == null)
                return new List<ProductData>();
                
            return product.alternativeIds
                .Select(id => GetProductById(id))
                .Where(p => p != null)
                .ToList();
        }
        
        /// <summary>
        /// Gets products filtered by tags
        /// </summary>
        public List<ProductData> GetFilteredProducts(bool? isBio = null, bool? isLocal = null, 
            bool? isVegan = null, bool? isGlutenFree = null, bool? isFairTrade = null)
        {
            var filtered = _database.products.AsEnumerable();
            
            if (isBio.HasValue)
                filtered = filtered.Where(p => p.isBio == isBio.Value);
            if (isLocal.HasValue)
                filtered = filtered.Where(p => p.isLocal == isLocal.Value);
            if (isVegan.HasValue)
                filtered = filtered.Where(p => p.isVegan == isVegan.Value);
            if (isGlutenFree.HasValue)
                filtered = filtered.Where(p => p.isGlutenFree == isGlutenFree.Value);
            if (isFairTrade.HasValue)
                filtered = filtered.Where(p => p.isFairTrade == isFairTrade.Value);
                
            return filtered.ToList();
        }
        
        public bool IsLoaded => _isLoaded;
    }
}
