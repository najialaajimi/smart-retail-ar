using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SmartRetailAR.Data;
using SmartRetailAR.Sprint1.Core;

namespace SmartRetailAR.Sprint1.UI
{
    /// <summary>
    /// Controls the QR scanner UI and handles scan results
    /// </summary>
    public class QRScannerUI : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Button scanButton;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private GameObject scannerOverlay;
        [SerializeField] private GameObject loadingIndicator;
        [SerializeField] private RawImage cameraPreview;
        
        [Header("Result Panel")]
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private TextMeshProUGUI productNameText;
        [SerializeField] private TextMeshProUGUI productBrandText;
        [SerializeField] private TextMeshProUGUI productPriceText;
        [SerializeField] private Button viewDetailsButton;
        [SerializeField] private Button scanAgainButton;
        
        [Header("References")]
        [SerializeField] private QRScanner qrScanner;
        
        private ProductData _currentProduct;
        
        void Start()
        {
            SetupUI();
            SubscribeToEvents();
        }
        
        void SetupUI()
        {
            if (scanButton != null)
                scanButton.onClick.AddListener(OnScanButtonClicked);
            
            if (viewDetailsButton != null)
                viewDetailsButton.onClick.AddListener(OnViewDetailsClicked);
            
            if (scanAgainButton != null)
                scanAgainButton.onClick.AddListener(OnScanAgainClicked);
            
            // Initialize UI state
            ShowScannerView();
        }
        
        void SubscribeToEvents()
        {
            if (qrScanner != null)
            {
                qrScanner.OnQRCodeScanned += OnQRCodeScanned;
                qrScanner.OnProductFound += OnProductFound;
                qrScanner.OnScanFailed += OnScanFailed;
            }
        }
        
        void OnScanButtonClicked()
        {
            UpdateStatus("Scanning for QR code...");
            ShowLoadingIndicator(true);
            
            if (qrScanner != null)
            {
                qrScanner.StartScanning();
            }
        }
        
        void OnQRCodeScanned(string qrCode)
        {
            UpdateStatus($"QR Code detected: {qrCode}");
        }
        
        void OnProductFound(ProductData product)
        {
            _currentProduct = product;
            ShowProductResult(product);
            ShowLoadingIndicator(false);
            UpdateStatus("Product found!");
        }
        
        void OnScanFailed()
        {
            ShowLoadingIndicator(false);
            UpdateStatus("Product not found. Please try again.");
        }
        
        void ShowProductResult(ProductData product)
        {
            if (resultPanel != null)
            {
                resultPanel.SetActive(true);
                
                if (productNameText != null)
                    productNameText.text = product.name;
                
                if (productBrandText != null)
                    productBrandText.text = product.brand;
                
                if (productPriceText != null)
                    productPriceText.text = $"{product.price:F2} €";
            }
            
            if (scannerOverlay != null)
                scannerOverlay.SetActive(false);
        }
        
        void OnViewDetailsClicked()
        {
            if (_currentProduct != null)
            {
                NavigationManager.Instance.ShowProductDetails(_currentProduct);
            }
        }
        
        void OnScanAgainClicked()
        {
            ShowScannerView();
        }
        
        void ShowScannerView()
        {
            if (resultPanel != null)
                resultPanel.SetActive(false);
            
            if (scannerOverlay != null)
                scannerOverlay.SetActive(true);
            
            UpdateStatus("Point camera at QR code");
            ShowLoadingIndicator(false);
        }
        
        void UpdateStatus(string message)
        {
            if (statusText != null)
                statusText.text = message;
            
            Debug.Log($"Scanner Status: {message}");
        }
        
        void ShowLoadingIndicator(bool show)
        {
            if (loadingIndicator != null)
                loadingIndicator.SetActive(show);
        }
        
        void OnDestroy()
        {
            if (qrScanner != null)
            {
                qrScanner.OnQRCodeScanned -= OnQRCodeScanned;
                qrScanner.OnProductFound -= OnProductFound;
                qrScanner.OnScanFailed -= OnScanFailed;
            }
            
            if (scanButton != null)
                scanButton.onClick.RemoveAllListeners();
            
            if (viewDetailsButton != null)
                viewDetailsButton.onClick.RemoveAllListeners();
            
            if (scanAgainButton != null)
                scanAgainButton.onClick.RemoveAllListeners();
        }
    }
}
