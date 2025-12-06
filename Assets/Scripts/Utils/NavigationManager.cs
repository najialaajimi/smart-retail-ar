using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace SmartRetailAR.Utils
{
    /// <summary>
    /// Singleton manager for scene navigation
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
        
        // Current scene tracking
        private string _currentScene;
        private string _previousScene;
        
        // Scene name constants
        public const string SCENE_MAIN = "MainScene";
        public const string SCENE_HOME = "HomeScene";
        public const string SCENE_SCANNER = "ScannerScene";
        public const string SCENE_PRODUCT_INFO = "ProductInfoScene";
        public const string SCENE_RECOMMENDATIONS = "RecommendationsScene";
        public const string SCENE_PROFILE = "ProfileScene";
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        /// <summary>
        /// Navigate to a scene by name
        /// </summary>
        public void NavigateTo(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("Scene name is null or empty");
                return;
            }
            
            _previousScene = _currentScene;
            _currentScene = sceneName;
            
            StartCoroutine(LoadSceneAsync(sceneName));
        }
        
        /// <summary>
        /// Navigate back to previous scene
        /// </summary>
        public void NavigateBack()
        {
            if (!string.IsNullOrEmpty(_previousScene))
            {
                NavigateTo(_previousScene);
            }
            else
            {
                NavigateTo(SCENE_HOME);
            }
        }
        
        /// <summary>
        /// Load scene asynchronously
        /// </summary>
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            
            while (!asyncLoad.isDone)
            {
                float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                // You can add loading UI updates here
                yield return null;
            }
        }
        
        /// <summary>
        /// Navigate to home scene
        /// </summary>
        public void GoToHome()
        {
            NavigateTo(SCENE_HOME);
        }
        
        /// <summary>
        /// Navigate to scanner scene
        /// </summary>
        public void GoToScanner()
        {
            NavigateTo(SCENE_SCANNER);
        }
        
        /// <summary>
        /// Navigate to product info scene
        /// </summary>
        public void GoToProductInfo()
        {
            NavigateTo(SCENE_PRODUCT_INFO);
        }
        
        /// <summary>
        /// Navigate to recommendations scene
        /// </summary>
        public void GoToRecommendations()
        {
            NavigateTo(SCENE_RECOMMENDATIONS);
        }
        
        /// <summary>
        /// Navigate to profile scene
        /// </summary>
        public void GoToProfile()
        {
            NavigateTo(SCENE_PROFILE);
        }
        
        /// <summary>
        /// Get current scene name
        /// </summary>
        public string GetCurrentScene()
        {
            return _currentScene;
        }
        
        /// <summary>
        /// Quit application
        /// </summary>
        public void QuitApp()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
