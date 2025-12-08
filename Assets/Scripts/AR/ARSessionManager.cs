using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

namespace SmartRetailAR.AR
{
    /// <summary>
    /// AR Session Manager - Manages AR session lifecycle and configuration
    /// </summary>
    [RequireComponent(typeof(ARSession))]
    public class ARSessionManager : MonoBehaviour
    {
        [Header("AR Components")]
        [SerializeField] private ARSession arSession;
        [SerializeField] private ARSessionOrigin arSessionOrigin;
        [SerializeField] private ARCameraManager arCameraManager;

        [Header("Session Configuration")]
        [SerializeField] private bool autoStartSession = true;
        [SerializeField] private bool resetSessionOnLoad = false;

        [Header("Events")]
        public event Action OnSessionInitialized;
        public event Action OnSessionStarted;
        public event Action OnSessionStopped;
        public event Action<ARSessionStateChangedEventArgs> OnSessionStateChanged;

        private bool isInitialized = false;
        private bool isSessionActive = false;

        private void Awake()
        {
            if (arSession == null)
            {
                arSession = GetComponent<ARSession>();
            }

            if (arSessionOrigin == null)
            {
                arSessionOrigin = FindObjectOfType<ARSessionOrigin>();
            }

            if (arCameraManager == null)
            {
                arCameraManager = FindObjectOfType<ARCameraManager>();
            }
        }

        private void Start()
        {
            InitializeSession();
            
            if (autoStartSession)
            {
                StartSession();
            }
        }

        private void OnEnable()
        {
            if (arSession != null)
            {
                ARSession.stateChanged += OnARSessionStateChanged;
            }
        }

        private void OnDisable()
        {
            if (arSession != null)
            {
                ARSession.stateChanged -= OnARSessionStateChanged;
            }
        }

        /// <summary>
        /// Initialize AR session
        /// </summary>
        private void InitializeSession()
        {
            if (isInitialized) return;

            // Check AR availability
            CheckARAvailability();

            if (resetSessionOnLoad)
            {
                ResetSession();
            }

            isInitialized = true;
            OnSessionInitialized?.Invoke();
            Debug.Log("AR Session initialized");
        }

        /// <summary>
        /// Start AR session
        /// </summary>
        public void StartSession()
        {
            if (!isInitialized)
            {
                InitializeSession();
            }

            if (arSession != null && !isSessionActive)
            {
                arSession.enabled = true;
                isSessionActive = true;
                OnSessionStarted?.Invoke();
                Debug.Log("AR Session started");
            }
        }

        /// <summary>
        /// Stop AR session
        /// </summary>
        public void StopSession()
        {
            if (arSession != null && isSessionActive)
            {
                arSession.enabled = false;
                isSessionActive = false;
                OnSessionStopped?.Invoke();
                Debug.Log("AR Session stopped");
            }
        }

        /// <summary>
        /// Reset AR session
        /// </summary>
        public void ResetSession()
        {
            if (arSession != null)
            {
                arSession.Reset();
                Debug.Log("AR Session reset");
            }
        }

        /// <summary>
        /// Pause AR session
        /// </summary>
        public void PauseSession()
        {
            if (arSession != null && isSessionActive)
            {
                arSession.enabled = false;
                Debug.Log("AR Session paused");
            }
        }

        /// <summary>
        /// Resume AR session
        /// </summary>
        public void ResumeSession()
        {
            if (arSession != null && isInitialized)
            {
                arSession.enabled = true;
                isSessionActive = true;
                Debug.Log("AR Session resumed");
            }
        }

        /// <summary>
        /// Check AR availability on device
        /// </summary>
        private void CheckARAvailability()
        {
            // Check if AR is supported
            if (ARSession.state == ARSessionState.None || 
                ARSession.state == ARSessionState.Unsupported)
            {
                Debug.LogWarning("AR is not supported on this device");
            }
            else if (ARSession.state == ARSessionState.CheckingAvailability)
            {
                Debug.Log("Checking AR availability...");
            }
        }

        /// <summary>
        /// Handle AR session state changes
        /// </summary>
        private void OnARSessionStateChanged(ARSessionStateChangedEventArgs args)
        {
            Debug.Log($"AR Session State Changed: {args.state}");
            
            switch (args.state)
            {
                case ARSessionState.None:
                case ARSessionState.Unsupported:
                    Debug.LogError("AR is not supported on this device");
                    break;
                    
                case ARSessionState.CheckingAvailability:
                    Debug.Log("Checking AR availability...");
                    break;
                    
                case ARSessionState.NeedsInstall:
                    Debug.LogWarning("AR software needs to be installed");
                    break;
                    
                case ARSessionState.Installing:
                    Debug.Log("Installing AR software...");
                    break;
                    
                case ARSessionState.Ready:
                    Debug.Log("AR Session is ready");
                    break;
                    
                case ARSessionState.SessionInitializing:
                    Debug.Log("AR Session is initializing...");
                    break;
                    
                case ARSessionState.SessionTracking:
                    Debug.Log("AR Session is tracking");
                    break;
            }

            OnSessionStateChanged?.Invoke(args);
        }

        /// <summary>
        /// Get current session state
        /// </summary>
        public ARSessionState GetSessionState()
        {
            return ARSession.state;
        }

        /// <summary>
        /// Check if session is active
        /// </summary>
        public bool IsSessionActive()
        {
            return isSessionActive && arSession != null && arSession.enabled;
        }

        /// <summary>
        /// Check if AR is supported
        /// </summary>
        public bool IsARSupported()
        {
            return ARSession.state != ARSessionState.None && 
                   ARSession.state != ARSessionState.Unsupported;
        }

        /// <summary>
        /// Get AR camera
        /// </summary>
        public Camera GetARCamera()
        {
            if (arSessionOrigin != null && arSessionOrigin.camera != null)
            {
                return arSessionOrigin.camera;
            }
            return Camera.main;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                PauseSession();
            }
            else
            {
                ResumeSession();
            }
        }
    }
}
