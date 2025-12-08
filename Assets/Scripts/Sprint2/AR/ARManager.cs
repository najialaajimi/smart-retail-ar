using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

namespace SmartRetailAR.AR
{
    public class ARManager : MonoBehaviour
    {
        private static ARManager _instance;
        public static ARManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("ARManager");
                    _instance = go.AddComponent<ARManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        [Header("AR Components")]
        public ARSession arSession;
        public ARSessionOrigin arSessionOrigin;
        public ARCameraManager arCameraManager;
        public ARPlaneManager arPlaneManager;
        public ARRaycastManager arRaycastManager;

        [Header("AR Settings")]
        public bool autoStartAR = true;
        public TrackingMode trackingMode = TrackingMode.PositionAndRotation;

        public event Action OnARSessionReady;
        public event Action<ARSessionState> OnARSessionStateChanged;

        private bool _isInitialized = false;
        private ARSessionState _currentState;

        public enum TrackingMode
        {
            Position,
            Rotation,
            PositionAndRotation
        }

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

        void Start()
        {
            if (autoStartAR)
            {
                InitializeAR();
            }
        }

        public void InitializeAR()
        {
            if (_isInitialized) return;

            // Find or create AR components
            if (arSession == null)
                arSession = FindObjectOfType<ARSession>();

            if (arSessionOrigin == null)
                arSessionOrigin = FindObjectOfType<ARSessionOrigin>();

            if (arCameraManager == null)
                arCameraManager = FindObjectOfType<ARCameraManager>();

            if (arPlaneManager == null)
                arPlaneManager = FindObjectOfType<ARPlaneManager>();

            if (arRaycastManager == null)
                arRaycastManager = FindObjectOfType<ARRaycastManager>();

            _isInitialized = true;
            OnARSessionReady?.Invoke();
            Debug.Log("AR Manager initialized");
        }

        void Update()
        {
            if (arSession != null)
            {
                ARSessionState newState = ARSession.state;
                if (newState != _currentState)
                {
                    _currentState = newState;
                    OnARSessionStateChanged?.Invoke(_currentState);
                    Debug.Log($"AR Session State: {_currentState}");
                }
            }
        }

        public bool IsARAvailable()
        {
            return ARSession.state == ARSessionState.SessionTracking ||
                   ARSession.state == ARSessionState.SessionInitializing;
        }

        public bool IsTrackingActive()
        {
            return ARSession.state == ARSessionState.SessionTracking;
        }

        public void EnablePlaneDetection(bool enable)
        {
            if (arPlaneManager != null)
            {
                arPlaneManager.enabled = enable;
                Debug.Log($"Plane detection: {(enable ? "enabled" : "disabled")}");
            }
        }

        public void SetPlaneDetectionMode(PlaneDetectionMode mode)
        {
            if (arPlaneManager != null)
            {
                arPlaneManager.requestedDetectionMode = mode;
                Debug.Log($"Plane detection mode set to: {mode}");
            }
        }

        public ARSessionState GetSessionState()
        {
            return _currentState;
        }
    }
}
