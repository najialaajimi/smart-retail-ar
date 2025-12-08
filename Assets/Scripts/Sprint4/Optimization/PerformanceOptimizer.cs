using UnityEngine;
using System.Collections.Generic;
using System;

namespace SmartRetailAR.Optimization
{
    public class PerformanceOptimizer : MonoBehaviour
    {
        private static PerformanceOptimizer _instance;
        public static PerformanceOptimizer Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("PerformanceOptimizer");
                    _instance = go.AddComponent<PerformanceOptimizer>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        [Header("Performance Settings")]
        public bool autoOptimize = true;
        public int targetFrameRate = 60;
        public QualityLevel currentQuality = QualityLevel.High;

        [Header("Monitoring")]
        public bool enableMonitoring = true;
        public float monitoringInterval = 1f;

        public enum QualityLevel
        {
            Low,
            Medium,
            High,
            Ultra
        }

        private float _lastFrameTime;
        private int _frameCount;
        private float _fps;
        private float _lastMonitorTime;
        private Queue<float> _fpsHistory = new Queue<float>();
        private const int FPS_HISTORY_SIZE = 60;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeOptimizer();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            ApplyQualitySettings(currentQuality);
        }

        void Update()
        {
            UpdatePerformanceMetrics();

            if (enableMonitoring && Time.time - _lastMonitorTime >= monitoringInterval)
            {
                MonitorPerformance();
                _lastMonitorTime = Time.time;
            }
        }

        private void InitializeOptimizer()
        {
            Application.targetFrameRate = targetFrameRate;
            QualitySettings.vSyncCount = 0; // Disable VSync for better control
            
            Debug.Log("Performance Optimizer initialized");
        }

        private void UpdatePerformanceMetrics()
        {
            _frameCount++;
            float deltaTime = Time.deltaTime;
            
            if (deltaTime > 0)
            {
                _fps = 1.0f / deltaTime;
                _fpsHistory.Enqueue(_fps);
                
                if (_fpsHistory.Count > FPS_HISTORY_SIZE)
                    _fpsHistory.Dequeue();
            }
        }

        private void MonitorPerformance()
        {
            float avgFps = CalculateAverageFPS();
            
            if (autoOptimize)
            {
                if (avgFps < targetFrameRate * 0.8f && currentQuality > QualityLevel.Low)
                {
                    // Reduce quality
                    currentQuality = (QualityLevel)((int)currentQuality - 1);
                    ApplyQualitySettings(currentQuality);
                    Debug.Log($"Performance degraded, reducing quality to {currentQuality}");
                }
                else if (avgFps > targetFrameRate * 0.95f && currentQuality < QualityLevel.Ultra)
                {
                    // Can increase quality
                    currentQuality = (QualityLevel)((int)currentQuality + 1);
                    ApplyQualitySettings(currentQuality);
                    Debug.Log($"Performance good, increasing quality to {currentQuality}");
                }
            }
        }

        public void ApplyQualitySettings(QualityLevel level)
        {
            currentQuality = level;

            switch (level)
            {
                case QualityLevel.Low:
                    QualitySettings.SetQualityLevel(0, true);
                    QualitySettings.shadowDistance = 20f;
                    QualitySettings.shadows = ShadowQuality.Disable;
                    QualitySettings.antiAliasing = 0;
                    break;

                case QualityLevel.Medium:
                    QualitySettings.SetQualityLevel(1, true);
                    QualitySettings.shadowDistance = 50f;
                    QualitySettings.shadows = ShadowQuality.HardOnly;
                    QualitySettings.antiAliasing = 2;
                    break;

                case QualityLevel.High:
                    QualitySettings.SetQualityLevel(2, true);
                    QualitySettings.shadowDistance = 100f;
                    QualitySettings.shadows = ShadowQuality.All;
                    QualitySettings.antiAliasing = 4;
                    break;

                case QualityLevel.Ultra:
                    QualitySettings.SetQualityLevel(3, true);
                    QualitySettings.shadowDistance = 150f;
                    QualitySettings.shadows = ShadowQuality.All;
                    QualitySettings.antiAliasing = 8;
                    break;
            }

            Debug.Log($"Quality settings applied: {level}");
        }

        public float GetCurrentFPS()
        {
            return _fps;
        }

        public float CalculateAverageFPS()
        {
            if (_fpsHistory.Count == 0) return 0f;
            
            float sum = 0f;
            foreach (var fps in _fpsHistory)
                sum += fps;
            
            return sum / _fpsHistory.Count;
        }

        public PerformanceMetrics GetMetrics()
        {
            return new PerformanceMetrics
            {
                currentFPS = _fps,
                averageFPS = CalculateAverageFPS(),
                frameCount = _frameCount,
                memoryUsed = System.GC.GetTotalMemory(false) / (1024f * 1024f),
                qualityLevel = currentQuality.ToString()
            };
        }

        public void OptimizeMemory()
        {
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
            Debug.Log("Memory optimization performed");
        }

        public void EnableLowPowerMode()
        {
            ApplyQualitySettings(QualityLevel.Low);
            targetFrameRate = 30;
            Application.targetFrameRate = targetFrameRate;
            Debug.Log("Low power mode enabled");
        }

        public void DisableLowPowerMode()
        {
            ApplyQualitySettings(QualityLevel.High);
            targetFrameRate = 60;
            Application.targetFrameRate = targetFrameRate;
            Debug.Log("Low power mode disabled");
        }

        [Serializable]
        public class PerformanceMetrics
        {
            public float currentFPS;
            public float averageFPS;
            public int frameCount;
            public float memoryUsed;
            public string qualityLevel;
        }
    }
}
