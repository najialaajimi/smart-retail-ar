using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ScannerController : MonoBehaviour
{
    [Header("UI References")]
    public RawImage cameraPreview;
    public Image scanFrame;
    public Button captureButton;
    public Button torchButton;
    public Button backButton;
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI statusText;
    public GameObject scanningOverlay;

    [Header("Scan Settings")]
    public float scanAnimationDuration = 0.5f;
    public Color scanningColor = Color.green;
    public Color successColor = Color.blue;
    public Color errorColor = Color.red;

    [Header("Test Mode")]
    public bool testMode = true;
    public string[] testProductIds = { "PROD001", "PROD002", "PROD003" };

    private bool isScanning = false;
    private bool isTorchEnabled = false;
    private QRCodeManager qrManager;

    void Start()
    {
        InitializeScanner();
        SetupButtons();
        SetupQRManager();
    }

    void InitializeScanner()
    {
        if (instructionText != null)
        {
            instructionText.text = "Pointez vers le QR code du produit";
        }

        if (statusText != null)
        {
            statusText.text = "";
        }

        if (scanningOverlay != null)
        {
            scanningOverlay.SetActive(false);
        }

        UpdateTorchButton();
    }

    void SetupButtons()
    {
        if (captureButton != null)
        {
            captureButton.onClick.AddListener(OnCaptureButtonClicked);
        }

        if (torchButton != null)
        {
            torchButton.onClick.AddListener(OnTorchButtonClicked);
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }
    }

    void SetupQRManager()
    {
        qrManager = QRCodeManager.Instance;
        qrManager.OnQRCodeScanned += OnQRCodeScanned;
        qrManager.OnScanError += OnScanError;
        qrManager.StartScanning();
    }

    void OnCaptureButtonClicked()
    {
        if (isScanning)
        {
            return;
        }

        StartScan();
    }

    void StartScan()
    {
        isScanning = true;
        
        if (scanningOverlay != null)
        {
            scanningOverlay.SetActive(true);
        }

        if (statusText != null)
        {
            statusText.text = "Scan en cours...";
            statusText.color = scanningColor;
        }

        // In test mode, simulate a scan
        if (testMode && testProductIds.Length > 0)
        {
            string randomProductId = testProductIds[Random.Range(0, testProductIds.Length)];
            StartCoroutine(SimulateScanDelay(randomProductId));
        }
        else
        {
            // TODO: Actual camera scanning with AR Foundation
            Debug.Log("Starting actual QR code scan");
        }
    }

    IEnumerator SimulateScanDelay(string productId)
    {
        yield return new WaitForSeconds(1.5f);
        qrManager.SimulateScan(productId);
    }

    void OnQRCodeScanned(string productId)
    {
        isScanning = false;

        if (statusText != null)
        {
            statusText.text = "Scan réussi !";
            statusText.color = successColor;
        }

        // Save to user history
        UserPreferences prefs = UserPreferences.Load();
        prefs.AddScannedProduct(productId);
        prefs.Save();

        // Navigate to product info after short delay
        StartCoroutine(NavigateAfterDelay(1f));
    }

    void OnScanError(string errorMessage)
    {
        isScanning = false;

        if (scanningOverlay != null)
        {
            scanningOverlay.SetActive(false);
        }

        if (statusText != null)
        {
            statusText.text = $"Erreur: {errorMessage}";
            statusText.color = errorColor;
        }

        StartCoroutine(ClearStatusAfterDelay(3f));
    }

    IEnumerator NavigateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        NavigationManager.Instance.NavigateToProductInfo();
    }

    IEnumerator ClearStatusAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (statusText != null)
        {
            statusText.text = "";
        }
    }

    void OnTorchButtonClicked()
    {
        isTorchEnabled = !isTorchEnabled;
        qrManager.EnableTorch(isTorchEnabled);
        UpdateTorchButton();
    }

    void UpdateTorchButton()
    {
        if (torchButton != null)
        {
            // TODO: Update button visual state
            Debug.Log($"Torch button state: {isTorchEnabled}");
        }
    }

    void OnBackButtonClicked()
    {
        qrManager.StopScanning();
        NavigationManager.Instance.NavigateBack();
    }

    void OnDestroy()
    {
        if (qrManager != null)
        {
            qrManager.OnQRCodeScanned -= OnQRCodeScanned;
            qrManager.OnScanError -= OnScanError;
            qrManager.StopScanning();
        }

        if (captureButton != null)
        {
            captureButton.onClick.RemoveListener(OnCaptureButtonClicked);
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
