using UnityEngine;
using SmartRetailAR.Data;
using System.Collections.Generic;
using System.Linq;

namespace SmartRetailAR.Recommendations
{
    /// <summary>
    /// Advanced recommendation engine with multi-criteria scoring
    /// Implements personalized product recommendations based on user preferences
    /// </summary>
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
        [SerializeField] private int maxRecommendations = 5;
        [SerializeField] private float similarityThreshold = 0.3f;
        [SerializeField] private bool enableMLRecommendations = false;

        private ScoringSystem scoringSystem;
        private UserPreferences userPreferences;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                Initialize();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Initialize()
        {
            scoringSystem = GetComponent<ScoringSystem>() ?? gameObject.AddComponent<ScoringSystem>();
            userPreferences = UserPreferencesManager.Instance.GetPreferences();
        }

        /// <summary>
        /// Get personalized recommendations for a product
        /// </summary>
        public List<ProductData> GetRecommendations(ProductData currentProduct, int count = 5)
        {
            if (currentProduct == null)
            {
                Debug.LogWarning("Cannot get recommendations for null product");
                return new List<ProductData>();
            }

            List<ProductData> allProducts = ProductDatabase.Instance.GetAllProducts();
            List<ProductData> recommendations = new List<ProductData>();

            // Filter out current product and products that don't match user preferences
            allProducts = allProducts
                .Where(p => p.id != currentProduct.id)
                .Where(p => userPreferences.MatchesDietaryPreferences(p))
                .ToList();

            // Score all products
            foreach (var product in allProducts)
            {
                float score = CalculateRecommendationScore(currentProduct, product);
                product.relevanceScore = score;
            }

            // Sort by score and return top N
            recommendations = allProducts
                .OrderByDescending(p => p.relevanceScore)
                .Take(Mathf.Min(count, maxRecommendations))
                .ToList();

            return recommendations;
        }

