using UnityEngine;
using UnityEngine.UI;
using SmartRetailAR.Utils;
using SmartRetailAR.Data;

namespace SmartRetailAR.UI
{
    public class HomeController : MonoBehaviour
    {
        [Header("UI Elements")]
        public Button scanQRButton;
        public Button scanARButton;
        public Button recommendationsButton;
        public Button profileButton;
        public Button settingsButton;
        public Text welcomeText;
        public Text statsText;

        private void Start()
        {
            SetupButtons();
            UpdateUI();
        }

        private void SetupButtons()
        {
            if (scanQRButton != null)
                scanQRButton.onClick.AddListener(() => NavigationManager.Instance.LoadQRScannerScene());

            if (scanARButton != null)
                scanARButton.onClick.AddListener(() => NavigationManager.Instance.LoadARCameraScene());

            if (recommendationsButton != null)
                recommendationsButton.onClick.AddListener(() => NavigationManager.Instance.LoadRecommendationsScene());

            if (profileButton != null)
                profileButton.onClick.AddListener(() => NavigationManager.Instance.LoadProfileScene());

            if (settingsButton != null)
                settingsButton.onClick.AddListener(() => NavigationManager.Instance.LoadSettingsScene());
        }

        private void UpdateUI()
        {
            string username = PlayerPrefs.GetString("Username", "Utilisateur");
            
            if (welcomeText != null)
                welcomeText.text = $"Bonjour, {username}!";

            if (statsText != null)
            {
                int totalProducts = ProductDatabase.Instance.GetTotalProducts();
                statsText.text = $"{totalProducts} produits disponibles";
            }
        }
    }
}
