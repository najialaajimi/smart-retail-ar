using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;
using System.Collections.Generic;

namespace SmartRetailAR.AR
{
    /// <summary>
    /// AR Image Tracker - Handles real-time image recognition and tracking
    /// Uses AR Foundation for cross-platform AR support
    /// </summary>
    [RequireComponent(typeof(ARTrackedImageManager))]
    public class ARImageTracker : MonoBehaviour
    {
        [Header("AR Components")]
        [SerializeField] private ARTrackedImageManager trackedImageManager;
        [SerializeField] private XRReferenceImageLibrary referenceImageLibrary;

        [Header("Events")]
        public event Action<string> OnProductDetected;
        public event Action<string> OnProductLost;

        [Header("Settings")]
        [SerializeField] private float detectionConfidenceThreshold = 0.7f;
        [SerializeField] private bool enableTracking = true;

        private Dictionary<string, ARTrackedImage> trackedImages = new Dictionary<string, ARTrackedImage>();
        private Dictionary<string, float> lastDetectionTime = new Dictionary<string, float>();
        private const float DETECTION_COOLDOWN = 1.0f;

        private void Awake()
        {
            if (trackedImageManager == null)
            {
                trackedImageManager = GetComponent<ARTrackedImageManager>();
            }
        }

        private void OnEnable()
        {
            if (trackedImageManager != null)
            {
                trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
            }
        }

        private void OnDisable()
        {
            if (trackedImageManager != null)
            {
                trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
            }
        }

        /// <summary>
        /// Called when tracked images are added, updated, or removed
        /// </summary>
        private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            if (!enableTracking) return;

            // Handle newly detected images
            foreach (var trackedImage in eventArgs.added)
            {
                HandleTrackedImage(trackedImage, true);
            }

            // Handle updated images
            foreach (var trackedImage in eventArgs.updated)
            {
                HandleTrackedImage(trackedImage, false);
            }

            // Handle removed/lost images
            foreach (var trackedImage in eventArgs.removed)
            {
                HandleImageLost(trackedImage);
            }
        }

        /// <summary>
        /// Process tracked image detection
        /// </summary>
        private void HandleTrackedImage(ARTrackedImage trackedImage, bool isNew)
        {
            // Check tracking state
            if (trackedImage.trackingState != TrackingState.Tracking)
            {
                return;
            }

            string imageName = trackedImage.referenceImage.name;
            
            // Apply detection cooldown
            if (lastDetectionTime.ContainsKey(imageName))
            {
                if (Time.time - lastDetectionTime[imageName] < DETECTION_COOLDOWN)
                {
                    return;
                }
            }

            // Store tracked image
            trackedImages[imageName] = trackedImage;
            lastDetectionTime[imageName] = Time.time;

            // Extract product ID from image name (assuming format: "ProductImage_PROD001")
            string productId = ExtractProductId(imageName);
            
            if (!string.IsNullOrEmpty(productId))
            {
                Debug.Log($"AR Image Detected: {imageName} -> Product ID: {productId}");
                OnProductDetected?.Invoke(productId);
            }
        }

        /// <summary>
        /// Handle image lost/removed
        /// </summary>
        private void HandleImageLost(ARTrackedImage trackedImage)
        {
            string imageName = trackedImage.referenceImage.name;
            
            if (trackedImages.ContainsKey(imageName))
            {
                trackedImages.Remove(imageName);
                string productId = ExtractProductId(imageName);
                
                if (!string.IsNullOrEmpty(productId))
                {
                    Debug.Log($"AR Image Lost: {imageName} -> Product ID: {productId}");
                    OnProductLost?.Invoke(productId);
                }
            }
        }

        /// <summary>
        /// Extract product ID from reference image name
        /// Expected format: "ProductImage_PROD001" or just "PROD001"
        /// </summary>
        private string ExtractProductId(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
                return null;

            // Check if it contains underscore separator
            if (imageName.Contains("_"))
            {
                string[] parts = imageName.Split(new char[] { '_' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 0)
                {
                    return parts[parts.Length - 1]; // Return last part
                }
            }

            // If no underscore, assume the entire name is the product ID
            return imageName.StartsWith("PROD") ? imageName : null;
        }

        /// <summary>
        /// Get tracked image by name
        /// </summary>
        public ARTrackedImage GetTrackedImage(string imageName)
        {
            return trackedImages.ContainsKey(imageName) ? trackedImages[imageName] : null;
        }

        /// <summary>
        /// Get all currently tracked images
        /// </summary>
        public List<ARTrackedImage> GetAllTrackedImages()
        {
            return new List<ARTrackedImage>(trackedImages.Values);
        }

        /// <summary>
        /// Enable/disable tracking
        /// </summary>
        public void SetTrackingEnabled(bool enabled)
        {
            enableTracking = enabled;
            if (trackedImageManager != null)
            {
                trackedImageManager.enabled = enabled;
            }
        }

        /// <summary>
        /// Get tracking state
        /// </summary>
        public bool IsTrackingEnabled()
        {
            return enableTracking;
        }

        /// <summary>
        /// Clear all tracked images
        /// </summary>
        public void ClearTrackedImages()
        {
            trackedImages.Clear();
            lastDetectionTime.Clear();
        }
    }
}
