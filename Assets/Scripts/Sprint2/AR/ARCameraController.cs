using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace SmartRetailAR.AR
{
    [RequireComponent(typeof(Camera))]
    public class ARCameraController : MonoBehaviour
    {
        [Header("Camera Settings")]
        public float nearClipPlane = 0.1f;
        public float farClipPlane = 100f;
        public bool autoFocus = true;

        [Header("AR Camera")]
        public ARCameraManager cameraManager;
        public ARCameraBackground cameraBackground;

        private Camera _camera;
        private bool _isInitialized = false;

        void Awake()
        {
            _camera = GetComponent<Camera>();
            if (cameraManager == null)
                cameraManager = GetComponent<ARCameraManager>();
            if (cameraBackground == null)
                cameraBackground = GetComponent<ARCameraBackground>();
        }

        void Start()
        {
            InitializeCamera();
        }

        private void InitializeCamera()
        {
            if (_isInitialized) return;

            if (_camera != null)
            {
                _camera.nearClipPlane = nearClipPlane;
                _camera.farClipPlane = farClipPlane;
            }

            if (cameraManager != null)
            {
                cameraManager.autoFocusRequested = autoFocus;
                cameraManager.frameReceived += OnCameraFrameReceived;
            }

            _isInitialized = true;
            Debug.Log("AR Camera initialized");
        }

        void OnDestroy()
        {
            if (cameraManager != null)
            {
                cameraManager.frameReceived -= OnCameraFrameReceived;
            }
        }

        private void OnCameraFrameReceived(ARCameraFrameEventArgs args)
        {
            // Process camera frame for tracking and recognition
        }

        public void RequestFocus()
        {
            if (cameraManager != null && autoFocus)
            {
                cameraManager.autoFocusRequested = true;
            }
        }

        public Texture2D GetCameraImage()
        {
            if (cameraManager != null && cameraManager.TryAcquireLatestCpuImage(out var image))
            {
                try
                {
                    Texture2D texture = new Texture2D(image.width, image.height, TextureFormat.RGBA32, false);
                    image.Convert(new XRCpuImage.ConversionParams
                    {
                        inputRect = new RectInt(0, 0, image.width, image.height),
                        outputDimensions = new Vector2Int(image.width, image.height),
                        outputFormat = TextureFormat.RGBA32,
                        transformation = XRCpuImage.Transformation.None
                    }, texture.GetRawTextureData(), texture.GetRawTextureData().Length);
                    
                    texture.Apply();
                    return texture;
                }
                finally
                {
                    image.Dispose();
                }
            }
            return null;
        }

        public bool IsCameraReady()
        {
            return _isInitialized && cameraManager != null && cameraManager.subsystem != null;
        }
    }
}
