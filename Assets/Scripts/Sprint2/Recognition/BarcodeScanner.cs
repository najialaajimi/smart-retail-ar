using UnityEngine;
using System;
using ZXing;
using ZXing.QrCode;

namespace SmartRetailAR.Recognition
{
    public class BarcodeScanner : MonoBehaviour
    {
        [Header("Scanner Settings")]
        public float scanInterval = 0.5f;
        public BarcodeFormat[] supportedFormats = {
            BarcodeFormat.QR_CODE,
            BarcodeFormat.EAN_13,
            BarcodeFormat.EAN_8,
            BarcodeFormat.UPC_A,
            BarcodeFormat.CODE_128
        };

        [Header("Camera")]
        public bool useCameraTexture = true;

        public event Action<string> OnBarcodeScanned;
        public event Action<string> OnScanError;

        private IBarcodeReader _barcodeReader;
        private float _lastScanTime;
        private WebCamTexture _webcamTexture;
        private bool _isScanning = false;

        void Start()
        {
            InitializeScanner();
        }

        void OnDestroy()
        {
            if (_webcamTexture != null && _webcamTexture.isPlaying)
            {
                _webcamTexture.Stop();
            }
        }

        private void InitializeScanner()
        {
            _barcodeReader = new BarcodeReader
            {
                AutoRotate = true,
                TryInverted = true,
                Options = new ZXing.Common.DecodingOptions
                {
                    TryHarder = true,
                    PossibleFormats = new System.Collections.Generic.List<BarcodeFormat>(supportedFormats)
                }
            };

            if (useCameraTexture)
            {
                StartCameraScanning();
            }

            Debug.Log("Barcode scanner initialized");
        }

        private void StartCameraScanning()
        {
            if (WebCamTexture.devices.Length > 0)
            {
                _webcamTexture = new WebCamTexture();
                _webcamTexture.Play();
                _isScanning = true;
            }
            else
            {
                Debug.LogWarning("No camera available for barcode scanning");
            }
        }

        void Update()
        {
            if (_isScanning && _webcamTexture != null && _webcamTexture.isPlaying)
            {
                if (Time.time - _lastScanTime >= scanInterval)
                {
                    ScanFrame();
                }
            }
        }

        private void ScanFrame()
        {
            _lastScanTime = Time.time;

            try
            {
                var result = _barcodeReader.Decode(_webcamTexture.GetPixels32(), _webcamTexture.width, _webcamTexture.height);
                if (result != null)
                {
                    OnBarcodeScanned?.Invoke(result.Text);
                    Debug.Log($"Barcode scanned: {result.Text} (Format: {result.BarcodeFormat})");
                }
            }
            catch (Exception e)
            {
                OnScanError?.Invoke(e.Message);
            }
        }

        public void ScanTexture(Texture2D texture)
        {
            if (texture == null)
            {
                OnScanError?.Invoke("Invalid texture provided");
                return;
            }

            try
            {
                var result = _barcodeReader.Decode(texture.GetPixels32(), texture.width, texture.height);
                if (result != null)
                {
                    OnBarcodeScanned?.Invoke(result.Text);
                }
                else
                {
                    OnScanError?.Invoke("No barcode detected in image");
                }
            }
            catch (Exception e)
            {
                OnScanError?.Invoke(e.Message);
            }
        }

        public void StartScanning()
        {
            _isScanning = true;
            if (_webcamTexture != null && !_webcamTexture.isPlaying)
            {
                _webcamTexture.Play();
            }
        }

        public void StopScanning()
        {
            _isScanning = false;
            if (_webcamTexture != null && _webcamTexture.isPlaying)
            {
                _webcamTexture.Stop();
            }
        }

        public bool IsScanning()
        {
            return _isScanning;
        }
    }
}
