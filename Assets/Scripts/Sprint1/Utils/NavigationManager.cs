using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace SmartRetailAR.Utils
{
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

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        // Sprint 1 Scenes
        public void LoadSplashScene() => LoadScene("SplashScene");
        public void LoadOnboardingScene() => LoadScene("OnboardingScene");
        public void LoadHomeScene() => LoadScene("HomeScene");
        public void LoadQRScannerScene() => LoadScene("QRScannerScene");
        public void LoadProductInfoScene() => LoadScene("ProductInfoScene");
        public void LoadSettingsScene() => LoadScene("SettingsScene");

        // Sprint 2 Scenes
        public void LoadARCameraScene() => LoadScene("ARCameraScene");
        public void LoadARProductTrackingScene() => LoadScene("ARProductTrackingScene");
        public void LoadAROverlayScene() => LoadScene("AROverlayScene");
        public void LoadARCalibrationScene() => LoadScene("ARCalibrationScene");

        // Sprint 3 Scenes
        public void LoadRecommendationsScene() => LoadScene("RecommendationsScene");
        public void LoadFilterScene() => LoadScene("FilterScene");
        public void LoadComparisonScene() => LoadScene("ComparisonScene");
        public void LoadWishlistScene() => LoadScene("WishlistScene");
        public void LoadProfileScene() => LoadScene("ProfileScene");

        // Sprint 4 Scenes
        public void LoadTestModeScene() => LoadScene("TestModeScene");
        public void LoadAnalyticsScene() => LoadScene("AnalyticsScene");
        public void LoadFeedbackScene() => LoadScene("FeedbackScene");
        public void LoadTutorialScene() => LoadScene("TutorialScene");
        public void LoadDebugScene() => LoadScene("DebugScene");

        private void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            
            while (!asyncLoad.isDone)
            {
                float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                Debug.Log($"Loading {sceneName}: {progress * 100}%");
                yield return null;
            }

            Debug.Log($"Loaded scene: {sceneName}");
        }

        public void GoBack()
        {
            if (SceneManager.sceneCount > 1)
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            }
            else
            {
                LoadHomeScene();
            }
        }

        public string GetCurrentScene()
        {
            return SceneManager.GetActiveScene().name;
        }
    }
}
