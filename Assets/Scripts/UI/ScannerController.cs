using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SmartRetailAR.Data;

namespace SmartRetailAR.UI
{
    /// <summary>
    /// Controller for the Scanner scene - handles AR product scanning
    /// </summary>
    public class ScannerController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI instructionText;
        [SerializeField] private GameObject scanningIndicator;
        [SerializeField] private GameObject arCameraObject;

        [Header("Test Mode")]
        [SerializeField] private bool testMode = false;
        [SerializeField] private string[] testProductIds = { "PROD001", "PROD002", "PROD003" };

        private bool isScanning = false;

        private void Start()
        {
            InitializeUI();
            StartScanning();
        }

        private void InitializeUI()
        {
            if (backButton != null)
                backButton.onClick.AddListener(OnBackButtonClicked);

            if (instructionText != null)
                instructionText.text = "Pointez votre caméra vers un produit";

            if (scanningIndicator != null)
                scanningIndicator.SetActive(false);
        }

        private void StartScanning()
        {
            isScanning = true;
            if (scanningIndicator != null)
                scanningIndicator.SetActive(true);

            if (testMode)
            {
                // Simulate scanning in test mode
                Invoke(nameof(SimulateScan), 2f);
            }
        }

        private void SimulateScan()
        {
            if (testProductIds.Length > 0)
            {
                string randomProductId = testProductIds[Random.Range(0, testProductIds.Length)];
                OnProductDetected(randomProductId);
            }
        }

        public void OnProductDetected(string productId)
        {
            Debug.Log($"Product detected: {productId}");
            
            isScanning = false;
            if (scanningIndicator != null)
                scanningIndicator.SetActive(false);

            // Load product data and navigate to product info
            ProductData product = ProductDatabase.Instance.GetProductById(productId);
            if (product != null)
            {
                PlayerPrefs.SetString("CurrentProductId", productId);
                PlayerPrefs.SetInt("TotalScannedProducts", PlayerPrefs.GetInt("TotalScannedProducts", 0) + 1);
                Utils.NavigationManager.Instance.LoadScene("ProductInfoScene");
            }
            else
            {
                Debug.LogError($"Product not found: {productId}");
                if (instructionText != null)
                    instructionText.text = "Produit non reconnu. Réessayez.";
                
                Invoke(nameof(StartScanning), 2f);
            }
        }

        private void OnBackButtonClicked()
        {
            Debug.Log("Returning to Home scene");
            Utils.NavigationManager.Instance.LoadScene("HomeScene");
        }

        private void OnDestroy()
        {
            if (backButton != null)
                backButton.onClick.RemoveListener(OnBackButtonClicked);
        }
    }
}