        /// <summary>
        /// Get similar products based on category and attributes
        /// </summary>
        public List<ProductData> GetSimilarProducts(ProductData product, int count = 3)
        {
            if (product == null) return new List<ProductData>();

            List<ProductData> allProducts = ProductDatabase.Instance.GetAllProducts();
            List<ProductData> similarProducts = new List<ProductData>();

            foreach (var p in allProducts)
            {
                if (p.id == product.id) continue;

                float similarity = CalculateSimilarityScore(product, p);
                p.similarityScore = similarity;

                if (similarity >= similarityThreshold)
                {
                    similarProducts.Add(p);
                }
            }

            return similarProducts
                .OrderByDescending(p => p.similarityScore)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Get alternative products (better options)
        /// </summary>
        public List<ProductData> GetAlternatives(ProductData product, int count = 3)
        {
            if (product == null) return new List<ProductData>();

            List<ProductData> categoryProducts = ProductDatabase.Instance
                .GetProductsByCategory(product.category);

            List<ProductData> alternatives = new List<ProductData>();

            foreach (var p in categoryProducts)
            {
                if (p.id == product.id) continue;
                if (!userPreferences.MatchesDietaryPreferences(p)) continue;

                // Calculate overall score
                float score = scoringSystem.CalculateOverallScore(p, userPreferences);
                float currentScore = scoringSystem.CalculateOverallScore(product, userPreferences);

                // Only include if better than current product
                if (score > currentScore)
                {
                    p.relevanceScore = score;
                    alternatives.Add(p);
                }
            }

            return alternatives
                .OrderByDescending(p => p.relevanceScore)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Calculate recommendation score between current and candidate product
        /// </summary>
        private float CalculateRecommendationScore(ProductData currentProduct, ProductData candidate)
        {
            float score = 0f;

            // Category similarity (30%)
            if (currentProduct.category == candidate.category)
            {
                score += 0.3f;
            }

            // User preference matching (30%)
            float preferenceScore = scoringSystem.CalculateOverallScore(candidate, userPreferences);
            score += preferenceScore * 0.3f;

            // Similarity score (20%)
            float similarity = CalculateSimilarityScore(currentProduct, candidate);
            score += similarity * 0.2f;

            // User history (20%)
            int scanCount = userPreferences.GetProductScanCount(candidate.id);
            float historyScore = Mathf.Min(scanCount / 10f, 1f);
            score += historyScore * 0.2f;

            return score;
        }

        /// <summary>
        /// Calculate similarity between two products
        /// </summary>
        private float CalculateSimilarityScore(ProductData product1, ProductData product2)
        {
            float score = 0f;
            int criteria = 0;

            // Category match
            if (product1.category == product2.category)
            {
                score += 1f;
            }
            criteria++;

            // Brand match
            if (product1.brand == product2.brand)
            {
                score += 0.5f;
            }
            criteria++;

            // Price similarity (within 20%)
            float priceDiff = Mathf.Abs(product1.price - product2.price) / product1.price;
            if (priceDiff <= 0.2f)
            {
                score += 1f - priceDiff;
            }
            criteria++;

            // Nutritional similarity
            if (product1.nutritionalInfo != null && product2.nutritionalInfo != null)
            {
                if (product1.nutritionalInfo.nutriScore == product2.nutritionalInfo.nutriScore)
                {
                    score += 1f;
                }
                criteria++;
            }

            // Ecological similarity
            if (product1.ecologicalInfo != null && product2.ecologicalInfo != null)
            {
                if (product1.ecologicalInfo.ecoScore == product2.ecologicalInfo.ecoScore)
                {
                    score += 1f;
                }
                if (product1.ecologicalInfo.organic == product2.ecologicalInfo.organic)
                {
                    score += 0.5f;
                }
                criteria += 2;
            }

            return criteria > 0 ? score / criteria : 0f;
        }

        /// <summary>
        /// Get trending products based on scan frequency
        /// </summary>
        public List<ProductData> GetTrendingProducts(int count = 5)
        {
            List<string> favoriteIds = userPreferences.GetFavoriteProducts(count);
            List<ProductData> trending = new List<ProductData>();

            foreach (string id in favoriteIds)
            {
                ProductData product = ProductDatabase.Instance.GetProductById(id);
                if (product != null)
                {
                    trending.Add(product);
                }
            }

            return trending;
        }

        /// <summary>
        /// Get products matching specific criteria
        /// </summary>
        public List<ProductData> GetProductsByCriteria(string category = null, float? maxPrice = null, 
            string nutriScore = null, bool? organic = null, int count = 10)
        {
            List<ProductData> allProducts = ProductDatabase.Instance.GetAllProducts();
            List<ProductData> filtered = new List<ProductData>();

            foreach (var product in allProducts)
            {
                // Apply filters
                if (!string.IsNullOrEmpty(category) && product.category != category)
                    continue;

                if (maxPrice.HasValue && product.price > maxPrice.Value)
                    continue;

                if (!string.IsNullOrEmpty(nutriScore) && 
                    product.nutritionalInfo?.nutriScore != nutriScore)
                    continue;

                if (organic.HasValue && 
                    product.ecologicalInfo?.organic != organic.Value)
                    continue;

                if (!userPreferences.MatchesDietaryPreferences(product))
                    continue;

                filtered.Add(product);
            }

            // Score and sort
            foreach (var product in filtered)
            {
                product.relevanceScore = scoringSystem.CalculateOverallScore(product, userPreferences);
            }

            return filtered
                .OrderByDescending(p => p.relevanceScore)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Update recommendation settings
        /// </summary>
        public void UpdateSettings(int maxRecs, float simThreshold, bool enableML)
        {
            maxRecommendations = maxRecs;
            similarityThreshold = simThreshold;
            enableMLRecommendations = enableML;
        }

        /// <summary>
        /// Refresh user preferences
        /// </summary>
        public void RefreshUserPreferences()
        {
            userPreferences = UserPreferencesManager.Instance.GetPreferences();
        }
    }
}
