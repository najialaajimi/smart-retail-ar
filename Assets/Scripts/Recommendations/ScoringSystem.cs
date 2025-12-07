using UnityEngine;
using SmartRetailAR.Data;

namespace SmartRetailAR.Recommendations
{
    /// <summary>
    /// Multi-criteria scoring system for products
    /// Evaluates nutritional, ecological, economical, and ethical aspects
    /// </summary>
    public class ScoringSystem : MonoBehaviour
    {
        [Header("Score Weights (must sum to 1.0)")]
        [SerializeField] [Range(0f, 1f)] private float defaultNutritionalWeight = 0.3f;
        [SerializeField] [Range(0f, 1f)] private float defaultEcologicalWeight = 0.3f;
        [SerializeField] [Range(0f, 1f)] private float defaultEconomicalWeight = 0.2f;
        [SerializeField] [Range(0f, 1f)] private float defaultEthicalWeight = 0.2f;

        /// <summary>
        /// Calculate overall score for a product based on user preferences
        /// </summary>
        public float CalculateOverallScore(ProductData product, UserPreferences preferences = null)
        {
            if (product == null) return 0f;

            // Use user preferences weights if available, otherwise use defaults
            float nutritionalWeight = preferences?.nutritionalWeight ?? defaultNutritionalWeight;
            float ecologicalWeight = preferences?.ecologicalWeight ?? defaultEcologicalWeight;
            float economicalWeight = preferences?.economicalWeight ?? defaultEconomicalWeight;
            float ethicalWeight = preferences?.ethicalWeight ?? defaultEthicalWeight;

            // Calculate individual scores
            float nutritionalScore = CalculateNutritionalScore(product);
            float ecologicalScore = CalculateEcologicalScore(product);
            float economicalScore = CalculateEconomicalScore(product, preferences);
            float ethicalScore = CalculateEthicalScore(product);

            // Calculate weighted sum
            float overallScore = 
                (nutritionalScore * nutritionalWeight) +
                (ecologicalScore * ecologicalWeight) +
                (economicalScore * economicalWeight) +
                (ethicalScore * ethicalWeight);

            return Mathf.Clamp01(overallScore);
        }

        /// <summary>
        /// Calculate nutritional score (0-1)
        /// Based on Nutri-Score and nutritional values
        /// </summary>
        public float CalculateNutritionalScore(ProductData product)
        {
            if (product?.nutritionalInfo == null) return 0.5f;

            float score = 0f;

            // Nutri-Score mapping (60% of score)
            float nutriScoreValue = GetNutriScoreValue(product.nutritionalInfo.nutriScore);
            score += nutriScoreValue * 0.6f;

            // Protein content (10%)
            float proteinScore = Mathf.Clamp01(product.nutritionalInfo.protein / 20f);
            score += proteinScore * 0.1f;

            // Fiber content (10%)
            float fiberScore = Mathf.Clamp01(product.nutritionalInfo.fiber / 10f);
            score += fiberScore * 0.1f;

            // Low sugar (10%)
            float sugarScore = Mathf.Clamp01(1f - (product.nutritionalInfo.sugar / 30f));
            score += sugarScore * 0.1f;

            // Low sodium (10%)
            float sodiumScore = Mathf.Clamp01(1f - (product.nutritionalInfo.sodium / 1000f));
            score += sodiumScore * 0.1f;

            return Mathf.Clamp01(score);
        }

        /// <summary>
        /// Calculate ecological score (0-1)
        /// Based on carbon footprint, packaging, origin, and organic status
        /// </summary>
        public float CalculateEcologicalScore(ProductData product)
        {
            if (product?.ecologicalInfo == null) return 0.5f;

            float score = 0f;

            // Eco-Score mapping (40% of score)
            float ecoScoreValue = GetEcoScoreValue(product.ecologicalInfo.ecoScore);
            score += ecoScoreValue * 0.4f;

            // Carbon footprint (30%)
            // Lower is better, normalized to typical range (0-10 kg CO2)
            float carbonScore = Mathf.Clamp01(1f - (product.ecologicalInfo.carbonFootprint / 10f));
            score += carbonScore * 0.3f;

            // Organic (20%)
            if (product.ecologicalInfo.organic)
            {
                score += 0.2f;
            }

            // Packaging recyclability (10%)
            if (!string.IsNullOrEmpty(product.ecologicalInfo.packaging))
            {
                if (product.ecologicalInfo.packaging.ToLower().Contains("recyclable"))
                {
                    score += 0.1f;
                }
            }

            return Mathf.Clamp01(score);
        }

