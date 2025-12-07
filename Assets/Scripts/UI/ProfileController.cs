using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using SmartRetailAR.Utils;
using SmartRetailAR.Data;

namespace SmartRetailAR.UI
{
    /// <summary>
    /// Controller for user profile screen
    /// Manages user preferences, history, and settings
    /// </summary>
    public class ProfileController : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI userNameText;
        public TMP_InputField userNameInput;
        public Button saveButton;
        public Button backButton;
        
        [Header("Dietary Preferences")]
        public Toggle vegetarianToggle;
        public Toggle veganToggle;
        public Toggle glutenFreeToggle;
        public Toggle lactoseFreeToggle;
        
        [Header("Settings")]
        public Toggle notificationsToggle;
        public Toggle ecoModeToggle;
        
        [Header("History")]
        public Transform historyContainer;
        public GameObject historyItemPrefab;
        public TextMeshProUGUI totalScansText;
        
        private UserPreferences _preferences;
        private List<GameObject> _historyItems = new List<GameObject>();
        
        private void Start()
        {
            // Setup button listeners
            if (saveButton != null)
            {
                saveButton.onClick.AddListener(OnSaveButtonClicked);
            }
            
            if (backButton != null)
            {
                backButton.onClick.AddListener(OnBackButtonClicked);
            }
            
            // Load user preferences
            LoadPreferences();
        }
        
        /// <summary>
        /// Load user preferences
        /// </summary>
        private void LoadPreferences()
        {
            _preferences = UserPreferences.Load();
            
            // Display user info
            if (userNameText != null)
            {
                userNameText.text = _preferences.userName;
            }
            
            if (userNameInput != null)
            {
                userNameInput.text = _preferences.userName;
            }
            
            // Load dietary preferences
            LoadDietaryPreferences();
            
            // Load settings
            LoadSettings();
            
            // Load history
            LoadHistory();
            
            // Display statistics
            DisplayStatistics();
        }
        
        /// <summary>
        /// Load dietary preferences
        /// </summary>
        private void LoadDietaryPreferences()
        {
            if (vegetarianToggle != null)
            {
                vegetarianToggle.isOn = _preferences.HasDietaryPreference("Vegetarian");
            }
            
            if (veganToggle != null)
            {
                veganToggle.isOn = _preferences.HasDietaryPreference("Vegan");
            }
            
            if (glutenFreeToggle != null)
            {
                glutenFreeToggle.isOn = _preferences.HasDietaryPreference("GlutenFree");
            }
            
            if (lactoseFreeToggle != null)
            {
                lactoseFreeToggle.isOn = _preferences.HasDietaryPreference("LactoseFree");
            }
        }
        
        /// <summary>
        /// Load settings
        /// </summary>
        private void LoadSettings()
        {
            if (notificationsToggle != null)
            {
                notificationsToggle.isOn = _preferences.notificationsEnabled;
            }
            
            if (ecoModeToggle != null)
            {
                ecoModeToggle.isOn = _preferences.ecoModeEnabled;
            }
        }
        
        /// <summary>
        /// Load scan history
        /// </summary>
        private void LoadHistory()
        {
            // Clear existing items
            ClearHistory();
            
            if (_preferences.scanHistoryIds == null || _preferences.scanHistoryIds.Count == 0)
            {
                Debug.Log("No scan history available");
                return;
            }
            
            // Display recent scans (limit to 10)
            int count = Mathf.Min(_preferences.scanHistoryIds.Count, 10);
            
            for (int i = 0; i < count; i++)
            {
                string productId = _preferences.scanHistoryIds[i];
                string timestamp = i < _preferences.scanHistoryTimestamps.Count 
                    ? _preferences.scanHistoryTimestamps[i] 
                    : "";
                
                CreateHistoryItem(productId, timestamp);
            }
        }
        
        /// <summary>
        /// Create history item
        /// </summary>
        private void CreateHistoryItem(string productId, string timestamp)
        {
            if (historyContainer == null) return;
            
            var product = ProductDatabase.Instance.GetProduct(productId);
            if (product == null) return;
            
            GameObject item;
            
            if (historyItemPrefab != null)
            {
                item = Instantiate(historyItemPrefab, historyContainer);
            }
            else
            {
                item = CreateSimpleHistoryItem();
            }
            
            // Set item data
            var nameText = item.GetComponentInChildren<Text>();
            if (nameText != null)
            {
                nameText.text = $"{product.name} - {product.brand}";
            }
            
            _historyItems.Add(item);
        }
        
        /// <summary>
        /// Create simple history item
        /// </summary>
        private GameObject CreateSimpleHistoryItem()
        {
            GameObject item = new GameObject("HistoryItem");
            item.transform.SetParent(historyContainer);
            
            var rectTransform = item.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(400, 60);
            
            var text = item.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 14;
            text.color = Color.black;
            
            return item;
        }
        
        /// <summary>
        /// Display statistics
        /// </summary>
        private void DisplayStatistics()
        {
            if (totalScansText != null)
            {
                int totalScans = _preferences.scanHistoryIds != null 
                    ? _preferences.scanHistoryIds.Count 
                    : 0;
                
                totalScansText.text = $"Total des scans: {totalScans}";
            }
        }
        
        /// <summary>
        /// Clear history items
        /// </summary>
        private void ClearHistory()
        {
            foreach (var item in _historyItems)
            {
                if (item != null)
                {
                    Destroy(item);
                }
            }
            
            _historyItems.Clear();
        }
        
        /// <summary>
        /// Save preferences
        /// </summary>
        private void SavePreferences()
        {
            // Update user name
            if (userNameInput != null && !string.IsNullOrEmpty(userNameInput.text))
            {
                _preferences.userName = userNameInput.text;
            }
            
            // Update dietary preferences
            _preferences.dietaryPreferences.Clear();
            
            if (vegetarianToggle != null && vegetarianToggle.isOn)
            {
                _preferences.dietaryPreferences.Add("Vegetarian");
            }
            
            if (veganToggle != null && veganToggle.isOn)
            {
                _preferences.dietaryPreferences.Add("Vegan");
            }
            
            if (glutenFreeToggle != null && glutenFreeToggle.isOn)
            {
                _preferences.dietaryPreferences.Add("GlutenFree");
            }
            
            if (lactoseFreeToggle != null && lactoseFreeToggle.isOn)
            {
                _preferences.dietaryPreferences.Add("LactoseFree");
            }
            
            // Update settings
            if (notificationsToggle != null)
            {
                _preferences.notificationsEnabled = notificationsToggle.isOn;
            }
            
            if (ecoModeToggle != null)
            {
                _preferences.ecoModeEnabled = ecoModeToggle.isOn;
            }
            
            // Save to PlayerPrefs
            _preferences.Save();
            
            Debug.Log("Preferences saved");
        }
        
        /// <summary>
        /// Handle save button click
        /// </summary>
        private void OnSaveButtonClicked()
        {
            SavePreferences();
        }
        
        /// <summary>
        /// Handle back button click
        /// </summary>
        private void OnBackButtonClicked()
        {
            SavePreferences();
            NavigationManager.Instance.NavigateBack();
        }
        
        private void OnDestroy()
        {
            // Cleanup
            ClearHistory();
            
            if (saveButton != null)
            {
                saveButton.onClick.RemoveListener(OnSaveButtonClicked);
            }
            
            if (backButton != null)
            {
                backButton.onClick.RemoveListener(OnBackButtonClicked);
            }
        }
    }
}
