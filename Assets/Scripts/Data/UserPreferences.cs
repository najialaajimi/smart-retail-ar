using UnityEngine;
using System.Collections.Generic;
using System;

namespace SmartRetailAR.Data
{
    /// <summary>
    /// User preferences and profile data management
    /// Handles dietary preferences, allergen filters, and purchase history
    /// </summary>
    [Serializable]
    public class UserPreferences
    {
        // Dietary preferences
        public bool vegetarian = false;
        public bool vegan = false;
        public bool glutenFree = false;
        public bool lactoseFree = false;
        public bool organic = false;
        
        // Allergen filters
        public List<string> allergenFilters = new List<string>();
        
        // Purchase history (using parallel lists for serialization)
        public List<string> scanHistoryIds = new List<string>();
        public List<long> scanHistoryTimestamps = new List<long>();
        
        // Product scan count (parallel lists for dictionary serialization)
        public List<string> scanCountKeys = new List<string>();
        public List<int> scanCountValues = new List<int>();
        
        // Private runtime dictionary
        [NonSerialized]
        private Dictionary<string, int> _productScanCount;
        [NonSerialized]
        private bool _isDictionaryInitialized = false;

        // Scoring preferences (weights for recommendation algorithm)
        public float nutritionalWeight = 0.3f;
        public float ecologicalWeight = 0.3f;
        public float economicalWeight = 0.2f;
        public float ethicalWeight = 0.2f;

        // Budget preference
        public float maxBudget = 100f;

        /// <summary>
        /// Initialize the dictionary from parallel lists
        /// </summary>
        public void InitializeDictionary()
        {
            if (_isDictionaryInitialized) return;

            _productScanCount = new Dictionary<string, int>();
            for (int i = 0; i < scanCountKeys.Count && i < scanCountValues.Count; i++)
            {
                _productScanCount[scanCountKeys[i]] = scanCountValues[i];
            }
            _isDictionaryInitialized = true;
        }

        /// <summary>
        /// Get scan count for a product
        /// </summary>
        public int GetProductScanCount(string productId)
        {
            InitializeDictionary();
            return _productScanCount.ContainsKey(productId) ? _productScanCount[productId] : 0;
        }

        /// <summary>
        /// Increment scan count for a product
        /// </summary>
        public void IncrementScanCount(string productId)
        {
            InitializeDictionary();
            
            if (_productScanCount.ContainsKey(productId))
            {
                _productScanCount[productId]++;
            }
            else
            {
                _productScanCount[productId] = 1;
            }

            // Sync to parallel lists for serialization
            SyncDictionaryToLists();
        }

        /// <summary>
        /// Add product to scan history
        /// </summary>
        public void AddToScanHistory(string productId)
        {
            scanHistoryIds.Add(productId);
            scanHistoryTimestamps.Add(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            IncrementScanCount(productId);
        }

        /// <summary>
        /// Sync dictionary to parallel lists for serialization
        /// </summary>
        private void SyncDictionaryToLists()
        {
            scanCountKeys.Clear();
            scanCountValues.Clear();

            if (_productScanCount != null)
            {
                foreach (var kvp in _productScanCount)
                {
                    scanCountKeys.Add(kvp.Key);
                    scanCountValues.Add(kvp.Value);
                }
            }
        }

        /// <summary>
        /// Check if product matches user dietary preferences
        /// </summary>
        public bool MatchesDietaryPreferences(ProductData product)
        {
            if (product == null) return false;

            // Check allergens
            if (allergenFilters.Count > 0 && product.allergens != null)
            {
                foreach (string allergen in allergenFilters)
                {
                    if (product.allergens.Contains(allergen))
                        return false;
                }
            }

            // Check organic preference
            if (organic && !product.ecologicalInfo.organic)
                return false;

            // Additional dietary checks can be added here
            // (vegetarian, vegan, gluten-free, etc. would require additional product data)

            return true;
        }

        /// <summary>
        /// Get user's favorite products (most scanned)
        /// </summary>
        public List<string> GetFavoriteProducts(int count = 5)
        {
            InitializeDictionary();
            
            List<string> favorites = new List<string>();
            var sortedProducts = new List<KeyValuePair<string, int>>(_productScanCount);
            sortedProducts.Sort((a, b) => b.Value.CompareTo(a.Value));

            for (int i = 0; i < Mathf.Min(count, sortedProducts.Count); i++)
            {
                favorites.Add(sortedProducts[i].Key);
            }

            return favorites;
        }

        /// <summary>
        /// Clear all user data
        /// </summary>
        public void ClearData()
        {
            vegetarian = false;
            vegan = false;
            glutenFree = false;
            lactoseFree = false;
            organic = false;
            allergenFilters.Clear();
            scanHistoryIds.Clear();
            scanHistoryTimestamps.Clear();
            scanCountKeys.Clear();
            scanCountValues.Clear();
            _productScanCount?.Clear();
            _isDictionaryInitialized = false;
        }
    }

    /// <summary>
    /// Manager for user preferences - handles loading and saving
    /// </summary>
    public class UserPreferencesManager : MonoBehaviour
    {
        private static UserPreferencesManager _instance;
        public static UserPreferencesManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("UserPreferencesManager");
                    _instance = go.AddComponent<UserPreferencesManager>();
                    DontDestroyOnLoad(go);
                    _instance.LoadPreferences();
                }
                return _instance;
            }
        }

        private UserPreferences preferences;
        private const string PREFS_KEY = "UserPreferences";

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                LoadPreferences();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Load user preferences from PlayerPrefs
        /// </summary>
        public void LoadPreferences()
        {
            string json = PlayerPrefs.GetString(PREFS_KEY, "");
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    preferences = JsonUtility.FromJson<UserPreferences>(json);
                    preferences.InitializeDictionary();
                    Debug.Log("User preferences loaded successfully");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error loading user preferences: {e.Message}");
                    preferences = new UserPreferences();
                }
            }
            else
            {
                preferences = new UserPreferences();
                Debug.Log("No saved preferences found, using defaults");
            }
        }

        /// <summary>
        /// Save user preferences to PlayerPrefs
        /// </summary>
        public void SavePreferences()
        {
            try
            {
                string json = JsonUtility.ToJson(preferences);
                PlayerPrefs.SetString(PREFS_KEY, json);
                PlayerPrefs.Save();
                Debug.Log("User preferences saved successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error saving user preferences: {e.Message}");
            }
        }

        /// <summary>
        /// Get current user preferences
        /// </summary>
        public UserPreferences GetPreferences()
        {
            if (preferences == null)
                LoadPreferences();
            return preferences;
        }

        /// <summary>
        /// Update user preferences
        /// </summary>
        public void UpdatePreferences(UserPreferences newPreferences)
        {
            preferences = newPreferences;
            SavePreferences();
        }

        private void OnApplicationQuit()
        {
            SavePreferences();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SavePreferences();
            }
        }
    }
}
