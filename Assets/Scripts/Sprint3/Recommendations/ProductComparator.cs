using UnityEngine;
using SmartRetailAR.Data;
using System.Collections.Generic;
using System.Linq;

namespace SmartRetailAR.Recommendations
{
    public class ProductComparator : MonoBehaviour
    {
        [System.Serializable]
        public class ComparisonResult
        {
            public ProductData product1;
            public ProductData product2;
            public Dictionary<string, ComparisonScore> scores;
            public string winner;
            public float overallScore;

            public ComparisonResult()
            {
                scores = new Dictionary<string, ComparisonScore>();
            }
        }

        [System.Serializable]
        public class ComparisonScore
        {
            public float product1Score;
            public float product2Score;
            public string betterProduct;
            public string reasoning;
        }

        public ComparisonResult CompareProducts(ProductData product1, ProductData product2)
        {
            if (product1 == null || product2 == null)
                return null;

            ComparisonResult result = new ComparisonResult
            {
                product1 = product1,
                product2 = product2
            };

            // Compare prices
            result.scores["price"] = ComparePrices(product1, product2);

            // Compare eco scores
            result.scores["eco"] = CompareScores(product1.scores.ecoScore, product2.scores.ecoScore, "Eco");

            // Compare health scores
            result.scores["health"] = CompareScores(product1.scores.healthScore, product2.scores.healthScore, "Health");

            // Compare nutri scores
            result.scores["nutri"] = CompareScores(product1.scores.nutriScore, product2.scores.nutriScore, "Nutri");

            // Compare nutritional values
            result.scores["nutrition"] = CompareNutrition(product1, product2);

            // Compare certifications
            result.scores["certifications"] = CompareCertifications(product1, product2);

            // Calculate overall winner
            CalculateOverallWinner(result);

            return result;
        }

        private ComparisonScore ComparePrices(ProductData p1, ProductData p2)
        {
            ComparisonScore score = new ComparisonScore
            {
                product1Score = p1.price,
                product2Score = p2.price
            };

            if (p1.price < p2.price)
            {
                score.betterProduct = "product1";
                score.reasoning = $"{p1.price:F2}€ moins cher que {p2.price:F2}€";
            }
            else if (p2.price < p1.price)
            {
                score.betterProduct = "product2";
                score.reasoning = $"{p2.price:F2}€ moins cher que {p1.price:F2}€";
            }
            else
            {
                score.betterProduct = "equal";
                score.reasoning = "Prix identiques";
            }

            return score;
        }

        private ComparisonScore CompareScores(string score1, string score2, string scoreType)
        {
            ComparisonScore comparison = new ComparisonScore
            {
                product1Score = GetScoreValue(score1),
                product2Score = GetScoreValue(score2)
            };

            if (comparison.product1Score > comparison.product2Score)
            {
                comparison.betterProduct = "product1";
                comparison.reasoning = $"{scoreType} Score: {score1} meilleur que {score2}";
            }
            else if (comparison.product2Score > comparison.product1Score)
            {
                comparison.betterProduct = "product2";
                comparison.reasoning = $"{scoreType} Score: {score2} meilleur que {score1}";
            }
            else
            {
                comparison.betterProduct = "equal";
                comparison.reasoning = $"{scoreType} Score identiques: {score1}";
            }

            return comparison;
        }

        private ComparisonScore CompareNutrition(ProductData p1, ProductData p2)
        {
            ComparisonScore score = new ComparisonScore();

            float p1Score = CalculateNutritionalScore(p1.nutritionalInfo);
            float p2Score = CalculateNutritionalScore(p2.nutritionalInfo);

            score.product1Score = p1Score;
            score.product2Score = p2Score;

            if (p1Score > p2Score)
            {
                score.betterProduct = "product1";
                score.reasoning = "Meilleurs valeurs nutritionnelles";
            }
            else if (p2Score > p1Score)
            {
                score.betterProduct = "product2";
                score.reasoning = "Meilleurs valeurs nutritionnelles";
            }
            else
            {
                score.betterProduct = "equal";
                score.reasoning = "Valeurs nutritionnelles similaires";
            }

            return score;
        }

