using UnityEngine;
using SmartRetailAR.Data;
using System.Collections.Generic;
using System.Linq;

namespace SmartRetailAR.Recommendations
{
    public class RecommendationEngine : MonoBehaviour
    {
        private static RecommendationEngine _instance;
        public static RecommendationEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("RecommendationEngine");
                    _instance = go.AddComponent<RecommendationEngine>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        [Header("Recommendation Settings")]
        public int maxRecommendations = 10;
        public float similarityThreshold = 0.6f;
        public bool enablePersonalization = true;

        [Header("Weights")]
        [Range(0f, 1f)] public float categoryWeight = 0.3f;
        [Range(0f, 1f)] public float priceWeight = 0.2f;
        [Range(0f, 1f)] public float scoreWeight = 0.3f;
        [Range(0f, 1f)] public float userPreferenceWeight = 0.2f;

        private Dictionary<string, float> _productScores = new Dictionary<string, float>();

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public List<ProductData> GetRecommendations(ProductData baseProduct, UserPreferencesData userPrefs = null)
        {
            if (baseProduct == null)
                return new List<ProductData>();

            List<ProductData> allProducts = ProductDatabase.Instance.GetAllProducts();
            List<RecommendationScore> scoredProducts = new List<RecommendationScore>();

            foreach (var product in allProducts)
            {
                if (product.id == baseProduct.id)
                    continue;

                float score = CalculateRecommendationScore(baseProduct, product, userPrefs);
                if (score >= similarityThreshold)
                {
                    scoredProducts.Add(new RecommendationScore { product = product, score = score });
                }
            }

            return scoredProducts
                .OrderByDescending(x => x.score)
                .Take(maxRecommendations)
                .Select(x => x.product)
                .ToList();
        }

        private float CalculateRecommendationScore(ProductData baseProduct, ProductData candidate, UserPreferencesData userPrefs)
        {
            float totalScore = 0f;

            // Category similarity
            float categoryScore = baseProduct.category == candidate.category ? 1f : 0f;
            totalScore += categoryScore * categoryWeight;

            // Price similarity
            float priceDiff = Mathf.Abs(baseProduct.price - candidate.price);
            float priceScore = 1f - Mathf.Clamp01(priceDiff / baseProduct.price);
            totalScore += priceScore * priceWeight;

            // Score similarity (eco, health, nutri)
            float scoreScore = CalculateScoreSimilarity(baseProduct.scores, candidate.scores);
            totalScore += scoreScore * scoreWeight;

            // User preferences
            if (enablePersonalization && userPrefs != null)
            {
                float prefScore = CalculateUserPreferenceScore(candidate, userPrefs);
                totalScore += prefScore * userPreferenceWeight;
            }

            return totalScore;
        }

        private float CalculateScoreSimilarity(Scores score1, Scores score2)
        {
            if (score1 == null || score2 == null)
                return 0f;

            float ecoSim = score1.ecoScore == score2.ecoScore ? 1f : 0.5f;
            float healthSim = score1.healthScore == score2.healthScore ? 1f : 0.5f;
            float nutriSim = score1.nutriScore == score2.nutriScore ? 1f : 0.5f;

            return (ecoSim + healthSim + nutriSim) / 3f;
        }

        private float CalculateUserPreferenceScore(ProductData product, UserPreferencesData prefs)
        {
            float score = 0f;
            int criteria = 0;

            if (prefs.preferBio && product.flags.isBio)
            {
                score += 1f;
                criteria++;
            }

            if (prefs.preferVegan && product.flags.isVegan)
            {
                score += 1f;
                criteria++;
            }

            if (prefs.preferLocal && product.origin == prefs.preferredOrigin)
            {
                score += 1f;
                criteria++;
            }

            return criteria > 0 ? score / criteria : 0.5f;
        }

        public List<ProductData> GetAlternativeProducts(string productId, bool betterScore = true)
        {
            ProductData baseProduct = ProductDatabase.Instance.GetProductById(productId);
            if (baseProduct == null)
                return new List<ProductData>();

            List<ProductData> alternatives = ProductDatabase.Instance.GetAlternativeProducts(productId);
            
            if (betterScore)
            {
                alternatives = alternatives.Where(p => 
                    GetScoreValue(p.scores.ecoScore) >= GetScoreValue(baseProduct.scores.ecoScore) ||
                    GetScoreValue(p.scores.healthScore) >= GetScoreValue(baseProduct.scores.healthScore)
                ).ToList();
            }

            return alternatives;
        }

        private int GetScoreValue(string score)
        {
            switch (score)
            {
                case "A": return 5;
                case "B": return 4;
                case "C": return 3;
                case "D": return 2;
                case "E": return 1;
                default: return 0;
            }
        }

        private class RecommendationScore
        {
            public ProductData product;
            public float score;
        }
    }

    [System.Serializable]
    public class UserPreferencesData
    {
        public bool preferBio;
        public bool preferVegan;
        public bool preferLocal;
        public string preferredOrigin;
        public float maxPrice;
        public List<string> favoriteCategories;
    }
}
