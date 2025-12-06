using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class UserPreferences
{
    public string userId;
    public string userName;
    public string avatarUrl;
    public List<string> dietaryPreferences; // Végétarien, Sans gluten, etc.
    public List<string> scannedProductIds;
    
    // Serializable scan count using parallel lists (JsonUtility doesn't support Dictionary)
    public List<string> scanCountKeys;
    public List<int> scanCountValues;
    
    public AppSettings settings;
    
    // Runtime dictionary for easier access (not serialized)
    [NonSerialized]
    private Dictionary<string, int> _productScanCount;

    [Serializable]
    public class AppSettings
    {
        public bool soundEnabled = true;
        public bool hapticFeedbackEnabled = true;
        public bool showTutorial = true;
        public string language = "fr";
        public bool darkModeEnabled = false;
    }

    public UserPreferences()
    {
        userId = System.Guid.NewGuid().ToString();
        userName = "Utilisateur";
        dietaryPreferences = new List<string>();
        scannedProductIds = new List<string>();
        scanCountKeys = new List<string>();
        scanCountValues = new List<int>();
        _productScanCount = new Dictionary<string, int>();
        settings = new AppSettings();
    }
    
    // Get runtime dictionary, rebuilding from lists if needed
    private Dictionary<string, int> GetProductScanCount()
    {
        if (_productScanCount == null || _productScanCount.Count == 0)
        {
            _productScanCount = new Dictionary<string, int>();
            if (scanCountKeys != null && scanCountValues != null)
            {
                for (int i = 0; i < Mathf.Min(scanCountKeys.Count, scanCountValues.Count); i++)
                {
                    _productScanCount[scanCountKeys[i]] = scanCountValues[i];
                }
            }
        }
        return _productScanCount;
    }
    
    // Sync dictionary to lists before serialization
    private void SyncDictionaryToLists()
    {
        var dict = GetProductScanCount();
        scanCountKeys = new List<string>(dict.Keys);
        scanCountValues = new List<int>(dict.Values);
    }

    public void AddScannedProduct(string productId)
    {
        if (!scannedProductIds.Contains(productId))
        {
            scannedProductIds.Add(productId);
        }

        var dict = GetProductScanCount();
        if (dict.ContainsKey(productId))
        {
            dict[productId]++;
        }
        else
        {
            dict[productId] = 1;
        }
    }

    public int GetScanCount(string productId)
    {
        var dict = GetProductScanCount();
        return dict.ContainsKey(productId) ? dict[productId] : 0;
    }

    public void Save()
    {
        // Sync dictionary to lists before saving
        SyncDictionaryToLists();
        
        string json = JsonUtility.ToJson(this, true);
        PlayerPrefs.SetString("UserPreferences", json);
        PlayerPrefs.Save();
    }

    public static UserPreferences Load()
    {
        string json = PlayerPrefs.GetString("UserPreferences", "");
        if (!string.IsNullOrEmpty(json))
        {
            return JsonUtility.FromJson<UserPreferences>(json);
        }
        return new UserPreferences();
    }
}
