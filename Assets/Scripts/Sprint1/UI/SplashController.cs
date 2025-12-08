using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartRetailAR.Utils;

namespace SmartRetailAR.UI
{
    public class SplashController : MonoBehaviour
    {
        [Header("UI Elements")]
        public Image logoImage;
        public Text versionText;
        public Slider loadingBar;

        [Header("Settings")]
        public float splashDuration = 3f;
        public bool skipToOnboarding = true;

        private void Start()
        {
            if (versionText != null)
                versionText.text = $"Version {Application.version}";

            StartCoroutine(LoadingSequence());
        }

        private IEnumerator LoadingSequence()
        {
            float elapsed = 0f;

            while (elapsed < splashDuration)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / splashDuration;

                if (loadingBar != null)
                    loadingBar.value = progress;

                yield return null;
            }

            // Check if first launch
            bool isFirstLaunch = !PlayerPrefs.HasKey("HasCompletedOnboarding");

            if (isFirstLaunch && skipToOnboarding)
            {
                NavigationManager.Instance.LoadOnboardingScene();
            }
            else
            {
                NavigationManager.Instance.LoadHomeScene();
            }
        }
    }
}
