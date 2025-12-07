using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SmartRetailAR.UI
{
    /// <summary>
    /// Controller for the Home scene - main entry point of the application
    /// </summary>
    public class HomeController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button scanButton;
        [SerializeField] private Button recommendationsButton;
        [SerializeField] private Button profileButton;
        [SerializeField] private Button historyButton;
        [SerializeField] private TextMeshProUGUI welcomeText;
        [SerializeField] private TextMeshProUGUI statsText;

        private void Start()
        {
            InitializeUI();
            LoadUserStats();
        }

        private void InitializeUI()
        {
            if (scanButton != null)
                scanButton.onClick.AddListener(OnScanButtonClicked);
            
            if (recommendationsButton != null)
                recommendationsButton.onClick.AddListener(OnRecommendationsButtonClicked);
            
            if (profileButton != null)
                profileButton.onClick.AddListener(OnProfileButtonClicked);
            
            if (historyButton != null)
                historyButton.onClick.AddListener(OnHistoryButtonClicked);

            UpdateWelcomeText();
        }

        private void UpdateWelcomeText()
        {
            if (welcomeText != null)
            {
                string userName = PlayerPrefs.GetString("UserName", "Utilisateur");
                welcomeText.text = $"Bienvenue, {userName}!";
            }
        }

        private void LoadUserStats()
        {
            if (statsText != null)
            {
                int scannedProducts = PlayerPrefs.GetInt("TotalScannedProducts", 0);
                statsText.text = $"Produits scannés: {scannedProducts}";
            }
        }

        private void OnScanButtonClicked()
        {
            Debug.Log("Navigating to Scanner scene");
            Utils.NavigationManager.Instance.LoadScene("ARScannerScene");
        }

        private void OnRecommendationsButtonClicked()
        {
            Debug.Log("Navigating to Recommendations scene");
            Utils.NavigationManager.Instance.LoadScene("RecommendationsScene");
        }

        private void OnProfileButtonClicked()
        {
            Debug.Log("Navigating to Profile scene");
            Utils.NavigationManager.Instance.LoadScene("ProfileScene");
        }

        private void OnHistoryButtonClicked()
        {
            Debug.Log("Navigating to History scene");
            Utils.NavigationManager.Instance.LoadScene("HistoryScene");
        }

        private void OnDestroy()
        {
            if (scanButton != null)
                scanButton.onClick.RemoveListener(OnScanButtonClicked);
            
            if (recommendationsButton != null)
                recommendationsButton.onClick.RemoveListener(OnRecommendationsButtonClicked);
            
            if (profileButton != null)
                profileButton.onClick.RemoveListener(OnProfileButtonClicked);
            
            if (historyButton != null)
                historyButton.onClick.RemoveListener(OnHistoryButtonClicked);
        }
    }
}
