using UnityEngine;
using UnityEngine.UI;
using SmartRetailAR.Utils;

namespace SmartRetailAR.UI
{
    /// <summary>
    /// Controller for the home screen
    /// Manages navigation to scanner and profile
    /// </summary>
    public class HomeController : MonoBehaviour
    {
        [Header("UI References")]
        public Button scanButton;
        public Button profileButton;
        public Text welcomeText;
        
        [Header("Animation Settings")]
        public float fadeInDuration = 1f;
        
        private void Start()
        {
            // Setup button listeners
            if (scanButton != null)
            {
                scanButton.onClick.AddListener(OnScanButtonClicked);
            }
            
            if (profileButton != null)
            {
                profileButton.onClick.AddListener(OnProfileButtonClicked);
            }
            
            // Initialize welcome text
            UpdateWelcomeText();
            
            // Play entrance animation
            PlayEntranceAnimation();
        }
        
        /// <summary>
        /// Update welcome text with user name
        /// </summary>
        private void UpdateWelcomeText()
        {
            if (welcomeText != null)
            {
                var preferences = Data.UserPreferences.Load();
                welcomeText.text = $"Bienvenue, {preferences.userName}!";
            }
        }
        
        /// <summary>
        /// Play entrance animation
        /// </summary>
        private void PlayEntranceAnimation()
        {
            // Simple fade-in animation
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            
            canvasGroup.alpha = 0f;
            StartCoroutine(FadeIn(canvasGroup));
        }
        
        /// <summary>
        /// Fade in coroutine
        /// </summary>
        private System.Collections.IEnumerator FadeIn(CanvasGroup canvasGroup)
        {
            float elapsed = 0f;
            
            while (elapsed < fadeInDuration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
                yield return null;
            }
            
            canvasGroup.alpha = 1f;
        }
        
        /// <summary>
        /// Handle scan button click
        /// </summary>
        private void OnScanButtonClicked()
        {
            Debug.Log("Navigating to scanner");
            NavigationManager.Instance.GoToScanner();
        }
        
        /// <summary>
        /// Handle profile button click
        /// </summary>
        private void OnProfileButtonClicked()
        {
            Debug.Log("Navigating to profile");
            NavigationManager.Instance.GoToProfile();
        }
        
        private void OnDestroy()
        {
            // Cleanup button listeners
            if (scanButton != null)
            {
                scanButton.onClick.RemoveListener(OnScanButtonClicked);
            }
            
            if (profileButton != null)
            {
                profileButton.onClick.RemoveListener(OnProfileButtonClicked);
            }
        }
    }
}
