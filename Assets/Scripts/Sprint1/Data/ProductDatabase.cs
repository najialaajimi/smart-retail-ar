using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SmartRetailAR.Data
{
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

        private ProductDatabaseWrapper _database;
        private Dictionary<string, ProductData> _productById;
        private Dictionary<string, ProductData> _productByBarcode;
        private Dictionary<string, ProductData> _productByQRCode;
        private bool _isLoaded = false;

        void Awake()
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
            if (_isLoaded) return;

            TextAsset jsonFile = Resources.Load<TextAsset>("Data/products_database");
            if (jsonFile != null)
            {
                _database = JsonUtility.FromJson<ProductDatabaseWrapper>(jsonFile.text);
                InitializeDictionaries();
                _isLoaded = true;
                Debug.Log($"Loaded {_database.totalProducts} products from database");
            }
            else
            {
                Debug.LogError("Failed to load products_database.json from Resources/Data/");
            }
        }

        private void InitializeDictionaries()
        {
            _productById = new Dictionary<string, ProductData>();
            _productByBarcode = new Dictionary<string, ProductData>();
            _productByQRCode = new Dictionary<string, ProductData>();

            foreach (var product in _database.products)
            {
                _productById[product.id] = product;
                _productByBarcode[product.barcode] = product;
                _productByQRCode[product.qrCode] = product;
            }
        }

        public ProductData GetProductById(string id)
        {
            if (_productById != null && _productById.ContainsKey(id))
                return _productById[id];
            return null;
        }

        public ProductData GetProductByBarcode(string barcode)
        {
            if (_productByBarcode != null && _productByBarcode.ContainsKey(barcode))
                return _productByBarcode[barcode];
            return null;
        }

        public ProductData GetProductByQRCode(string qrCode)
        {
            if (_productByQRCode != null && _productByQRCode.ContainsKey(qrCode))
                return _productByQRCode[qrCode];
            return null;
        }

        public List<ProductData> GetAllProducts()
        {
            return _database?.products ?? new List<ProductData>();
        }

        public List<ProductData> SearchProducts(string query)
        {
            if (string.IsNullOrEmpty(query) || _database == null)
                return new List<ProductData>();

            query = query.ToLower();
            return _database.products.Where(p =>
                p.name.ToLower().Contains(query) ||
                p.brand.ToLower().Contains(query) ||
                p.category.ToLower().Contains(query) ||
                p.description.ToLower().Contains(query)
            ).ToList();
        }

        public List<ProductData> FilterProducts(string category = null, bool? isBio = null, 
            bool? isVegan = null, string ecoScore = null, float? maxPrice = null)
        {
            if (_database == null) return new List<ProductData>();

            var filtered = _database.products.AsEnumerable();

            if (!string.IsNullOrEmpty(category))
                filtered = filtered.Where(p => p.category == category);

            if (isBio.HasValue)
                filtered = filtered.Where(p => p.flags.isBio == isBio.Value);

            if (isVegan.HasValue)
                filtered = filtered.Where(p => p.flags.isVegan == isVegan.Value);

            if (!string.IsNullOrEmpty(ecoScore))
                filtered = filtered.Where(p => p.scores.ecoScore == ecoScore);

            if (maxPrice.HasValue)
                filtered = filtered.Where(p => p.price <= maxPrice.Value);

            return filtered.ToList();
        }

        public List<ProductData> GetAlternativeProducts(string productId)
        {
            var product = GetProductById(productId);
            if (product == null || product.alternatives == null)
                return new List<ProductData>();

            return product.alternatives
                .Select(altId => GetProductById(altId))
                .Where(alt => alt != null)
                .ToList();
        }

        public int GetTotalProducts()
        {
            return _database?.totalProducts ?? 0;
        }
    }
}
