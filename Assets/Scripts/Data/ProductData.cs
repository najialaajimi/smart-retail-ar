using System;
using System.Collections.Generic;
using UnityEngine;

namespace SmartRetailAR.Data
{
    /// <summary>
    /// Data structure for product information
    /// </summary>
    [Serializable]
    public class ProductData
    {
        public string id;
        public string name;
        public string brand;
        public string description;
        public string imageUrl;
        public string origin;
        public string originRegion;
        
        // Nutritional information
        public NutritionalInfo nutritionalInfo;
        
        // Scores
        public float healthScore;
        public float ecoScore;
        public float price;
        
        // Categories and tags
        public List<string> categories;
        public List<string> tags;
        
        // Alternatives
        public List<string> alternativeIds;
    }
    
    [Serializable]
    public class NutritionalInfo
    {
        public float calories;
        public float proteins;
        public float carbohydrates;
        public float fats;
        public float fiber;
        public float sugar;
        public float salt;
        public string servingSize;
    }
    
    /// <summary>
    /// Wrapper for product database JSON deserialization
    /// </summary>
    [Serializable]
    public class ProductDatabaseWrapper
    {
        public List<ProductData> products;
    }
}
