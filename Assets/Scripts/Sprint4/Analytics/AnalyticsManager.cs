using UnityEngine;
using System;
using System.Collections.Generic;

namespace SmartRetailAR.Sprint4.Analytics
{
    /// <summary>
    /// Singleton manager for tracking analytics and user behavior
    /// Sprint 4: Analytics and Performance monitoring
    /// </summary>
    public class AnalyticsManager : MonoBehaviour
    {
        private static AnalyticsManager _instance;
        public static AnalyticsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("AnalyticsManager");
                    _instance = go.AddComponent<AnalyticsManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }
        
        [Serializable]
        public class AnalyticsData
        {
            public int totalScans;
            public int successfulScans;
            public int failedScans;
            public float averageScanTime;
            public int totalProductViews;
            public int totalAlternativeViews;
            public int totalARViews;
            public Dictionary<string, int> productScanCounts = new Dictionary<string, int>();
            public List<float> scanLatencies = new List<float>();
            public int userSatisfactionRating;
            public DateTime sessionStartTime;
        }
        
        private AnalyticsData _data = new AnalyticsData();
        private float _currentScanStartTime;
        
        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            _data.sessionStartTime = DateTime.Now;
        }
        
        public void StartScanTimer()
        {
            _currentScanStartTime = Time.time;
        }
        
        public void TrackProductScan(string productId, bool success)
        {
            _data.totalScans++;
            
            if (success)
            {
                _data.successfulScans++;
                
                // Track scan time
                float scanTime = Time.time - _currentScanStartTime;
                _data.scanLatencies.Add(scanTime);
                UpdateAverageScanTime();
                
                // Track product scan count
                if (!_data.productScanCounts.ContainsKey(productId))
                {
                    _data.productScanCounts[productId] = 0;
                }
                _data.productScanCounts[productId]++;
                
                Debug.Log($"Product scan tracked: {productId}, Time: {scanTime:F2}s");
            }
            else
            {
                _data.failedScans++;
            }
            
            LogKPIs();
        }
        
        public void TrackProductView(string productId)
        {
            _data.totalProductViews++;
            Debug.Log($"Product view tracked: {productId}");
        }
        
        public void TrackAlternativeView(string productId)
        {
            _data.totalAlternativeViews++;
            Debug.Log($"Alternative view tracked: {productId}");
        }
        
        public void TrackARView(string productId)
        {
            _data.totalARViews++;
            Debug.Log($"AR view tracked: {productId}");
        }
        
        public void SetUserSatisfaction(int rating)
        {
            _data.userSatisfactionRating = Mathf.Clamp(rating, 0, 100);
            Debug.Log($"User satisfaction: {_data.userSatisfactionRating}%");
        }
        
        private void UpdateAverageScanTime()
        {
            if (_data.scanLatencies.Count > 0)
            {
                float sum = 0f;
                foreach (float time in _data.scanLatencies)
                {
                    sum += time;
                }
                _data.averageScanTime = sum / _data.scanLatencies.Count;
            }
        }
        
        public float GetRecognitionRate()
        {
            if (_data.totalScans == 0) return 0f;
            return (_data.successfulScans / (float)_data.totalScans) * 100f;
        }
        
        public float GetAverageLatency()
        {
            return _data.averageScanTime;
        }
        
        public int GetUserSatisfaction()
        {
            return _data.userSatisfactionRating;
        }
        
        public void LogKPIs()
        {
            float recognitionRate = GetRecognitionRate();
            
            Debug.Log("=== KPI Status ===");
            Debug.Log($"Recognition Rate: {recognitionRate:F1}% (Target: ≥95%)");
            Debug.Log($"Average Latency: {_data.averageScanTime:F2}s (Target: ≤1s)");
            Debug.Log($"User Satisfaction: {_data.userSatisfactionRating}% (Target: ≥80%)");
            Debug.Log($"Total Scans: {_data.totalScans} (Success: {_data.successfulScans}, Failed: {_data.failedScans})");
            Debug.Log("==================");
        }
        
        public AnalyticsData GetAnalyticsData()
        {
            return _data;
        }
        
        public void ExportAnalytics()
        {
            string json = JsonUtility.ToJson(_data, true);
            string path = Application.persistentDataPath + "/analytics.json";
            System.IO.File.WriteAllText(path, json);
            Debug.Log($"Analytics exported to: {path}");
        }
    }
}
