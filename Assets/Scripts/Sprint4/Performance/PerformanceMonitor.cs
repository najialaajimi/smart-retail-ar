using UnityEngine;
using System.Collections;

namespace SmartRetailAR.Sprint4.Performance
{
    /// <summary>
    /// Monitors and optimizes application performance
    /// Sprint 4: Performance monitoring and optimization
    /// </summary>
    public class PerformanceMonitor : MonoBehaviour
    {
        private static PerformanceMonitor _instance;
        public static PerformanceMonitor Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("PerformanceMonitor");
                    _instance = go.AddComponent<PerformanceMonitor>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }
        
        [Header("Performance Settings")]
        [SerializeField] private bool showDebugInfo = true;
        [SerializeField] private float updateInterval = 1f;
        
        // Performance metrics
        private float _fps;
        private float _deltaTime;
        private float _frameTime;
        private int _frameCount;
        private float _fpsAccumulator;
        private float _fpsNextPeriod;
        
        // Memory metrics
        private long _totalMemory;
        private long _usedMemory;
        private long _monoMemory;
        
        // Battery metrics
        private float _batteryLevel;
        private BatteryStatus _batteryStatus;
        
        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            _fpsNextPeriod = Time.realtimeSinceStartup + updateInterval;
        }
        
        void Update()
        {
            // FPS calculation
            _frameCount++;
            _deltaTime += Time.unscaledDeltaTime;
            _fpsAccumulator += Time.timeScale / Time.unscaledDeltaTime;
            
            if (Time.realtimeSinceStartup > _fpsNextPeriod)
            {
                _fps = _fpsAccumulator / _frameCount;
                _frameTime = _deltaTime / _frameCount * 1000f;
                
                _frameCount = 0;
                _deltaTime = 0f;
                _fpsAccumulator = 0f;
                _fpsNextPeriod += updateInterval;
                
                UpdateMemoryMetrics();
                UpdateBatteryMetrics();
            }
        }
        
        void UpdateMemoryMetrics()
        {
            _totalMemory = System.GC.GetTotalMemory(false);
            _usedMemory = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();
            _monoMemory = UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong();
        }
        
        void UpdateBatteryMetrics()
        {
            _batteryLevel = SystemInfo.batteryLevel;
            _batteryStatus = SystemInfo.batteryStatus;
        }
        
        public float GetFPS()
        {
            return _fps;
        }
        
        public float GetFrameTime()
        {
            return _frameTime;
        }
        
        public long GetUsedMemory()
        {
            return _usedMemory;
        }
        
        public float GetBatteryLevel()
        {
            return _batteryLevel;
        }
        
        public BatteryStatus GetBatteryStatus()
        {
            return _batteryStatus;
        }
        
        public string GetPerformanceReport()
        {
            return $"FPS: {_fps:F1} | Frame Time: {_frameTime:F1}ms | " +
                   $"Memory: {_usedMemory / 1024 / 1024}MB | " +
                   $"Battery: {(_batteryLevel * 100):F0}%";
        }
        
        void OnGUI()
        {
            if (showDebugInfo)
            {
                int w = Screen.width, h = Screen.height;
                GUIStyle style = new GUIStyle();
                Rect rect = new Rect(10, 10, w, h * 2 / 100);
                style.alignment = TextAnchor.UpperLeft;
                style.fontSize = h * 2 / 100;
                style.normal.textColor = Color.white;
                
                string text = GetPerformanceReport();
                GUI.Label(rect, text, style);
            }
        }
        
        /// <summary>
        /// Optimize performance based on current metrics
        /// </summary>
        public void OptimizePerformance()
        {
            // Reduce quality if FPS is low
            if (_fps < 30f)
            {
                QualitySettings.DecreaseLevel();
                Debug.Log("Quality reduced to improve performance");
            }
            
            // Force garbage collection if memory is high
            if (_usedMemory > 500 * 1024 * 1024) // 500MB
            {
                System.GC.Collect();
                Resources.UnloadUnusedAssets();
                Debug.Log("Memory optimization performed");
            }
        }
        
        /// <summary>
        /// Check if device meets minimum requirements
        /// </summary>
        public bool MeetsMinimumRequirements()
        {
            bool meetsRequirements = true;
            
            // Check system memory
            if (SystemInfo.systemMemorySize < 2048) // Less than 2GB
            {
                Debug.LogWarning("Device has insufficient memory");
                meetsRequirements = false;
            }
            
            // Check AR support
            if (!UnityEngine.XR.ARSubsystems.XRSubsystemHelpers.AreAllSubsystemsLoaded())
            {
                Debug.LogWarning("AR subsystems not fully loaded");
            }
            
            return meetsRequirements;
        }
    }
}
