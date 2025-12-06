using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ProductDatabaseWrapper
{
    public List<ProductData> products;
}

public class ProductDatabase : MonoBehaviour
{
    private static ProductDatabase instance;
    public static ProductDatabase Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("ProductDatabase");
                instance = go.AddComponent<ProductDatabase>();
                DontDestroyOnLoad(go);
                instance.LoadDatabase();
            }
            return instance;
        }
    }

    private Dictionary<string, ProductData> productsById = new Dictionary<string, ProductData>();
    private List<ProductData> allProducts = new List<ProductData>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadDatabase();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void LoadDatabase()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Data/products_database");
        if (jsonFile != null)
        {
            ProductDatabaseWrapper wrapper = JsonUtility.FromJson<ProductDatabaseWrapper>(jsonFile.text);
            if (wrapper != null && wrapper.products != null)
            {
                allProducts = wrapper.products;
                productsById.Clear();
                foreach (var product in allProducts)
                {
                    productsById[product.productId] = product;
                }
                Debug.Log($"Loaded {allProducts.Count} products from database");
            }
        }
        else
        {
            Debug.LogWarning("products_database.json not found in Resources/Data/");
        }
    }

    public ProductData GetProductById(string productId)
    {
        if (productsById.ContainsKey(productId))
        {
            return productsById[productId];
        }
        Debug.LogWarning($"Product with ID {productId} not found");
        return null;
    }

    public List<ProductData> GetAllProducts()
    {
        return new List<ProductData>(allProducts);
    }

    public List<ProductData> GetAlternatives(string productId)
    {
        ProductData product = GetProductById(productId);
        if (product == null || product.alternativeIds == null)
        {
            return new List<ProductData>();
        }

        List<ProductData> alternatives = new List<ProductData>();
        foreach (string altId in product.alternativeIds)
        {
            ProductData alt = GetProductById(altId);
            if (alt != null)
            {
                alternatives.Add(alt);
            }
        }
        return alternatives;
    }

    public List<ProductData> FilterProducts(List<ProductData> products, List<string> tags, float? minHealthScore = null, float? maxPrice = null)
    {
        var filtered = products.AsEnumerable();

        if (tags != null && tags.Count > 0)
        {
            filtered = filtered.Where(p => p.tags != null && tags.Any(tag => p.tags.Contains(tag)));
        }

        if (minHealthScore.HasValue)
        {
            filtered = filtered.Where(p => p.scores.healthScore >= minHealthScore.Value);
        }

        if (maxPrice.HasValue)
        {
            filtered = filtered.Where(p => p.price <= maxPrice.Value);
        }

        return filtered.ToList();
    }

    public List<ProductData> SearchProducts(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            return allProducts;
        }

        query = query.ToLower();
        return allProducts.Where(p => 
            p.name.ToLower().Contains(query) || 
            p.brand.ToLower().Contains(query)
        ).ToList();
    }
}
