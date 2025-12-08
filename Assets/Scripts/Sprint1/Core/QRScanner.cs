using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ZXing;
using System.Collections;
using SmartRetailAR.Data;
using SmartRetailAR.Utils;

namespace SmartRetailAR.Sprint1.Core
{
    /// <summary>
    /// Handles QR code scanning using the device camera.
    /// Supports both real AR scanning and test mode for development.
    /// </summary>
    public class QRScanner : MonoBehaviour
    {
        [Header("AR Components")]
        [SerializeField] private ARCameraManager arCameraManager;
        
        [Header("Test Mode")]
        [SerializeField] private bool testMode = true;
        [SerializeField] private string[] testProductIds = { "PROD001", "PROD002", "PROD003" };
        
        [Header("Settings")]
        [SerializeField] private float scanInterval = 0.5f;
        [SerializeField] private bool continuousScanning = false;
        
        private IBarcodeReader _barcodeReader;
        private Texture2D _cameraTexture;
        private bool _isScanning = false;
        private float _lastScanTime = 0f;
        
        // Events
        public System.Action<string> OnQRCodeScanned;
        public System.Action<ProductData> OnProductFound;
        public System.Action OnScanFailed;
        
        void Start()
        {
            InitializeScanner();
        }
        
        void InitializeScanner()
        {
            _barcodeReader = new BarcodeReader
            {
                AutoRotate = true,
                Options = new ZXing.Common.DecodingOptions
                {
                    TryHarder = true,
                    PossibleFormats = new BarcodeFormat[]
                    {
                        BarcodeFormat.QR_CODE,
                        BarcodeFormat.EAN_13,
                        BarcodeFormat.EAN_8,
                        BarcodeFormat.CODE_128
                    }
                }
            };
            
            Debug.Log($"QR Scanner initialized. Test Mode: {testMode}");
        }
        
        /// <summary>
        /// Starts scanning for QR codes
        /// </summary>
        public void StartScanning()
        {
            if (_isScanning) return;
            
            _isScanning = true;
            
            if (testMode)
            {
                StartCoroutine(TestModeScan());
            }
            else
            {
                StartCoroutine(ScanRoutine());
            }
            
            Debug.Log("QR scanning started");
        }
        
        /// <summary>
        /// Stops scanning
        /// </summary>
        public void StopScanning()
        {
            _isScanning = false;
            Debug.Log("QR scanning stopped");
        }
        
        /// <summary>
        /// Test mode - simulates QR code scanning without AR hardware
        /// </summary>
        private IEnumerator TestModeScan()
        {
            yield return new WaitForSeconds(1.5f);
            
            if (!_isScanning) yield break;
            
            // Randomly select a test product
            string testProductId = testProductIds[Random.Range(0, testProductIds.Length)];
            
            Debug.Log($"Test Mode: Simulated scan of product {testProductId}");
            
            OnQRCodeScanned?.Invoke(testProductId);
            
            // Look up product in database
            var product = ProductDatabaseManager.Instance.GetProductById(testProductId);
            if (product != null)
            {
                OnProductFound?.Invoke(product);
                Debug.Log($"Product found: {product.name}");
            }
            else
            {
                OnScanFailed?.Invoke();
                Debug.LogWarning($"Product not found: {testProductId}");
            }
            
            if (!continuousScanning)
            {
                _isScanning = false;
            }
        }
        
        /// <summary>
        /// Real scanning routine using AR camera
        /// </summary>
        private IEnumerator ScanRoutine()
        {
            while (_isScanning)
            {
                if (Time.time - _lastScanTime < scanInterval)
                {
                    yield return null;
                    continue;
                }
                
                _lastScanTime = Time.time;
                
                // Try to acquire camera image
                if (arCameraManager != null && arCameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
                {
                    ProcessCameraImage(image);
                    image.Dispose();
                }
                
                yield return null;
            }
        }
        
        private void ProcessCameraImage(XRCpuImage image)
        {
            // Convert XRCpuImage to Texture2D
            var conversionParams = new XRCpuImage.ConversionParams
            {
                inputRect = new RectInt(0, 0, image.width, image.height),
                outputDimensions = new Vector2Int(image.width / 2, image.height / 2),
                outputFormat = TextureFormat.RGB24,
                transformation = XRCpuImage.Transformation.MirrorY
            };
            
            if (_cameraTexture == null || 
                _cameraTexture.width != conversionParams.outputDimensions.x || 
                _cameraTexture.height != conversionParams.outputDimensions.y)
            {
                _cameraTexture = new Texture2D(
                    conversionParams.outputDimensions.x,
                    conversionParams.outputDimensions.y,
                    TextureFormat.RGB24,
                    false);
            }
            
            var rawTextureData = _cameraTexture.GetRawTextureData<byte>();
            
            try
            {
                image.Convert(conversionParams, rawTextureData);
                _cameraTexture.Apply();
                
                // Decode QR code
                var result = _barcodeReader.Decode(_cameraTexture.GetPixels32(), 
                    _cameraTexture.width, _cameraTexture.height);
                
                if (result != null)
                {
                    ProcessScanResult(result.Text);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error processing camera image: {e.Message}");
            }
        }
        
        private void ProcessScanResult(string qrCode)
        {
            Debug.Log($"QR Code scanned: {qrCode}");
            OnQRCodeScanned?.Invoke(qrCode);
            
            // Try to find product by QR code first, then by barcode
            var product = ProductDatabaseManager.Instance.GetProductByQRCode(qrCode);
            if (product == null)
            {
                product = ProductDatabaseManager.Instance.GetProductByBarcode(qrCode);
            }
            
            if (product != null)
            {
                OnProductFound?.Invoke(product);
                AnalyticsManager.Instance.TrackProductScan(product.id, true);
                
                if (!continuousScanning)
                {
                    StopScanning();
                }
            }
            else
            {
                OnScanFailed?.Invoke();
                AnalyticsManager.Instance.TrackProductScan(qrCode, false);
                Debug.LogWarning($"No product found for code: {qrCode}");
            }
        }
        
        void OnDestroy()
        {
            if (_cameraTexture != null)
            {
                Destroy(_cameraTexture);
            }
        }
    }
}
