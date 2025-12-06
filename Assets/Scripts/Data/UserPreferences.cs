using System;
using System.Collections.Generic;
using UnityEngine;

namespace SmartRetailAR.Data
{
    /// <summary>
    /// User preferences and profile data
    /// </summary>
    [Serializable]
    public class UserPreferences
    {
        public string userId;
        public string userName;
        public string avatarUrl;
        
        // Dietary preferences
        public List<string> dietaryPreferences; // vegetarian, vegan, gluten-free, etc.
        public List<string> allergens;
        
        // Scan history - Using parallel lists for Unity JsonUtility compatibility
        public List<string> scanHistoryIds;
        public List<string> scanHistoryTimestamps;
        
        // Product scan count - Using parallel lists for Unity JsonUtility compatibility
        public List<string> scanCountKeys;
        public List<int> scanCountValues;
        
        // Runtime dictionary for performance (not serialized)
        [NonSerialized]
        private Dictionary<string, int> _productScanCount;
        
        // Settings
        public bool notificationsEnabled;
        public bool ecoModeEnabled;
        public string preferredLanguage;
        
        public UserPreferences()
        {
            userId = System.Guid.NewGuid().ToString();
            userName = "User";
            dietaryPreferences = new List<string>();
            allergens = new List<string>();
            scanHistoryIds = new List<string>();
            scanHistoryTimestamps = new List<string>();
            scanCountKeys = new List<string>();
            scanCountValues = new List<int>();
            notificationsEnabled = true;
            ecoModeEnabled = true;
            preferredLanguage = "en";
        }
        
        /// <summary>
        /// Add product to scan history
        /// </summary>
        public void AddToHistory(string productId)
        {
            if (string.IsNullOrEmpty(productId)) return;
            
            // Remove if already exists to avoid duplicates
            int existingIndex = scanHistoryIds.IndexOf(productId);
            if (existingIndex >= 0)
            {
                scanHistoryIds.RemoveAt(existingIndex);
                scanHistoryTimestamps.RemoveAt(existingIndex);
            }
            
            // Add to beginning of list
            scanHistoryIds.Insert(0, productId);
            scanHistoryTimestamps.Insert(0, DateTime.Now.ToString("o"));
            
            // Keep only last 50 items
            if (scanHistoryIds.Count > 50)
            {
                scanHistoryIds.RemoveAt(scanHistoryIds.Count - 1);
                scanHistoryTimestamps.RemoveAt(scanHistoryTimestamps.Count - 1);
            }
            
            // Update scan count
            IncrementScanCount(productId);
        }
        
        /// <summary>
        /// Increment scan count for a product
        /// </summary>
        private void IncrementScanCount(string productId)
        {
            // Build runtime dictionary if not exists
            if (_productScanCount == null)
            {
                _productScanCount = new Dictionary<string, int>();
                for (int i = 0; i < scanCountKeys.Count; i++)
                {
                    _productScanCount[scanCountKeys[i]] = scanCountValues[i];
                }
            }
            
            if (_productScanCount.ContainsKey(productId))
            {
                _productScanCount[productId]++;
            }
            else
            {
                _productScanCount[productId] = 1;
            }
            
            // Sync back to lists for serialization
            SyncScanCountToLists();
        }
        
        /// <summary>
        /// Get scan count for a product
        /// </summary>
        public int GetScanCount(string productId)
        {
            // Build runtime dictionary if not exists
            if (_productScanCount == null)
            {
                _productScanCount = new Dictionary<string, int>();
                for (int i = 0; i < scanCountKeys.Count; i++)
                {
                    _productScanCount[scanCountKeys[i]] = scanCountValues[i];
                }
            }
            
            return _productScanCount.ContainsKey(productId) ? _productScanCount[productId] : 0;
        }
        
        /// <summary>
        /// Sync runtime dictionary to lists for serialization
        /// </summary>
        private void SyncScanCountToLists()
        {
            scanCountKeys.Clear();
            scanCountValues.Clear();
            
            foreach (var kvp in _productScanCount)
            {
                scanCountKeys.Add(kvp.Key);
                scanCountValues.Add(kvp.Value);
            }
        }
        
        /// <summary>
        /// Check if user has dietary preference
        /// </summary>
        public bool HasDietaryPreference(string preference)
        {
            return dietaryPreferences != null && dietaryPreferences.Contains(preference);
        }
        
        /// <summary>
        /// Check if user has allergen
        /// </summary>
        public bool HasAllergen(string allergen)
        {
            return allergens != null && allergens.Contains(allergen);
        }
        
        /// <summary>
        /// Save preferences to PlayerPrefs
        /// </summary>
        public void Save()
        {
            string json = JsonUtility.ToJson(this);
            PlayerPrefs.SetString("UserPreferences", json);
            PlayerPrefs.Save();
        }
        
        /// <summary>
        /// Load preferences from PlayerPrefs
        /// </summary>
        public static UserPreferences Load()
        {
            if (PlayerPrefs.HasKey("UserPreferences"))
            {
                string json = PlayerPrefs.GetString("UserPreferences");
                try
                {
                    return JsonUtility.FromJson<UserPreferences>(json);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error loading user preferences: {e.Message}");
                    return new UserPreferences();
                }
            }
            
            return new UserPreferences();
        }
    }
}
