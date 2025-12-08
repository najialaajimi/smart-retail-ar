using UnityEngine;
using SmartRetailAR.Data;
using System.Collections.Generic;
using System.Linq;

namespace SmartRetailAR.Recommendations
{
    public class AIRecommendations : MonoBehaviour
    {
        [Header("AI Settings")]
        public bool useDeepLearning = true;
        public float learningRate = 0.01f;
        public int trainingEpochs = 100;

        [Header("Collaborative Filtering")]
        public int numNeighbors = 10;
        public float minSimilarity = 0.5f;

        private Dictionary<string, Dictionary<string, float>> _userProductMatrix;
        private Dictionary<string, List<string>> _productRelations;
        private bool _isModelTrained = false;

        void Start()
        {
            InitializeAI();
        }

        private void InitializeAI()
        {
            _userProductMatrix = new Dictionary<string, Dictionary<string, float>>();
            _productRelations = new Dictionary<string, List<string>>();
            Debug.Log("AI Recommendations initialized");
        }

        public List<ProductData> GetAIRecommendations(string userId, ProductData currentProduct = null)
        {
            List<ProductData> recommendations = new List<ProductData>();

            if (useDeepLearning)
            {
                recommendations = GetDeepLearningRecommendations(userId, currentProduct);
            }
            else
            {
                recommendations = GetCollaborativeFilteringRecommendations(userId);
            }

            return recommendations;
        }

        private List<ProductData> GetDeepLearningRecommendations(string userId, ProductData currentProduct)
        {
            // Placeholder for deep learning model
            // In production: Use trained neural network model
            
            List<ProductData> allProducts = ProductDatabase.Instance.GetAllProducts();
            List<ProductData> recommendations = new List<ProductData>();

            // Simulate DL scoring
            Dictionary<ProductData, float> scores = new Dictionary<ProductData, float>();
            foreach (var product in allProducts)
            {
                if (currentProduct != null && product.id == currentProduct.id)
                    continue;

                float score = SimulateNeuralNetworkScore(userId, product, currentProduct);
                scores[product] = score;
            }

            recommendations = scores
                .OrderByDescending(kvp => kvp.Value)
                .Take(10)
                .Select(kvp => kvp.Key)
                .ToList();

            return recommendations;
        }

        private float SimulateNeuralNetworkScore(string userId, ProductData product, ProductData context)
        {
            // Simulate neural network output
            float baseScore = Random.Range(0.5f, 1.0f);

            // Contextual boost
            if (context != null && context.category == product.category)
                baseScore += 0.2f;

            if (product.flags.isBio)
                baseScore += 0.1f;

            if (GetScoreValue(product.scores.ecoScore) >= 4)
                baseScore += 0.15f;

            return Mathf.Clamp01(baseScore);
        }

        private List<ProductData> GetCollaborativeFilteringRecommendations(string userId)
        {
            // User-based collaborative filtering
            List<string> similarUsers = FindSimilarUsers(userId);
            Dictionary<string, float> productScores = new Dictionary<string, float>();

            foreach (var similarUser in similarUsers)
            {
                if (_userProductMatrix.ContainsKey(similarUser))
                {
                    foreach (var productRating in _userProductMatrix[similarUser])
                    {
                        if (!productScores.ContainsKey(productRating.Key))
                            productScores[productRating.Key] = 0f;
                        
                        productScores[productRating.Key] += productRating.Value;
                    }
                }
            }

            return productScores
                .OrderByDescending(kvp => kvp.Value)
                .Take(10)
                .Select(kvp => ProductDatabase.Instance.GetProductById(kvp.Key))
                .Where(p => p != null)
                .ToList();
        }

        private List<string> FindSimilarUsers(string userId)
        {
            // Placeholder for user similarity calculation
            List<string> similarUsers = new List<string>();
            
            // In production: Calculate cosine similarity between user vectors
            for (int i = 0; i < numNeighbors; i++)
            {
                similarUsers.Add($"user_{i}");
            }

            return similarUsers;
        }

        public void TrainModel(Dictionary<string, Dictionary<string, float>> userProductRatings)
        {
            _userProductMatrix = userProductRatings;
            
            // Simulate training
            Debug.Log($"Training AI model with {userProductRatings.Count} users...");
            
            // In production: Train neural network or matrix factorization model
            _isModelTrained = true;
            
            Debug.Log("AI model training completed");
        }

        public void RecordUserInteraction(string userId, string productId, float rating)
        {
            if (!_userProductMatrix.ContainsKey(userId))
            {
                _userProductMatrix[userId] = new Dictionary<string, float>();
            }

            _userProductMatrix[userId][productId] = rating;
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

        public bool IsModelTrained()
        {
            return _isModelTrained;
        }
    }
}
