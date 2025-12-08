using UnityEngine;
using System.Collections.Generic;

namespace SmartRetailAR.Data
{
    /// <summary>
    /// Manages user preferences and settings
    /// </summary>
    public class UserPreferences : MonoBehaviour
    {
        private static UserPreferences _instance;
        public static UserPreferences Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("UserPreferences");
                    _instance = go.AddComponent<UserPreferences>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }
        
        // User settings
        private bool _preferBio = false;
        private bool _preferLocal = false;
        private bool _preferVegan = false;
        private bool _preferGlutenFree = false;
        private bool _preferFairTrade = false;
        private float _maxPrice = 100f;
        private float _minEcoScore = 0f;
        private float _minHealthScore = 0f;
        
        // User stats
        private int _totalScans = 0;
        private int _totalProductViews = 0;
        private List<string> _recentProducts = new List<string>();
        private List<string> _favoriteProducts = new List<string>();
        
        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            LoadPreferences();
        }
        
        #region Preferences Getters/Setters
        
        public bool PreferBio
        {
            get => _preferBio;
            set { _preferBio = value; SavePreferences(); }
        }
        
        public bool PreferLocal
        {
            get => _preferLocal;
            set { _preferLocal = value; SavePreferences(); }
        }
        
        public bool PreferVegan
        {
            get => _preferVegan;
            set { _preferVegan = value; SavePreferences(); }
        }
        
        public bool PreferGlutenFree
        {
            get => _preferGlutenFree;
            set { _preferGlutenFree = value; SavePreferences(); }
        }
        
        public bool PreferFairTrade
        {
            get => _preferFairTrade;
            set { _preferFairTrade = value; SavePreferences(); }
        }
        
        public float MaxPrice
        {
            get => _maxPrice;
            set { _maxPrice = value; SavePreferences(); }
        }
        
        public float MinEcoScore
        {
            get => _minEcoScore;
            set { _minEcoScore = value; SavePreferences(); }
        }
        
        public float MinHealthScore
        {
            get => _minHealthScore;
            set { _minHealthScore = value; SavePreferences(); }
        }
        
        #endregion
        
        #region Statistics
        
        public void IncrementScans()
        {
            _totalScans++;
            SavePreferences();
        }
        
        public void IncrementProductViews()
        {
            _totalProductViews++;
            SavePreferences();
        }
        
        public int GetTotalScans() => _totalScans;
        public int GetTotalProductViews() => _totalProductViews;
        
        public void AddRecentProduct(string productId)
        {
            if (_recentProducts.Contains(productId))
                _recentProducts.Remove(productId);
            
            _recentProducts.Insert(0, productId);
            
            // Keep only last 20
            if (_recentProducts.Count > 20)
                _recentProducts.RemoveAt(_recentProducts.Count - 1);
            
            SavePreferences();
        }
        
        public List<string> GetRecentProducts() => new List<string>(_recentProducts);
        
        public void AddFavorite(string productId)
        {
            if (!_favoriteProducts.Contains(productId))
            {
                _favoriteProducts.Add(productId);
                SavePreferences();
            }
        }
        
        public void RemoveFavorite(string productId)
        {
            if (_favoriteProducts.Contains(productId))
            {
                _favoriteProducts.Remove(productId);
                SavePreferences();
            }
        }
        
        public bool IsFavorite(string productId)
        {
            return _favoriteProducts.Contains(productId);
        }
        
        public List<string> GetFavorites() => new List<string>(_favoriteProducts);
        
        #endregion
        
        #region Persistence
        
        private void LoadPreferences()
        {
            _preferBio = PlayerPrefs.GetInt("PreferBio", 0) == 1;
            _preferLocal = PlayerPrefs.GetInt("PreferLocal", 0) == 1;
            _preferVegan = PlayerPrefs.GetInt("PreferVegan", 0) == 1;
            _preferGlutenFree = PlayerPrefs.GetInt("PreferGlutenFree", 0) == 1;
            _preferFairTrade = PlayerPrefs.GetInt("PreferFairTrade", 0) == 1;
            
            _maxPrice = PlayerPrefs.GetFloat("MaxPrice", 100f);
            _minEcoScore = PlayerPrefs.GetFloat("MinEcoScore", 0f);
            _minHealthScore = PlayerPrefs.GetFloat("MinHealthScore", 0f);
            
            _totalScans = PlayerPrefs.GetInt("TotalScans", 0);
            _totalProductViews = PlayerPrefs.GetInt("TotalProductViews", 0);
            
            // Load recent products
            string recentJson = PlayerPrefs.GetString("RecentProducts", "[]");
            try
            {
                _recentProducts = JsonUtility.FromJson<StringList>(recentJson).items;
            }
            catch
            {
                _recentProducts = new List<string>();
            }
            
            // Load favorites
            string favoritesJson = PlayerPrefs.GetString("Favorites", "[]");
            try
            {
                _favoriteProducts = JsonUtility.FromJson<StringList>(favoritesJson).items;
            }
            catch
            {
                _favoriteProducts = new List<string>();
            }
            
            Debug.Log("User preferences loaded");
        }
        
        private void SavePreferences()
        {
            PlayerPrefs.SetInt("PreferBio", _preferBio ? 1 : 0);
            PlayerPrefs.SetInt("PreferLocal", _preferLocal ? 1 : 0);
            PlayerPrefs.SetInt("PreferVegan", _preferVegan ? 1 : 0);
            PlayerPrefs.SetInt("PreferGlutenFree", _preferGlutenFree ? 1 : 0);
            PlayerPrefs.SetInt("PreferFairTrade", _preferFairTrade ? 1 : 0);
            
            PlayerPrefs.SetFloat("MaxPrice", _maxPrice);
            PlayerPrefs.SetFloat("MinEcoScore", _minEcoScore);
            PlayerPrefs.SetFloat("MinHealthScore", _minHealthScore);
            
            PlayerPrefs.SetInt("TotalScans", _totalScans);
            PlayerPrefs.SetInt("TotalProductViews", _totalProductViews);
            
            // Save recent products
            StringList recentList = new StringList { items = _recentProducts };
            PlayerPrefs.SetString("RecentProducts", JsonUtility.ToJson(recentList));
            
            // Save favorites
            StringList favoritesList = new StringList { items = _favoriteProducts };
            PlayerPrefs.SetString("Favorites", JsonUtility.ToJson(favoritesList));
            
            PlayerPrefs.Save();
        }
        
        public void ResetPreferences()
        {
            _preferBio = false;
            _preferLocal = false;
            _preferVegan = false;
            _preferGlutenFree = false;
            _preferFairTrade = false;
            _maxPrice = 100f;
            _minEcoScore = 0f;
            _minHealthScore = 0f;
            
            SavePreferences();
            Debug.Log("User preferences reset to defaults");
        }
        
        public void ClearStatistics()
        {
            _totalScans = 0;
            _totalProductViews = 0;
            _recentProducts.Clear();
            
            SavePreferences();
            Debug.Log("User statistics cleared");
        }
        
        #endregion
        
        // Helper class for JSON serialization of string lists
        [System.Serializable]
        private class StringList
        {
            public List<string> items = new List<string>();
        }
    }
}
