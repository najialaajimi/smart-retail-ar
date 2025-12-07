using UnityEngine;
using System.Collections.Generic;

namespace SmartRetailAR.Testing
{
    /// <summary>
    /// Performance Monitor - Tracks FPS, memory, and other performance metrics
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

        [Header("Monitoring Settings")]
        [SerializeField] private bool enableMonitoring = true;
        [SerializeField] private float updateInterval = 1.0f;
        [SerializeField] private bool showDebugOverlay = false;

        [Header("Performance Thresholds")]
        [SerializeField] private float targetFPS = 30f;
        [SerializeField] private float maxMemoryMB = 1024f;
        [SerializeField] private float maxLatencyMs = 1000f;

        // Performance metrics
        private float currentFPS = 0f;
        private float averageFPS = 0f;
        private float minFPS = 999f;
        private float maxFPS = 0f;
        private long currentMemoryMB = 0;
        private long peakMemoryMB = 0;
        
        private float deltaTime = 0f;
        private float updateTimer = 0f;
        private int frameCount = 0;
        private List<float> fpsHistory = new List<float>();
        private const int FPS_HISTORY_SIZE = 60;

        private void Awake()
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

        private void Update()
        {
            if (!enableMonitoring) return;

            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            frameCount++;
            updateTimer += Time.unscaledDeltaTime;

            if (updateTimer >= updateInterval)
            {
                UpdateMetrics();
                updateTimer = 0f;
            }
        }

        private void UpdateMetrics()
        {
            // Calculate FPS
            currentFPS = 1.0f / deltaTime;
            
            // Update FPS history
            fpsHistory.Add(currentFPS);
            if (fpsHistory.Count > FPS_HISTORY_SIZE)
            {
                fpsHistory.RemoveAt(0);
            }

            // Calculate average FPS
            float sum = 0f;
            foreach (float fps in fpsHistory)
            {
                sum += fps;
            }
            averageFPS = sum / fpsHistory.Count;

            // Update min/max FPS
            if (currentFPS < minFPS) minFPS = currentFPS;
            if (currentFPS > maxFPS) maxFPS = currentFPS;

            // Update memory usage
            currentMemoryMB = System.GC.GetTotalMemory(false) / (1024 * 1024);
            if (currentMemoryMB > peakMemoryMB)
            {
                peakMemoryMB = currentMemoryMB;
            }

            // Track with analytics
            AnalyticsManager.Instance.TrackARPerformance(currentFPS, deltaTime * 1000f);

            // Check performance thresholds
            CheckPerformanceThresholds();

            frameCount = 0;
        }

        private void CheckPerformanceThresholds()
        {
            // Check FPS threshold
            if (currentFPS < targetFPS)
            {
                Debug.LogWarning($"Performance Warning: FPS below target ({currentFPS:F1} < {targetFPS})");
            }

            // Check memory threshold
            if (currentMemoryMB > maxMemoryMB)
            {
                Debug.LogWarning($"Memory Warning: Usage above threshold ({currentMemoryMB}MB > {maxMemoryMB}MB)");
            }

            // Check latency
            float latencyMs = deltaTime * 1000f;
            if (latencyMs > maxLatencyMs)
            {
                Debug.LogWarning($"Latency Warning: Frame time exceeds threshold ({latencyMs:F1}ms > {maxLatencyMs}ms)");
            }
        }

        /// <summary>
        /// Get current FPS
        /// </summary>
        public float GetCurrentFPS()
        {
            return currentFPS;
        }

        /// <summary>
        /// Get average FPS
        /// </summary>
        public float GetAverageFPS()
        {
            return averageFPS;
        }

        /// <summary>
        /// Get current memory usage in MB
        /// </summary>
        public long GetCurrentMemoryMB()
        {
            return currentMemoryMB;
        }

        /// <summary>
        /// Get performance summary
        /// </summary>
        public PerformanceSummary GetPerformanceSummary()
        {
            return new PerformanceSummary
            {
                currentFPS = currentFPS,
                averageFPS = averageFPS,
                minFPS = minFPS,
                maxFPS = maxFPS,
                currentMemoryMB = currentMemoryMB,
                peakMemoryMB = peakMemoryMB,
                frameTimeMs = deltaTime * 1000f,
                meetsTargetFPS = currentFPS >= targetFPS,
                memoryWithinLimit = currentMemoryMB <= maxMemoryMB
            };
        }

        /// <summary>
        /// Reset performance statistics
        /// </summary>
        public void ResetStatistics()
        {
            minFPS = 999f;
            maxFPS = 0f;
            peakMemoryMB = 0;
            fpsHistory.Clear();
            Debug.Log("Performance statistics reset");
        }

        /// <summary>
        /// Enable/disable monitoring
        /// </summary>
        public void SetMonitoringEnabled(bool enabled)
        {
            enableMonitoring = enabled;
        }

        /// <summary>
        /// Force garbage collection
        /// </summary>
        public void ForceGarbageCollection()
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            Debug.Log("Forced garbage collection");
        }

        private void OnGUI()
        {
            if (!showDebugOverlay || !enableMonitoring) return;

            int w = Screen.width, h = Screen.height;
            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(10, 10, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 50;
            style.normal.textColor = Color.white;

            float msec = deltaTime * 1000.0f;
            string text = string.Format("FPS: {0:0.} ({1:0.0} ms)\nMemory: {2} MB", 
                currentFPS, msec, currentMemoryMB);
            
            GUI.Label(rect, text, style);
        }
    }

    [System.Serializable]
    public class PerformanceSummary
    {
        public float currentFPS;
        public float averageFPS;
        public float minFPS;
        public float maxFPS;
        public long currentMemoryMB;
        public long peakMemoryMB;
        public float frameTimeMs;
        public bool meetsTargetFPS;
        public bool memoryWithinLimit;

        public override string ToString()
        {
            return $"FPS: {currentFPS:F1} (Avg: {averageFPS:F1}, Min: {minFPS:F1}, Max: {maxFPS:F1})\n" +
                   $"Frame Time: {frameTimeMs:F2}ms\n" +
                   $"Memory: {currentMemoryMB}MB (Peak: {peakMemoryMB}MB)\n" +
                   $"Target FPS Met: {meetsTargetFPS}\n" +
                   $"Memory Within Limit: {memoryWithinLimit}";
        }
    }
}
