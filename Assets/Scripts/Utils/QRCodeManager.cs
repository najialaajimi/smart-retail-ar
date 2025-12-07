using UnityEngine;

namespace SmartRetailAR.Utils
{
    /// <summary>
    /// Singleton manager for QR code scanning
    /// Handles QR code detection and decoding
    /// </summary>
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
        
        // Current scanned product ID
        private string _currentProductId;
        
        // Delegates for QR code events
        public delegate void QRCodeScannedDelegate(string productId);
        public event QRCodeScannedDelegate OnQRCodeScanned;
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        /// <summary>
        /// Process scanned QR code data
        /// </summary>
        public void ProcessQRCode(string qrData)
        {
            if (string.IsNullOrEmpty(qrData))
            {
                Debug.LogWarning("QR code data is empty");
                return;
            }
            
            // Extract product ID from QR code
            // Assuming QR code format: "smartretail://product/{productId}"
            string productId = ExtractProductId(qrData);
            
            if (!string.IsNullOrEmpty(productId))
            {
                _currentProductId = productId;
                Debug.Log($"QR Code scanned: Product ID = {productId}");
                
                // Trigger event
                OnQRCodeScanned?.Invoke(productId);
            }
            else
            {
                Debug.LogWarning($"Could not extract product ID from QR data: {qrData}");
            }
        }
        
        /// <summary>
        /// Extract product ID from QR code data
        /// </summary>
        private string ExtractProductId(string qrData)
        {
            // If QR data is already a product ID (for testing)
            if (!qrData.Contains("://") && !qrData.Contains("/"))
            {
                return qrData;
            }
            
            // Parse URL format: "smartretail://product/{productId}"
            if (qrData.StartsWith("smartretail://product/"))
            {
                return qrData.Replace("smartretail://product/", "");
            }
            
            // Parse HTTP URL format
            if (qrData.Contains("/product/"))
            {
                int startIndex = qrData.IndexOf("/product/") + 9;
                int endIndex = qrData.IndexOf("?", startIndex);
                
                if (endIndex == -1)
                    endIndex = qrData.Length;
                
                return qrData.Substring(startIndex, endIndex - startIndex);
            }
            
            // If no format matches, return as-is
            return qrData;
        }
        
        /// <summary>
        /// Get current scanned product ID
        /// </summary>
        public string GetCurrentProductId()
        {
            return _currentProductId;
        }
        
        /// <summary>
        /// Set current product ID (for testing)
        /// </summary>
        public void SetCurrentProductId(string productId)
        {
            _currentProductId = productId;
            OnQRCodeScanned?.Invoke(productId);
        }
        
        /// <summary>
        /// Clear current product
        /// </summary>
        public void ClearCurrentProduct()
        {
            _currentProductId = null;
        }
        
        /// <summary>
        /// Validate QR code format
        /// </summary>
        public bool IsValidQRCode(string qrData)
        {
            if (string.IsNullOrEmpty(qrData))
                return false;
            
            string productId = ExtractProductId(qrData);
            return !string.IsNullOrEmpty(productId);
        }
    }
}
