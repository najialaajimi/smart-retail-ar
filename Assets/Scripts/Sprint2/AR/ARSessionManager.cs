using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using SmartRetailAR.Data;

namespace SmartRetailAR.Sprint2.AR
{
    /// <summary>
    /// Manages AR session and coordinates AR features
    /// Sprint 2: AR Foundation integration
    /// </summary>
    public class ARSessionManager : MonoBehaviour
    {
        [Header("AR Components")]
        [SerializeField] private ARSession arSession;
        [SerializeField] private ARSessionOrigin arSessionOrigin;
        [SerializeField] private ARCameraManager arCameraManager;
        [SerializeField] private ARPlaneManager arPlaneManager;
        [SerializeField] private ARRaycastManager arRaycastManager;
        [SerializeField] private ARTrackedImageManager arTrackedImageManager;
        
        [Header("Settings")]
        [SerializeField] private bool enablePlaneDetection = true;
        [SerializeField] private bool enableImageTracking = true;
        
        private static ARSessionManager _instance;
        public static ARSessionManager Instance => _instance;
        
        // Events
        public System.Action OnARSessionStarted;
        public System.Action OnARSessionFailed;
        public System.Action<ARTrackedImage> OnImageTracked;
        
        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }
        
        void Start()
        {
            InitializeAR();
        }
        
        void InitializeAR()
        {
            // Check AR support
            if (ARSession.state == ARSessionState.None || 
                ARSession.state == ARSessionState.CheckingAvailability)
            {
                StartCoroutine(CheckARSupport());
            }
            else if (ARSession.state == ARSessionState.Ready ||
                     ARSession.state == ARSessionState.SessionInitializing ||
                     ARSession.state == ARSessionState.SessionTracking)
            {
                StartARSession();
            }
        }
        
        System.Collections.IEnumerator CheckARSupport()
        {
            yield return ARSession.CheckAvailability();
            
            if (ARSession.state == ARSessionState.Unsupported)
            {
                Debug.LogError("AR is not supported on this device");
                OnARSessionFailed?.Invoke();
            }
            else
            {
                StartARSession();
            }
        }
        
        void StartARSession()
        {
            // Configure plane detection
            if (arPlaneManager != null)
            {
                arPlaneManager.enabled = enablePlaneDetection;
            }
            
            // Configure image tracking
            if (arTrackedImageManager != null)
            {
                arTrackedImageManager.enabled = enableImageTracking;
                arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
            }
            
            Debug.Log("AR Session started");
            OnARSessionStarted?.Invoke();
        }
        
        void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
        {
            foreach (var trackedImage in args.added)
            {
                OnImageTracked?.Invoke(trackedImage);
                Debug.Log($"Image tracked: {trackedImage.referenceImage.name}");
            }
            
            foreach (var trackedImage in args.updated)
            {
                if (trackedImage.trackingState == TrackingState.Tracking)
                {
                    OnImageTracked?.Invoke(trackedImage);
                }
            }
        }
        
        public bool TryGetTouchPosition(out Vector2 touchPosition)
        {
            if (Input.touchCount > 0)
            {
                touchPosition = Input.GetTouch(0).position;
                return true;
            }
            
            touchPosition = Vector2.zero;
            return false;
        }
        
        public bool Raycast(Vector2 screenPosition, out ARRaycastHit hit, 
            TrackableType trackableTypes = TrackableType.PlaneWithinPolygon)
        {
            if (arRaycastManager != null)
            {
                var hits = new System.Collections.Generic.List<ARRaycastHit>();
                if (arRaycastManager.Raycast(screenPosition, hits, trackableTypes))
                {
                    hit = hits[0];
                    return true;
                }
            }
            
            hit = default;
            return false;
        }
        
        public void EnablePlaneDetection(bool enable)
        {
            if (arPlaneManager != null)
            {
                arPlaneManager.enabled = enable;
            }
        }
        
        public void EnableImageTracking(bool enable)
        {
            if (arTrackedImageManager != null)
            {
                arTrackedImageManager.enabled = enable;
            }
        }
        
        public Camera GetARCamera()
        {
            if (arSessionOrigin != null)
            {
                return arSessionOrigin.camera;
            }
            return Camera.main;
        }
        
        void OnDestroy()
        {
            if (arTrackedImageManager != null)
            {
                arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
            }
        }
    }
}
