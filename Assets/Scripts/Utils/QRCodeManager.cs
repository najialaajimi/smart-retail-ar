using System;
using UnityEngine;

public class QRCodeManager : MonoBehaviour
{
    private static QRCodeManager instance;
    public static QRCodeManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("QRCodeManager");
                instance = go.AddComponent<QRCodeManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    public event Action<string> OnQRCodeScanned;
    public event Action<string> OnScanError;

    private bool isScanning = false;
    private string lastScannedCode = "";
    private string currentProductId = "";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void StartScanning()
    {
        isScanning = true;
        Debug.Log("QR Code scanning started");
    }

    public void StopScanning()
    {
        isScanning = false;
        Debug.Log("QR Code scanning stopped");
    }

    public bool IsScanning()
    {
        return isScanning;
    }

    // Simulation method for testing - in production, this would integrate with AR Foundation
    public void SimulateScan(string qrCodeData)
    {
        if (!isScanning)
        {
            Debug.LogWarning("Cannot scan - scanner is not active");
            return;
        }

        lastScannedCode = qrCodeData;
        ProcessQRCode(qrCodeData);
    }

    private void ProcessQRCode(string qrData)
    {
        if (string.IsNullOrEmpty(qrData))
        {
            OnScanError?.Invoke("QR code vide ou invalide");
            return;
        }

        // Extract product ID from QR code
        // Format expected: "PRODUCT:productId" or just "productId"
        string productId = ExtractProductId(qrData);

        if (string.IsNullOrEmpty(productId))
        {
            OnScanError?.Invoke("Format de QR code invalide");
            return;
        }

        // Verify product exists in database
        ProductData product = ProductDatabase.Instance.GetProductById(productId);
        if (product == null)
        {
            OnScanError?.Invoke("Produit non trouvé dans la base de données");
            return;
        }

        currentProductId = productId;
        OnQRCodeScanned?.Invoke(productId);
        Debug.Log($"QR Code scanned successfully: {productId}");
    }

    private string ExtractProductId(string qrData)
    {
        if (qrData.StartsWith("PRODUCT:"))
        {
            return qrData.Substring(8);
        }
        return qrData;
    }

    public string GetLastScannedCode()
    {
        return lastScannedCode;
    }

    public string GetCurrentProductId()
    {
        return currentProductId;
    }

    public void SetCurrentProductId(string productId)
    {
        currentProductId = productId;
    }

    // Camera control methods (for integration with AR Foundation)
    public void EnableTorch(bool enable)
    {
        Debug.Log($"Torch {(enable ? "enabled" : "disabled")}");
        // TODO: Implement with AR Foundation camera
    }

    public void SetCameraResolution(int width, int height)
    {
        Debug.Log($"Camera resolution set to {width}x{height}");
        // TODO: Implement with AR Foundation camera
    }
}
