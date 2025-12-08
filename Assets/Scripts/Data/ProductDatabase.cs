using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace SmartRetailAR.Data
{
    /// <summary>
    /// Singleton manager for the product database
    /// Loads and manages product data from JSON
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

        private List<ProductData> products = new List<ProductData>();
        private Dictionary<string, ProductData> productDictionary = new Dictionary<string, ProductData>();
        private bool isLoaded = false;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                LoadDatabase();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void LoadDatabase()
        {
            if (isLoaded) return;

            try
            {
                TextAsset jsonFile = Resources.Load<TextAsset>("Data/products_database");
                if (jsonFile != null)
                {
                    ProductDatabaseWrapper wrapper = JsonUtility.FromJson<ProductDatabaseWrapper>(jsonFile.text);
                    if (wrapper != null && wrapper.products != null)
                    {
                        products = wrapper.products;
                        
                        // Build dictionary for fast lookup
                        productDictionary.Clear();
                        foreach (var product in products)
                        {
                            if (!string.IsNullOrEmpty(product.id))
                            {
                                productDictionary[product.id] = product;
                            }
                        }

                        isLoaded = true;
                        Debug.Log($"Product database loaded successfully. {products.Count} products available.");
                    }
                    else
                    {
                        Debug.LogError("Failed to parse product database JSON");
                    }
                }
                else
                {
                    Debug.LogError("Product database JSON file not found in Resources/Data/");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error loading product database: {e.Message}");
            }
        }

        /// <summary>
        /// Get a product by its ID
        /// </summary>
        public ProductData GetProductById(string id)
        {
            if (!isLoaded) LoadDatabase();
            
            if (productDictionary.ContainsKey(id))
            {
                return productDictionary[id];
            }
            
            Debug.LogWarning($"Product with ID {id} not found in database");
            return null;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        public List<ProductData> GetAllProducts()
        {
            if (!isLoaded) LoadDatabase();
            return new List<ProductData>(products);
        }

        /// <summary>
        /// Get products by category
        /// </summary>
        public List<ProductData> GetProductsByCategory(string category)
        {
            if (!isLoaded) LoadDatabase();
            return products.Where(p => p.category == category).ToList();
        }

        /// <summary>
        /// Get products by brand
        /// </summary>
        public List<ProductData> GetProductsByBrand(string brand)
        {
            if (!isLoaded) LoadDatabase();
            return products.Where(p => p.brand == brand).ToList();
        }

        /// <summary>
        /// Search products by name
        /// </summary>
        public List<ProductData> SearchProducts(string searchTerm)
        {
            if (!isLoaded) LoadDatabase();
            
            if (string.IsNullOrEmpty(searchTerm))
                return new List<ProductData>(products);

            searchTerm = searchTerm.ToLower();
            return products.Where(p => 
                p.name.ToLower().Contains(searchTerm) || 
                p.description.ToLower().Contains(searchTerm) ||
                p.category.ToLower().Contains(searchTerm)
            ).ToList();
        }

        /// <summary>
        /// Get all unique categories
        /// </summary>
        public List<string> GetAllCategories()
        {
            if (!isLoaded) LoadDatabase();
            return products.Select(p => p.category).Distinct().ToList();
        }

        /// <summary>
        /// Get product count
        /// </summary>
        public int GetProductCount()
        {
            if (!isLoaded) LoadDatabase();
            return products.Count;
        }
    }
}
