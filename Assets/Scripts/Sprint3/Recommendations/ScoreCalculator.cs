using UnityEngine;
using SmartRetailAR.Data;

namespace SmartRetailAR.Recommendations
{
    public class ScoreCalculator : MonoBehaviour
    {
        [Header("Score Weights")]
        [Range(0f, 1f)] public float ecoWeight = 0.35f;
        [Range(0f, 1f)] public float healthWeight = 0.35f;
        [Range(0f, 1f)] public float socialWeight = 0.15f;
        [Range(0f, 1f)] public float priceWeight = 0.15f;

        public float CalculateOverallScore(ProductData product)
        {
            if (product == null) return 0f;

            float ecoScore = GetScoreValue(product.scores.ecoScore) / 5f;
            float healthScore = GetScoreValue(product.scores.healthScore) / 5f;
            float socialScore = product.scores.socialScore / 10f;
            float priceScore = CalculatePriceScore(product.price);

            float overall = (ecoScore * ecoWeight) +
                          (healthScore * healthWeight) +
                          (socialScore * socialWeight) +
                          (priceScore * priceWeight);

            return overall * 100f; // Convert to 0-100 scale
        }

        public float CalculateEcoScore(ProductData product)
        {
            if (product == null) return 0f;

            float score = GetScoreValue(product.scores.ecoScore) * 20f;

            // Bonuses
            if (product.flags.isBio) score += 10f;
            if (product.origin == "France") score += 5f;
            if (product.certifications != null && product.certifications.Contains("Écocert"))
                score += 5f;

            return Mathf.Clamp(score, 0f, 100f);
        }

        public float CalculateHealthScore(ProductData product)
        {
            if (product == null || product.nutritionalInfo == null) return 0f;

            float score = GetScoreValue(product.scores.healthScore) * 20f;

            // Nutritional bonuses/penalties
            NutritionalInfo info = product.nutritionalInfo;
            
            if (info.proteins > 10f) score += 10f;
            if (info.fibers > 5f) score += 10f;
            if (info.sugars < 5f) score += 10f;
            if (info.salt < 1f) score += 10f;
            
            if (info.calories > 500) score -= 10f;
            if (info.fats > 20f) score -= 10f;

            // Flag bonuses
            if (product.flags.isVegan) score += 5f;
            if (product.flags.isGlutenFree) score += 5f;

            return Mathf.Clamp(score, 0f, 100f);
        }

        public float CalculateSocialScore(ProductData product)
        {
            if (product == null) return 0f;

            float score = product.scores.socialScore * 10f;

            // Certification bonuses
            if (product.certifications != null)
            {
                if (product.certifications.Contains("Commerce Équitable")) score += 15f;
                if (product.certifications.Contains("Max Havelaar")) score += 10f;
            }

            return Mathf.Clamp(score, 0f, 100f);
        }

        private float CalculatePriceScore(float price)
        {
            // Lower prices get higher scores
            // Normalized to 0-1 range assuming max reasonable price of 50€
            return Mathf.Clamp01(1f - (price / 50f));
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

        public string GetScoreGrade(float score)
        {
            if (score >= 90f) return "A";
            if (score >= 75f) return "B";
            if (score >= 60f) return "C";
            if (score >= 45f) return "D";
            return "E";
        }

        public Color GetScoreColor(string grade)
        {
            switch (grade)
            {
                case "A": return new Color(0.2f, 0.8f, 0.2f);
                case "B": return new Color(0.5f, 0.8f, 0.2f);
                case "C": return new Color(0.9f, 0.8f, 0.2f);
                case "D": return new Color(0.9f, 0.5f, 0.2f);
                case "E": return new Color(0.9f, 0.2f, 0.2f);
                default: return Color.gray;
            }
        }

        public string GetScoreDescription(float score)
        {
            if (score >= 90f) return "Excellent";
            if (score >= 75f) return "Très bien";
            if (score >= 60f) return "Bien";
            if (score >= 45f) return "Moyen";
            return "À améliorer";
        }
    }
}
