using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using SmartRetailAR.Data;
using System;
using System.Collections.Generic;

namespace SmartRetailAR.AR
{
    public class ARProductTracker : MonoBehaviour
    {
        [Header("Tracking")]
        public ARTrackedImageManager trackedImageManager;
        public float trackingQualityThreshold = 0.8f;

        [Header("Product Tracking")]
        public GameObject productOverlayPrefab;
        public float updateInterval = 0.1f;

        public event Action<ProductData, Pose> OnProductDetected;
        public event Action<ProductData> OnProductLost;

        private Dictionary<string, GameObject> _trackedProducts = new Dictionary<string, GameObject>();
        private Dictionary<TrackableId, ProductData> _imageToProduct = new Dictionary<TrackableId, ProductData>();
        private float _lastUpdateTime;

        void Start()
        {
            if (trackedImageManager == null)
                trackedImageManager = GetComponent<ARTrackedImageManager>();

            if (trackedImageManager != null)
            {
                trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
            }
        }

        void OnDestroy()
        {
            if (trackedImageManager != null)
            {
                trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
            }
        }

        private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
        {
            // Handle added images
            foreach (var trackedImage in args.added)
            {
                HandleTrackedImage(trackedImage);
            }

            // Handle updated images
            foreach (var trackedImage in args.updated)
            {
                if (trackedImage.trackingState == TrackingState.Tracking)
                {
                    UpdateTrackedImage(trackedImage);
                }
                else if (trackedImage.trackingState == TrackingState.None)
                {
                    RemoveTrackedImage(trackedImage);
                }
            }

            // Handle removed images
            foreach (var trackedImage in args.removed)
            {
                RemoveTrackedImage(trackedImage);
            }
        }

        private void HandleTrackedImage(ARTrackedImage trackedImage)
        {
            if (trackedImage.trackingState != TrackingState.Tracking)
                return;

            // Try to find product by image name
            string imageName = trackedImage.referenceImage.name;
            ProductData product = ProductDatabase.Instance.GetProductById(imageName);

            if (product != null)
            {
                _imageToProduct[trackedImage.trackableId] = product;
                CreateProductOverlay(trackedImage, product);
                OnProductDetected?.Invoke(product, trackedImage.transform.GetWorldPose());
                Debug.Log($"Product detected: {product.name}");
            }
        }

        private void UpdateTrackedImage(ARTrackedImage trackedImage)
        {
            if (_imageToProduct.ContainsKey(trackedImage.trackableId))
            {
                ProductData product = _imageToProduct[trackedImage.trackableId];
                if (_trackedProducts.ContainsKey(product.id))
                {
                    GameObject overlay = _trackedProducts[product.id];
                    overlay.transform.SetPositionAndRotation(
                        trackedImage.transform.position,
                        trackedImage.transform.rotation
                    );
                }
            }
        }

        private void RemoveTrackedImage(ARTrackedImage trackedImage)
        {
            if (_imageToProduct.ContainsKey(trackedImage.trackableId))
            {
                ProductData product = _imageToProduct[trackedImage.trackableId];
                if (_trackedProducts.ContainsKey(product.id))
                {
                    Destroy(_trackedProducts[product.id]);
                    _trackedProducts.Remove(product.id);
                }
                _imageToProduct.Remove(trackedImage.trackableId);
                OnProductLost?.Invoke(product);
                Debug.Log($"Product lost: {product.name}");
            }
        }

        private void CreateProductOverlay(ARTrackedImage trackedImage, ProductData product)
        {
            if (productOverlayPrefab != null && !_trackedProducts.ContainsKey(product.id))
            {
                GameObject overlay = Instantiate(productOverlayPrefab, trackedImage.transform);
                overlay.transform.localPosition = Vector3.zero;
                overlay.transform.localRotation = Quaternion.identity;
                _trackedProducts[product.id] = overlay;

                // Update overlay with product data
                var overlayController = overlay.GetComponent<AROverlayController>();
                if (overlayController != null)
                {
                    overlayController.SetProductData(product);
                }
            }
        }

        public bool IsProductTracked(string productId)
        {
            return _trackedProducts.ContainsKey(productId);
        }

        public List<ProductData> GetTrackedProducts()
        {
            return new List<ProductData>(_imageToProduct.Values);
        }
    }

    public static class TransformExtensions
    {
        public static Pose GetWorldPose(this Transform transform)
        {
            return new Pose(transform.position, transform.rotation);
        }
    }
}
