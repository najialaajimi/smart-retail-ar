using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProductData
{
    public string productId;
    public string name;
    public string brand;
    public string imageUrl;
    public NutritionalInfo nutritionalInfo;
    public OriginInfo origin;
    public Scores scores;
    public float price;
    public List<string> alternativeIds;
    public List<string> tags; // Bio, Éco-responsable, etc.

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
    }

    [Serializable]
    public class OriginInfo
    {
        public string country;
        public string region;
        public string producer;
    }

    [Serializable]
    public class Scores
    {
        public float healthScore; // 0-100
        public float ecoScore; // 0-100
        public string nutritionGrade; // A, B, C, D, E
    }

    public ProductData()
    {
        nutritionalInfo = new NutritionalInfo();
        origin = new OriginInfo();
        scores = new Scores();
        alternativeIds = new List<string>();
        tags = new List<string>();
    }
}
