using UnityEngine;
using SmartRetailAR.Data;
using System;
using System.Collections.Generic;

namespace SmartRetailAR.Recognition
{
    public class ProductRecognition : MonoBehaviour
    {
        [Header("Recognition")]
        public ImageRecognition imageRecognition;
        public BarcodeScanner barcodeScanner;

        [Header("Settings")]
        public bool preferBarcodeOverImage = true;
        public float recognitionCooldown = 1f;

        public event Action<ProductData> OnProductRecognized;
        public event Action<string> OnRecognitionFailed;

        private float _lastRecognitionTime;
        private ProductData _lastRecognizedProduct;

        void Start()
        {
            if (imageRecognition == null)
                imageRecognition = GetComponent<ImageRecognition>();

            if (barcodeScanner == null)
                barcodeScanner = GetComponent<BarcodeScanner>();

            SetupRecognitionEvents();
        }

        private void SetupRecognitionEvents()
        {
            if (imageRecognition != null)
            {
                imageRecognition.OnImageRecognized += OnImageRecognized;
                imageRecognition.OnRecognitionError += OnError;
            }

            if (barcodeScanner != null)
            {
                barcodeScanner.OnBarcodeScanned += OnBarcodeScanned;
                barcodeScanner.OnScanError += OnError;
            }
        }

        void OnDestroy()
        {
            if (imageRecognition != null)
            {
                imageRecognition.OnImageRecognized -= OnImageRecognized;
                imageRecognition.OnRecognitionError -= OnError;
            }

            if (barcodeScanner != null)
            {
                barcodeScanner.OnBarcodeScanned -= OnBarcodeScanned;
                barcodeScanner.OnScanError -= OnError;
            }
        }

        private void OnImageRecognized(ImageRecognition.RecognitionResult result)
        {
            if (Time.time - _lastRecognitionTime < recognitionCooldown)
                return;

            ProductData product = ProductDatabase.Instance.GetProductById(result.objectId);
            if (product != null)
            {
                RecognizeProduct(product, "Image Recognition");
            }
            else
            {
                OnRecognitionFailed?.Invoke($"Product not found: {result.objectId}");
            }
        }

        private void OnBarcodeScanned(string barcode)
        {
            if (Time.time - _lastRecognitionTime < recognitionCooldown)
                return;

            ProductData product = ProductDatabase.Instance.GetProductByBarcode(barcode);
            if (product != null)
            {
                RecognizeProduct(product, "Barcode");
            }
            else
            {
                OnRecognitionFailed?.Invoke($"Product not found for barcode: {barcode}");
            }
        }

        private void RecognizeProduct(ProductData product, string method)
        {
            _lastRecognizedProduct = product;
            _lastRecognitionTime = Time.time;
            OnProductRecognized?.Invoke(product);
            Debug.Log($"Product recognized via {method}: {product.name}");
        }

        private void OnError(string error)
        {
            OnRecognitionFailed?.Invoke(error);
            Debug.LogWarning($"Recognition error: {error}");
        }

        public ProductData GetLastRecognizedProduct()
        {
            return _lastRecognizedProduct;
        }

        public void ClearLastRecognition()
        {
            _lastRecognizedProduct = null;
        }

        public void EnableRecognition(bool enable)
        {
            if (imageRecognition != null)
                imageRecognition.EnableRecognition(enable);

            if (barcodeScanner != null)
                barcodeScanner.enabled = enable;
        }
    }
}
