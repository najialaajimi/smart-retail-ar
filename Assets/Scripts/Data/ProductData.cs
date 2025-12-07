using System;
using System.Collections.Generic;

namespace SmartRetailAR.Data
{
    /// <summary>
    /// Data structure representing a product in the database
    /// </summary>
    [Serializable]
    public class ProductData
    {
        public string id;
        public string name;
        public string category;
        public string brand;
        public float price;
        public string description;
        public NutritionalInfo nutritionalInfo;
        public EcologicalInfo ecologicalInfo;
        public EthicalInfo ethicalInfo;
        public List<string> allergens;
        public string imageUrl;
        public string barcode;

        // Additional fields for recommendations
        [NonSerialized]
        public float relevanceScore;
        [NonSerialized]
        public float similarityScore;
    }

    [Serializable]
    public class NutritionalInfo
    {
        public int calories;
        public float protein;
        public float carbs;
        public float fat;
        public float fiber;
        public float sugar;
        public int sodium;
        public string nutriScore;
    }

    [Serializable]
    public class EcologicalInfo
    {
        public float carbonFootprint;
        public string packaging;
        public string origin;
        public bool organic;
        public string ecoScore;
    }

    [Serializable]
    public class EthicalInfo
    {
        public bool fairTrade;
        public bool local;
        public string ethicalScore;
    }

    /// <summary>
    /// Wrapper class for deserializing the product database JSON
    /// </summary>
    [Serializable]
    public class ProductDatabaseWrapper
    {
        public List<ProductData> products;
    }
}
