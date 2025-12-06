using UnityEngine;
using UnityEngine.UI;
using SmartRetailAR.Utils;
using SmartRetailAR.Data;

namespace SmartRetailAR.UI
{
    /// <summary>
    /// Controller for QR code scanner interface
    /// Manages camera view and QR code scanning
    /// </summary>
    public class ScannerController : MonoBehaviour
    {
        [Header("UI References")]
        public Button scanButton;
        public Button torchButton;
        public Button backButton;
        public Text statusText;
        public RawImage cameraView;
        public GameObject scanOverlay;
        
        [Header("Test Mode")]
        public bool testMode = true;
        public string[] testProductIds = new string[] 
        { 
            "prod001", "prod002", "prod003", "prod004", "prod005",
            "prod006", "prod007", "prod008", "prod009", "prod010"
        };
        
        [Header("Scanner Settings")]
        public float scanCooldown = 1f;
        
        private bool _isScanning = false;
        private float _lastScanTime;
        private bool _torchEnabled = false;
        
        private void Start()
        {
            // Setup button listeners
            if (scanButton != null)
            {
                scanButton.onClick.AddListener(OnScanButtonClicked);
            }
            
            if (torchButton != null)
            {
                torchButton.onClick.AddListener(OnTorchButtonClicked);
            }
            
            if (backButton != null)
            {
                backButton.onClick.AddListener(OnBackButtonClicked);
            }
            
            // Initialize scanner
            InitializeScanner();
            
            // Update status
            UpdateStatusText("Pointez vers le QR code du produit");
        }
        
        /// <summary>
        /// Initialize scanner (camera or test mode)
        /// </summary>
        private void InitializeScanner()
        {
            if (testMode)
            {
                Debug.Log("Scanner in TEST MODE - using simulated products");
                if (cameraView != null)
                {
                    // Display placeholder for test mode
                    cameraView.color = new Color(0.2f, 0.2f, 0.2f, 1f);
                }
            }
            else
            {
                // Initialize AR camera for real QR scanning
                // This will be implemented in Sprint 2 with AR Foundation
                Debug.Log("Scanner in REAL MODE - AR camera not yet implemented");
            }
        }
        
        /// <summary>
        /// Handle scan button click
        /// </summary>
        private void OnScanButtonClicked()
        {
            if (_isScanning) return;
            
            // Check cooldown
            if (Time.time - _lastScanTime < scanCooldown)
            {
                return;
            }
            
            _lastScanTime = Time.time;
            
            if (testMode)
            {
                // Simulate QR scan with random test product
                SimulateScan();
            }
            else
            {
                // Trigger real QR scan
                StartRealScan();
            }
        }
        
        /// <summary>
        /// Simulate QR code scan in test mode
        /// </summary>
        private void SimulateScan()
        {
            if (testProductIds == null || testProductIds.Length == 0)
            {
                UpdateStatusText("Erreur: Aucun produit de test disponible");
                return;
            }
            
            _isScanning = true;
            UpdateStatusText("Scan en cours...");
            
            // Pick random test product
            string productId = testProductIds[Random.Range(0, testProductIds.Length)];
            
            // Simulate scan delay
            StartCoroutine(ProcessScanResult(productId));
        }
        
        /// <summary>
        /// Start real QR scan
        /// </summary>
        private void StartRealScan()
        {
            _isScanning = true;
            UpdateStatusText("Scan en cours...");
            
            // Real scanning will be implemented with AR Foundation
            Debug.Log("Real QR scanning not yet implemented");
            
            _isScanning = false;
            UpdateStatusText("Pointez vers le QR code du produit");
        }
        
        /// <summary>
        /// Process scan result
        /// </summary>
        private System.Collections.IEnumerator ProcessScanResult(string productId)
        {
            // Simulate processing delay
            yield return new WaitForSeconds(0.5f);
            
            // Verify product exists
            var product = ProductDatabase.Instance.GetProduct(productId);
            
            if (product != null)
            {
                UpdateStatusText($"Produit trouvé: {product.name}");
                
                // Set current product in QRCodeManager
                QRCodeManager.Instance.SetCurrentProductId(productId);
                
                // Wait a moment then navigate
                yield return new WaitForSeconds(0.5f);
                
                // Navigate to product info
                NavigationManager.Instance.GoToProductInfo();
            }
            else
            {
                UpdateStatusText("Produit non trouvé");
                _isScanning = false;
                
                // Reset status after delay
                yield return new WaitForSeconds(2f);
                UpdateStatusText("Pointez vers le QR code du produit");
            }
        }
        
        /// <summary>
        /// Handle torch button click
        /// </summary>
        private void OnTorchButtonClicked()
        {
            _torchEnabled = !_torchEnabled;
            
            // Toggle torch (will be implemented with AR camera)
            Debug.Log($"Torch {(_torchEnabled ? "enabled" : "disabled")}");
            
            // Update button visual state
            if (torchButton != null)
            {
                var colors = torchButton.colors;
                colors.normalColor = _torchEnabled ? Color.yellow : Color.white;
                torchButton.colors = colors;
            }
        }
        
        /// <summary>
        /// Handle back button click
        /// </summary>
        private void OnBackButtonClicked()
        {
            NavigationManager.Instance.NavigateBack();
        }
        
        /// <summary>
        /// Update status text
        /// </summary>
        private void UpdateStatusText(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
            
            Debug.Log($"Scanner Status: {message}");
        }
        
        private void OnDestroy()
        {
            // Cleanup button listeners
            if (scanButton != null)
            {
                scanButton.onClick.RemoveListener(OnScanButtonClicked);
            }
            
            if (torchButton != null)
            {
                torchButton.onClick.RemoveListener(OnTorchButtonClicked);
            }
            
            if (backButton != null)
            {
                backButton.onClick.RemoveListener(OnBackButtonClicked);
            }
        }
    }
}