        private float CalculateNutritionalScore(NutritionalInfo info)
        {
            if (info == null) return 0f;

            float score = 100f;
            
            // Penalize high calories
            if (info.calories > 500) score -= 10f;
            
            // Reward proteins
            score += Mathf.Min(info.proteins * 2f, 20f);
            
            // Penalize high fats
            if (info.fats > 20f) score -= 15f;
            
            // Penalize high sugars
            if (info.sugars > 20f) score -= 15f;
            
            // Reward fibers
            score += Mathf.Min(info.fibers * 3f, 15f);
            
            // Penalize high salt
            if (info.salt > 2f) score -= 10f;

            return Mathf.Clamp(score, 0f, 100f);
        }

        private ComparisonScore CompareCertifications(ProductData p1, ProductData p2)
        {
            ComparisonScore score = new ComparisonScore
            {
                product1Score = p1.certifications?.Count ?? 0,
                product2Score = p2.certifications?.Count ?? 0
            };

            if (score.product1Score > score.product2Score)
            {
                score.betterProduct = "product1";
                score.reasoning = $"{score.product1Score} certifications vs {score.product2Score}";
            }
            else if (score.product2Score > score.product1Score)
            {
                score.betterProduct = "product2";
                score.reasoning = $"{score.product2Score} certifications vs {score.product1Score}";
            }
            else
            {
                score.betterProduct = "equal";
                score.reasoning = "Même nombre de certifications";
            }

            return score;
        }

        private void CalculateOverallWinner(ComparisonResult result)
        {
            int p1Wins = 0;
            int p2Wins = 0;
            float p1TotalScore = 0f;
            float p2TotalScore = 0f;

            foreach (var kvp in result.scores)
            {
                if (kvp.Value.betterProduct == "product1")
                    p1Wins++;
                else if (kvp.Value.betterProduct == "product2")
                    p2Wins++;

                p1TotalScore += kvp.Value.product1Score;
                p2TotalScore += kvp.Value.product2Score;
            }

            if (p1Wins > p2Wins)
            {
                result.winner = "product1";
                result.overallScore = p1TotalScore / result.scores.Count;
            }
            else if (p2Wins > p1Wins)
            {
                result.winner = "product2";
                result.overallScore = p2TotalScore / result.scores.Count;
            }
            else
            {
                result.winner = "equal";
                result.overallScore = (p1TotalScore + p2TotalScore) / (2 * result.scores.Count);
            }
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

        public List<ComparisonResult> CompareMultipleProducts(List<ProductData> products)
        {
            List<ComparisonResult> results = new List<ComparisonResult>();

            for (int i = 0; i < products.Count; i++)
            {
                for (int j = i + 1; j < products.Count; j++)
                {
                    results.Add(CompareProducts(products[i], products[j]));
                }
            }

            return results;
        }

        public ProductData FindBestProduct(List<ProductData> products, string criteria = "overall")
        {
            if (products == null || products.Count == 0)
                return null;

            return products.OrderByDescending(p => ScoreProduct(p, criteria)).First();
        }

        private float ScoreProduct(ProductData product, string criteria)
        {
            switch (criteria.ToLower())
            {
                case "price":
                    return 1000f / (product.price + 1f); // Lower price = higher score
                case "eco":
                    return GetScoreValue(product.scores.ecoScore);
                case "health":
                    return GetScoreValue(product.scores.healthScore);
                case "nutrition":
                    return CalculateNutritionalScore(product.nutritionalInfo);
                default: // overall
                    return (GetScoreValue(product.scores.ecoScore) +
                           GetScoreValue(product.scores.healthScore) +
                           GetScoreValue(product.scores.nutriScore)) / 3f;
            }
        }
    }
}
