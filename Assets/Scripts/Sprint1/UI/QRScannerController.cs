using UnityEngine;
using UnityEngine.UI;
using SmartRetailAR.Utils;
using SmartRetailAR.Data;
using System.Collections;

namespace SmartRetailAR.UI
{
    public class QRScannerController : MonoBehaviour
    {
        [Header("UI Elements")]
        public RawImage cameraPreview;
        public Text statusText;
        public Button cancelButton;
        public GameObject scanFrame;
        public Text instructionsText;

        [Header("Test Mode")]
        public bool testMode = true;
        public float autoScanDelay = 2f;

        private bool _isScanning = false;
        private WebCamTexture _webcamTexture;

        private void Start()
        {
            SetupUI();
            
            QRCodeManager.Instance.OnQRCodeScanned += OnQRCodeScanned;
            QRCodeManager.Instance.OnQRCodeError += OnQRCodeError;

            if (testMode)
            {
                StartCoroutine(TestModeScan());
            }
            else
            {
                StartCamera();
            }
        }

        private void OnDestroy()
        {
            if (QRCodeManager.Instance != null)
            {
                QRCodeManager.Instance.OnQRCodeScanned -= OnQRCodeScanned;
                QRCodeManager.Instance.OnQRCodeError -= OnQRCodeError;
            }

            StopCamera();
        }

        private void SetupUI()
        {
            if (cancelButton != null)
                cancelButton.onClick.AddListener(() => NavigationManager.Instance.GoBack());

            if (instructionsText != null)
                instructionsText.text = testMode ? "Mode Test - Scan automatique" : "Scannez un code QR produit";

            if (statusText != null)
                statusText.text = "Prêt à scanner...";
        }

        private void StartCamera()
        {
            if (WebCamTexture.devices.Length > 0)
            {
                _webcamTexture = new WebCamTexture();
                if (cameraPreview != null)
                    cameraPreview.texture = _webcamTexture;
                _webcamTexture.Play();
                _isScanning = true;
            }
            else
            {
                Debug.LogWarning("No camera detected, using test mode");
                testMode = true;
                StartCoroutine(TestModeScan());
            }
        }

        private void StopCamera()
        {
            if (_webcamTexture != null && _webcamTexture.isPlaying)
            {
                _webcamTexture.Stop();
                _webcamTexture = null;
            }
            _isScanning = false;
        }

        private IEnumerator TestModeScan()
        {
            if (statusText != null)
                statusText.text = "Mode test activé...";

            yield return new WaitForSeconds(autoScanDelay);

            // Get random product from database
            var allProducts = ProductDatabase.Instance.GetAllProducts();
            if (allProducts.Count > 0)
            {
                int randomIndex = Random.Range(0, Mathf.Min(100, allProducts.Count));
                ProductData randomProduct = allProducts[randomIndex];
                
                if (statusText != null)
                    statusText.text = $"Scan simulé: {randomProduct.name}";

                QRCodeManager.Instance.ProcessQRCode(randomProduct.qrCode);
            }
        }

        private void OnQRCodeScanned(QRCodeData qrData)
        {
            Debug.Log($"QR Code scanned: {qrData.code}");
            StopCamera();
            NavigationManager.Instance.LoadProductInfoScene();
        }

        private void OnQRCodeError(string error)
        {
            if (statusText != null)
                statusText.text = $"Erreur: {error}";
            
            Debug.LogError($"QR Scanner Error: {error}");
        }
    }
}
