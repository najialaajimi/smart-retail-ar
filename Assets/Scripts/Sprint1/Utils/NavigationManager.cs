using UnityEngine;
using UnityEngine.SceneManagement;
using SmartRetailAR.Data;
using System.Collections.Generic;

namespace SmartRetailAR.Utils
{
    /// <summary>
    /// Singleton manager for navigation between scenes and UI panels
    /// </summary>
    public class NavigationManager : MonoBehaviour
    {
        private static NavigationManager _instance;
        public static NavigationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("NavigationManager");
                    _instance = go.AddComponent<NavigationManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }
        
        private Stack<string> _navigationStack = new Stack<string>();
        private ProductData _currentProduct;
        
        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        public void ShowHome()
        {
            LoadScene("HomeScene");
        }
        
        public void ShowScanner()
        {
            LoadScene("ScannerScene");
        }
        
        public void ShowProductDetails(ProductData product)
        {
            _currentProduct = product;
            LoadScene("ProductInfoScene");
        }
        
        public void ShowARView(ProductData product)
        {
            _currentProduct = product;
            LoadScene("ARScene");
        }
        
        public void ShowRecommendations(ProductData product)
        {
            _currentProduct = product;
            LoadScene("RecommendationsScene");
        }
        
        public void ShowProfile()
        {
            LoadScene("ProfileScene");
        }
        
        public void ShowSettings()
        {
            LoadScene("SettingsScene");
        }
        
        public void GoBack()
        {
            if (_navigationStack.Count > 0)
            {
                string previousScene = _navigationStack.Pop();
                SceneManager.LoadScene(previousScene);
            }
            else
            {
                ShowHome();
            }
        }
        
        private void LoadScene(string sceneName)
        {
            string currentScene = SceneManager.GetActiveScene().name;
            if (currentScene != sceneName)
            {
                _navigationStack.Push(currentScene);
            }
            SceneManager.LoadScene(sceneName);
        }
        
        public ProductData GetCurrentProduct()
        {
            return _currentProduct;
        }
    }
}
