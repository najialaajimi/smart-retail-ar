using System;
using System.Collections.Generic;
using UnityEngine;

namespace SmartRetailAR.Data
{
    /// <summary>
    /// Represents a product with all its information including nutritional data,
    /// certifications, and alternatives.
    /// </summary>
    [Serializable]
    public class ProductData
    {
        public string id;
        public string name;
        public string brand;
        public string category;
        public string origin;
        public float price;
        public string barcode;
        public string qrCode;
        
        // Nutritional information
        public NutritionalInfo nutritionalInfo;
        
        // Scores
        public float ecoScore; // 0-100
        public float healthScore; // 0-100
        public float qualityScore; // 0-100
        
        // Certifications
        public List<string> certifications;
        
        // Tags
        public bool isBio;
        public bool isLocal;
        public bool isVegan;
        public bool isGlutenFree;
        public bool isFairTrade;
        
        // Alternatives
        public List<string> alternativeIds;
        
        // Additional info
        public string description;
        public string imageUrl;
        public string manufacturer;
        public DateTime expirationDate;
        
        public ProductData()
        {
            certifications = new List<string>();
            alternativeIds = new List<string>();
        }
    }
    
    [Serializable]
    public class NutritionalInfo
    {
        public float servingSize; // in grams
        public float calories;
        public float protein;
        public float carbohydrates;
        public float sugars;
        public float fats;
        public float saturatedFats;
        public float fiber;
        public float sodium;
        public float cholesterol;
        
        // Vitamins and minerals (percentage of daily value)
        public float vitaminA;
        public float vitaminC;
        public float calcium;
        public float iron;
    }
    
    [Serializable]
    public class ProductDatabase
    {
        public List<ProductData> products;
        
        public ProductDatabase()
        {
            products = new List<ProductData>();
        }
    }
}
