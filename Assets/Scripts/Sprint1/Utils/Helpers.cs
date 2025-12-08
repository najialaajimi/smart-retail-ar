using UnityEngine;
using System.Collections;

namespace SmartRetailAR.Utils
{
    /// <summary>
    /// Utility class for common helper functions
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Format price with currency symbol
        /// </summary>
        public static string FormatPrice(float price, string currency = "€")
        {
            return $"{price:F2} {currency}";
        }
        
        /// <summary>
        /// Get color based on score value
        /// </summary>
        public static Color GetScoreColor(float score)
        {
            if (score >= 75f)
                return new Color(0.2f, 0.8f, 0.2f); // Green
            else if (score >= 50f)
                return new Color(1f, 0.8f, 0f); // Yellow
            else
                return new Color(0.9f, 0.2f, 0.2f); // Red
        }
        
        /// <summary>
        /// Get score letter grade
        /// </summary>
        public static string GetScoreGrade(float score)
        {
            if (score >= 90f) return "A";
            if (score >= 80f) return "B";
            if (score >= 70f) return "C";
            if (score >= 60f) return "D";
            return "F";
        }
        
        /// <summary>
        /// Format nutritional value with unit
        /// </summary>
        public static string FormatNutrition(float value, string unit)
        {
            return $"{value:F1}{unit}";
        }
        
        /// <summary>
        /// Check if device has AR support
        /// </summary>
        public static bool IsARSupported()
        {
            #if UNITY_ANDROID
                return UnityEngine.Android.Permission.HasUserAuthorizedPermission(
                    UnityEngine.Android.Permission.Camera);
            #elif UNITY_IOS
                return Application.platform == RuntimePlatform.IPhonePlayer;
            #else
                return false;
            #endif
        }
        
        /// <summary>
        /// Calculate percentage difference
        /// </summary>
        public static float CalculatePercentageDiff(float original, float current)
        {
            if (original == 0) return 0;
            return ((current - original) / original) * 100f;
        }
        
        /// <summary>
        /// Lerp color over time
        /// </summary>
        public static IEnumerator LerpColor(UnityEngine.UI.Graphic graphic, 
            Color targetColor, float duration)
        {
            Color startColor = graphic.color;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                graphic.color = Color.Lerp(startColor, targetColor, t);
                yield return null;
            }
            
            graphic.color = targetColor;
        }
        
        /// <summary>
        /// Format file size in human readable format
        /// </summary>
        public static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            
            return $"{len:F2} {sizes[order]}";
        }
        
        /// <summary>
        /// Check if string is valid product ID
        /// </summary>
        public static bool IsValidProductId(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            if (id.Length < 4) return false;
            return id.StartsWith("PROD");
        }
        
        /// <summary>
        /// Get contrast color for text (black or white)
        /// </summary>
        public static Color GetContrastColor(Color backgroundColor)
        {
            float luminance = 0.299f * backgroundColor.r + 
                             0.587f * backgroundColor.g + 
                             0.114f * backgroundColor.b;
            
            return luminance > 0.5f ? Color.black : Color.white;
        }
    }
}
