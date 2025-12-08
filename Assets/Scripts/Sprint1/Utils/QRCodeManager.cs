using UnityEngine;
using SmartRetailAR.Data;
using System;

namespace SmartRetailAR.Utils
{
    public class QRCodeManager : MonoBehaviour
    {
        private static QRCodeManager _instance;
        public static QRCodeManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("QRCodeManager");
                    _instance = go.AddComponent<QRCodeManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        public event Action<QRCodeData> OnQRCodeScanned;
        public event Action<string> OnQRCodeError;

        private QRCodeData _lastScannedCode;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void ProcessQRCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                OnQRCodeError?.Invoke("Invalid QR code");
                return;
            }

            // Try to find product by QR code
            ProductData product = ProductDatabase.Instance.GetProductByQRCode(code);

            if (product != null)
            {
                QRCodeData qrData = new QRCodeData(code, product.id);
                _lastScannedCode = qrData;
                OnQRCodeScanned?.Invoke(qrData);
                Debug.Log($"QR Code scanned: {code} -> Product: {product.name}");
            }
            else
            {
                OnQRCodeError?.Invoke($"Product not found for QR code: {code}");
                Debug.LogWarning($"No product found for QR code: {code}");
            }
        }

        public QRCodeData GetLastScannedCode()
        {
            return _lastScannedCode;
        }

        public void ClearLastScannedCode()
        {
            _lastScannedCode = null;
        }

        // Simulate QR code scan for testing
        public void SimulateScan(string productId)
        {
            ProductData product = ProductDatabase.Instance.GetProductById(productId);
            if (product != null)
            {
                ProcessQRCode(product.qrCode);
            }
            else
            {
                OnQRCodeError?.Invoke($"Product not found: {productId}");
            }
        }
    }
}
