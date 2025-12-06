using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserPreferences
{
    public string userId;
    public string userName;
    public string avatarUrl;
    public List<string> dietaryPreferences; // Végétarien, Sans gluten, etc.
    public List<string> scannedProductIds;
    public Dictionary<string, int> productScanCount;
    public AppSettings settings;

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
        productScanCount = new Dictionary<string, int>();
        settings = new AppSettings();
    }

    public void AddScannedProduct(string productId)
    {
        if (!scannedProductIds.Contains(productId))
        {
            scannedProductIds.Add(productId);
        }

        if (productScanCount.ContainsKey(productId))
        {
            productScanCount[productId]++;
        }
        else
        {
            productScanCount[productId] = 1;
        }
    }

    public int GetScanCount(string productId)
    {
        return productScanCount.ContainsKey(productId) ? productScanCount[productId] : 0;
    }

    public void Save()
    {
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