        /// <summary>
        /// Calculate economical score (0-1)
        /// Based on price and value for money
        /// </summary>
        public float CalculateEconomicalScore(ProductData product, UserPreferences preferences = null)
        {
            if (product == null) return 0.5f;

            float score = 0f;

            // Price affordability (60%)
            float maxBudget = preferences?.maxBudget ?? 100f;
            float priceScore = Mathf.Clamp01(1f - (product.price / maxBudget));
            score += priceScore * 0.6f;

            // Value for money (40%)
            // Higher quality (nutri + eco scores) at lower price = better value
            float qualityScore = (CalculateNutritionalScore(product) + CalculateEcologicalScore(product)) / 2f;
            float valueScore = qualityScore * (1f - Mathf.Clamp01(product.price / 50f));
            score += valueScore * 0.4f;

            return Mathf.Clamp01(score);
        }

        /// <summary>
        /// Calculate ethical score (0-1)
        /// Based on fair trade, local sourcing, and ethical practices
        /// </summary>
        public float CalculateEthicalScore(ProductData product)
        {
            if (product?.ethicalInfo == null) return 0.5f;

            float score = 0f;

            // Ethical Score mapping (50% of score)
            float ethicalScoreValue = GetEthicalScoreValue(product.ethicalInfo.ethicalScore);
            score += ethicalScoreValue * 0.5f;

            // Fair trade (30%)
            if (product.ethicalInfo.fairTrade)
            {
                score += 0.3f;
            }

            // Local sourcing (20%)
            if (product.ethicalInfo.local)
            {
                score += 0.2f;
            }

            return Mathf.Clamp01(score);
        }

        /// <summary>
        /// Convert Nutri-Score letter to numeric value
        /// </summary>
        private float GetNutriScoreValue(string nutriScore)
        {
            if (string.IsNullOrEmpty(nutriScore)) return 0.5f;

            switch (nutriScore.ToUpper())
            {
                case "A": return 1.0f;
                case "B": return 0.8f;
                case "C": return 0.6f;
                case "D": return 0.4f;
                case "E": return 0.2f;
                default: return 0.5f;
            }
        }

        /// <summary>
        /// Convert Eco-Score letter to numeric value
        /// </summary>
        private float GetEcoScoreValue(string ecoScore)
        {
            if (string.IsNullOrEmpty(ecoScore)) return 0.5f;

            switch (ecoScore.ToUpper())
            {
                case "A": return 1.0f;
                case "B": return 0.8f;
                case "C": return 0.6f;
                case "D": return 0.4f;
                case "E": return 0.2f;
                default: return 0.5f;
            }
        }

        /// <summary>
        /// Convert Ethical Score letter to numeric value
        /// </summary>
        private float GetEthicalScoreValue(string ethicalScore)
        {
            if (string.IsNullOrEmpty(ethicalScore)) return 0.5f;

            switch (ethicalScore.ToUpper())
            {
                case "A": return 1.0f;
                case "B": return 0.8f;
                case "C": return 0.6f;
                case "D": return 0.4f;
                case "E": return 0.2f;
                default: return 0.5f;
            }
        }

        /// <summary>
        /// Get detailed score breakdown
        /// </summary>
        public ScoreBreakdown GetScoreBreakdown(ProductData product, UserPreferences preferences = null)
        {
            return new ScoreBreakdown
            {
                nutritionalScore = CalculateNutritionalScore(product),
                ecologicalScore = CalculateEcologicalScore(product),
                economicalScore = CalculateEconomicalScore(product, preferences),
                ethicalScore = CalculateEthicalScore(product),
                overallScore = CalculateOverallScore(product, preferences)
            };
        }
    }

    /// <summary>
    /// Score breakdown structure
    /// </summary>
    [System.Serializable]
    public class ScoreBreakdown
    {
        public float nutritionalScore;
        public float ecologicalScore;
        public float economicalScore;
        public float ethicalScore;
        public float overallScore;

        public override string ToString()
        {
            return $"Overall: {overallScore:F2}\n" +
                   $"Nutritional: {nutritionalScore:F2}\n" +
                   $"Ecological: {ecologicalScore:F2}\n" +
                   $"Economical: {economicalScore:F2}\n" +
                   $"Ethical: {ethicalScore:F2}";
        }
    }
}
