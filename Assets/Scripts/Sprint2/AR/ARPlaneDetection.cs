using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;
using System.Collections.Generic;

namespace SmartRetailAR.AR
{
    public class ARPlaneDetection : MonoBehaviour
    {
        [Header("Plane Detection")]
        public ARPlaneManager planeManager;
        public bool visualizePlanes = true;
        public PlaneDetectionMode detectionMode = PlaneDetectionMode.Horizontal;

        [Header("Plane Settings")]
        public Material planeMaterial;
        public float minPlaneSize = 0.1f;

        public event Action<ARPlane> OnPlaneAdded;
        public event Action<ARPlane> OnPlaneUpdated;
        public event Action<ARPlane> OnPlaneRemoved;

        private Dictionary<TrackableId, ARPlane> _detectedPlanes = new Dictionary<TrackableId, ARPlane>();

        void Start()
        {
            if (planeManager == null)
                planeManager = GetComponent<ARPlaneManager>();

            if (planeManager != null)
            {
                planeManager.requestedDetectionMode = detectionMode;
                planeManager.planesChanged += OnPlanesChanged;
            }
        }

        void OnDestroy()
        {
            if (planeManager != null)
            {
                planeManager.planesChanged -= OnPlanesChanged;
            }
        }

        private void OnPlanesChanged(ARPlanesChangedEventArgs args)
        {
            // Process added planes
            foreach (var plane in args.added)
            {
                if (plane.size.x * plane.size.y >= minPlaneSize)
                {
                    _detectedPlanes[plane.trackableId] = plane;
                    OnPlaneAdded?.Invoke(plane);
                    Debug.Log($"Plane added: {plane.trackableId}, Size: {plane.size}");
                }
            }

            // Process updated planes
            foreach (var plane in args.updated)
            {
                if (_detectedPlanes.ContainsKey(plane.trackableId))
                {
                    OnPlaneUpdated?.Invoke(plane);
                }
            }

            // Process removed planes
            foreach (var plane in args.removed)
            {
                if (_detectedPlanes.ContainsKey(plane.trackableId))
                {
                    _detectedPlanes.Remove(plane.trackableId);
                    OnPlaneRemoved?.Invoke(plane);
                    Debug.Log($"Plane removed: {plane.trackableId}");
                }
            }
        }

        public void EnablePlaneDetection(bool enable)
        {
            if (planeManager != null)
            {
                planeManager.enabled = enable;
            }
        }

        public void SetDetectionMode(PlaneDetectionMode mode)
        {
            detectionMode = mode;
            if (planeManager != null)
            {
                planeManager.requestedDetectionMode = mode;
            }
        }

        public void TogglePlaneVisualization(bool show)
        {
            visualizePlanes = show;
            foreach (var plane in planeManager.trackables)
            {
                plane.gameObject.SetActive(show);
            }
        }

        public List<ARPlane> GetAllPlanes()
        {
            return new List<ARPlane>(_detectedPlanes.Values);
        }

        public ARPlane GetLargestPlane()
        {
            ARPlane largest = null;
            float maxArea = 0f;

            foreach (var plane in _detectedPlanes.Values)
            {
                float area = plane.size.x * plane.size.y;
                if (area > maxArea)
                {
                    maxArea = area;
                    largest = plane;
                }
            }

            return largest;
        }
    }
}
