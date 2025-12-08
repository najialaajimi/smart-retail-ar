using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using SmartRetailAR.Data;

namespace SmartRetailAR.Sprint3.Recommendations
{
    /// <summary>
    /// Recommendation engine for suggesting alternative products
    /// Sprint 3: Smart recommendations based on user preferences and product attributes
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
        
        [System.Serializable]
        public class RecommendationCriteria
        {
            public bool preferBio;
            public bool preferLocal;
            public bool preferVegan;
            public bool preferGlutenFree;
            public bool preferFairTrade;
            public float maxPrice;
            public float minEcoScore;
            public float minHealthScore;
            public string preferredCategory;
        }
        
        private RecommendationCriteria _criteria = new RecommendationCriteria();
        
        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            LoadDefaultCriteria();
        }
        
        void LoadDefaultCriteria()
        {
            _criteria = new RecommendationCriteria
            {
                preferBio = false,
                preferLocal = false,
                preferVegan = false,
                preferGlutenFree = false,
                preferFairTrade = false,
                maxPrice = float.MaxValue,
                minEcoScore = 0f,
                minHealthScore = 0f
            };
        }
        
        public void SetCriteria(RecommendationCriteria criteria)
        {
            _criteria = criteria;
            Debug.Log("Recommendation criteria updated");
        }
        
        public RecommendationCriteria GetCriteria()
        {
            return _criteria;
        }
        
        /// <summary>
        /// Get recommended alternatives for a product
        /// </summary>
        public List<ProductData> GetRecommendations(ProductData currentProduct, int maxResults = 10)
        {
            if (currentProduct == null)
                return new List<ProductData>();
            
            var allProducts = ProductDatabaseManager.Instance.GetAllProducts();
            var recommendations = new List<ProductData>();
            
            // First, add explicitly defined alternatives
            if (currentProduct.alternativeIds != null && currentProduct.alternativeIds.Count > 0)
            {
                foreach (string altId in currentProduct.alternativeIds)
                {
                    var altProduct = ProductDatabaseManager.Instance.GetProductById(altId);
                    if (altProduct != null)
                    {
                        recommendations.Add(altProduct);
                    }
                }
            }
            
            // Then, find similar products in the same category
            var similarProducts = allProducts
                .Where(p => p.id != currentProduct.id && 
                           p.category == currentProduct.category)
                .ToList();
            
            // Score and sort products
            var scoredProducts = new List<(ProductData product, float score)>();
            
            foreach (var product in similarProducts)
            {
                if (recommendations.Contains(product))
                    continue;
                
                float score = CalculateRecommendationScore(currentProduct, product);
                scoredProducts.Add((product, score));
            }
            
            // Sort by score and add to recommendations
            var topProducts = scoredProducts
                .OrderByDescending(p => p.score)
                .Take(maxResults - recommendations.Count)
                .Select(p => p.product);
            
            recommendations.AddRange(topProducts);
            
            return recommendations.Take(maxResults).ToList();
        }
        
        /// <summary>
        /// Calculate recommendation score based on criteria and product attributes
        /// </summary>
        float CalculateRecommendationScore(ProductData current, ProductData candidate)
        {
            float score = 0f;
            
            // Price similarity (prefer similar price range)
            float priceDiff = Mathf.Abs(current.price - candidate.price);
            float priceScore = 1f / (1f + priceDiff);
            score += priceScore * 10f;
            
            // Eco score improvement
            if (candidate.ecoScore > current.ecoScore)
            {
                score += (candidate.ecoScore - current.ecoScore) * 0.5f;
            }
            
            // Health score improvement
            if (candidate.healthScore > current.healthScore)
            {
                score += (candidate.healthScore - current.healthScore) * 0.5f;
            }
            
            // Apply user preferences
            if (_criteria.preferBio && candidate.isBio)
                score += 20f;
            
            if (_criteria.preferLocal && candidate.isLocal)
                score += 15f;
            
            if (_criteria.preferVegan && candidate.isVegan)
                score += 15f;
            
            if (_criteria.preferGlutenFree && candidate.isGlutenFree)
                score += 10f;
            
            if (_criteria.preferFairTrade && candidate.isFairTrade)
                score += 10f;
            
            // Filter by criteria
            if (_criteria.maxPrice < float.MaxValue && candidate.price > _criteria.maxPrice)
                score -= 50f;
            
            if (candidate.ecoScore < _criteria.minEcoScore)
                score -= 30f;
            
            if (candidate.healthScore < _criteria.minHealthScore)
                score -= 30f;
            
            return score;
        }
        
        /// <summary>
        /// Get products filtered by specific criteria
        /// </summary>
        public List<ProductData> GetFilteredProducts(RecommendationCriteria criteria = null)
        {
            if (criteria == null)
                criteria = _criteria;
            
            var allProducts = ProductDatabaseManager.Instance.GetAllProducts();
            var filtered = allProducts.AsEnumerable();
            
            // Apply filters
            if (criteria.preferBio)
                filtered = filtered.Where(p => p.isBio);
            
            if (criteria.preferLocal)
                filtered = filtered.Where(p => p.isLocal);
            
            if (criteria.preferVegan)
                filtered = filtered.Where(p => p.isVegan);
            
            if (criteria.preferGlutenFree)
                filtered = filtered.Where(p => p.isGlutenFree);
            
            if (criteria.preferFairTrade)
                filtered = filtered.Where(p => p.isFairTrade);
            
            if (criteria.maxPrice < float.MaxValue)
                filtered = filtered.Where(p => p.price <= criteria.maxPrice);
            
            if (criteria.minEcoScore > 0f)
                filtered = filtered.Where(p => p.ecoScore >= criteria.minEcoScore);
            
            if (criteria.minHealthScore > 0f)
                filtered = filtered.Where(p => p.healthScore >= criteria.minHealthScore);
            
            if (!string.IsNullOrEmpty(criteria.preferredCategory))
                filtered = filtered.Where(p => p.category == criteria.preferredCategory);
            
            return filtered.ToList();
        }
        
        /// <summary>
        /// Get best eco-friendly alternatives
        /// </summary>
        public List<ProductData> GetEcoFriendlyAlternatives(ProductData currentProduct, int count = 5)
        {
            var alternatives = GetRecommendations(currentProduct, 50);
            return alternatives
                .OrderByDescending(p => p.ecoScore)
                .Take(count)
                .ToList();
        }
        
        /// <summary>
        /// Get healthiest alternatives
        /// </summary>
        public List<ProductData> GetHealthyAlternatives(ProductData currentProduct, int count = 5)
        {
            var alternatives = GetRecommendations(currentProduct, 50);
            return alternatives
                .OrderByDescending(p => p.healthScore)
                .Take(count)
                .ToList();
        }
        
        /// <summary>
        /// Get budget-friendly alternatives
        /// </summary>
        public List<ProductData> GetBudgetAlternatives(ProductData currentProduct, int count = 5)
        {
            var alternatives = GetRecommendations(currentProduct, 50);
            return alternatives
                .Where(p => p.price < currentProduct.price)
                .OrderBy(p => p.price)
                .Take(count)
                .ToList();
        }
    }
}
