using UnityEngine;

namespace SmartRetailAR.Utils
{
    /// <summary>
    /// Global constants and configuration values
    /// </summary>
    public static class Constants
    {
        // App Info
        public const string APP_NAME = "Smart Retail AR";
        public const string APP_VERSION = "1.0.0";
        
        // Scene Names
        public const string SCENE_HOME = "HomeScene";
        public const string SCENE_SCANNER = "ScannerScene";
        public const string SCENE_PRODUCT_INFO = "ProductInfoScene";
        public const string SCENE_AR = "ARScene";
        public const string SCENE_RECOMMENDATIONS = "RecommendationsScene";
        public const string SCENE_PROFILE = "ProfileScene";
        public const string SCENE_SETTINGS = "SettingsScene";
        
        // Resource Paths
        public const string RESOURCE_PRODUCTS_DB = "Data/products_database";
        public const string RESOURCE_IMAGES = "Images/Products";
        public const string RESOURCE_QRCODES = "QRCodes";
        
        // Player Prefs Keys
        public const string PREF_FIRST_LAUNCH = "FirstLaunch";
        public const string PREF_USER_NAME = "UserName";
        public const string PREF_SCAN_COUNT = "ScanCount";
        public const string PREF_PREFER_BIO = "PreferBio";
        public const string PREF_PREFER_LOCAL = "PreferLocal";
        public const string PREF_PREFER_VEGAN = "PreferVegan";
        
        // KPI Targets
        public const float TARGET_RECOGNITION_RATE = 95f; // percentage
        public const float TARGET_LATENCY = 1.0f; // seconds
        public const int TARGET_SATISFACTION = 80; // percentage
        
        // Performance Thresholds
        public const float MIN_FPS = 30f;
        public const long MAX_MEMORY_MB = 500; // MB
        public const float LOW_BATTERY_THRESHOLD = 0.2f; // 20%
        
        // AR Settings
        public const float AR_SCAN_INTERVAL = 0.5f;
        public const float AR_RAYCAST_MAX_DISTANCE = 10f;
        public const int AR_MAX_TRACKED_IMAGES = 3;
        
        // UI Settings
        public const float UI_FADE_DURATION = 0.3f;
        public const float UI_SCROLL_SENSITIVITY = 10f;
        public const int UI_PRODUCTS_PER_PAGE = 20;
        
        // Recommendation Settings
        public const int MAX_RECOMMENDATIONS = 10;
        public const int MAX_ECO_ALTERNATIVES = 5;
        public const int MAX_HEALTHY_ALTERNATIVES = 5;
        public const int MAX_BUDGET_ALTERNATIVES = 5;
        
        // Score Ranges
        public const float SCORE_EXCELLENT = 90f;
        public const float SCORE_GOOD = 75f;
        public const float SCORE_AVERAGE = 50f;
        public const float SCORE_POOR = 25f;
        
        // Colors (Hex)
        public const string COLOR_PRIMARY = "#2196F3";
        public const string COLOR_SECONDARY = "#FF9800";
        public const string COLOR_SUCCESS = "#4CAF50";
        public const string COLOR_WARNING = "#FFC107";
        public const string COLOR_DANGER = "#F44336";
        public const string COLOR_INFO = "#00BCD4";
        
        // Tags
        public const string TAG_BIO = "Bio";
        public const string TAG_LOCAL = "Local";
        public const string TAG_VEGAN = "Vegan";
        public const string TAG_GLUTEN_FREE = "GlutenFree";
        public const string TAG_FAIR_TRADE = "FairTrade";
        
        // API (for future use)
        public const string API_BASE_URL = "https://api.smartretailar.com";
        public const string API_VERSION = "v1";
        public const int API_TIMEOUT = 10; // seconds
        
        // File Paths
        public static string GetPersistentPath(string filename)
        {
            return System.IO.Path.Combine(Application.persistentDataPath, filename);
        }
        
        public static string GetStreamingPath(string filename)
        {
            return System.IO.Path.Combine(Application.streamingAssetsPath, filename);
        }
        
        // Validation
        public static bool IsValidScore(float score)
        {
            return score >= 0f && score <= 100f;
        }
        
        public static bool IsValidPrice(float price)
        {
            return price >= 0f;
        }
    }
}
