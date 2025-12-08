using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace SmartRetailAR.Utils
{
    /// <summary>
    /// Singleton manager for scene navigation and transitions
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

        [Header("Loading Settings")]
        [SerializeField] private float minLoadingTime = 0.5f;
        
        private bool isLoading = false;
        private string currentScene;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                currentScene = SceneManager.GetActiveScene().name;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Load a scene by name
        /// </summary>
        public void LoadScene(string sceneName)
        {
            if (isLoading)
            {
                Debug.LogWarning($"Already loading a scene. Ignoring request to load {sceneName}");
                return;
            }

            StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        /// <summary>
        /// Load scene asynchronously with loading time
        /// </summary>
        private IEnumerator LoadSceneCoroutine(string sceneName)
        {
            isLoading = true;
            float startTime = Time.time;

            Debug.Log($"Loading scene: {sceneName}");

            // Start loading the scene
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            
            if (asyncLoad == null)
            {
                Debug.LogError($"Failed to load scene: {sceneName}");
                isLoading = false;
                yield break;
            }

            // Wait until the scene is loaded
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // Ensure minimum loading time for smooth transition
            float elapsedTime = Time.time - startTime;
            if (elapsedTime < minLoadingTime)
            {
                yield return new WaitForSeconds(minLoadingTime - elapsedTime);
            }

            currentScene = sceneName;
            isLoading = false;
            
            Debug.Log($"Scene loaded: {sceneName}");
        }

        /// <summary>
        /// Reload the current scene
        /// </summary>
        public void ReloadCurrentScene()
        {
            LoadScene(currentScene);
        }

        /// <summary>
        /// Go back to previous scene (simple implementation)
        /// </summary>
        public void GoBack()
        {
            LoadScene("HomeScene");
        }

        /// <summary>
        /// Get current scene name
        /// </summary>
        public string GetCurrentScene()
        {
            return currentScene;
        }

        /// <summary>
        /// Check if currently loading
        /// </summary>
        public bool IsLoading()
        {
            return isLoading;
        }
    }
}
