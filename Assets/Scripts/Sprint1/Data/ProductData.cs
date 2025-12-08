using System;
using System.Collections.Generic;
using UnityEngine;

namespace SmartRetailAR.Data
{
    [Serializable]
    public class NutritionalInfo
    {
        public string servingSize;
        public int calories;
        public float proteins;
        public float carbohydrates;
        public float fats;
        public float fibers;
        public float salt;
        public float sugars;
    }

    [Serializable]
    public class Scores
    {
        public string ecoScore;
        public string healthScore;
        public string nutriScore;
        public float socialScore;
    }

    [Serializable]
    public class ProductFlags
    {
        public bool isBio;
        public bool isVegan;
        public bool isVegetarian;
        public bool isGlutenFree;
        public bool isLactoseFree;
    }

    [Serializable]
    public class ProductData
    {
        public string id;
        public string name;
        public string brand;
        public string barcode;
        public string qrCode;
        public string category;
        public string subcategory;
        public string description;
        public string origin;
        public float price;
        public string currency;
        public string weight;
        public NutritionalInfo nutritionalInfo;
        public Scores scores;
        public List<string> certifications;
        public ProductFlags flags;
        public string imageUrl;
        public string model3D;
        public string availability;
        public int stockQuantity;
        public List<string> alternatives;
    }

    [Serializable]
    public class ProductDatabaseWrapper
    {
        public string version;
        public string lastUpdate;
        public int totalProducts;
        public List<ProductData> products;
    }
}
