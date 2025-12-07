using UnityEngine;
using System.Collections.Generic;
using System;

namespace SmartRetailAR.Testing
{
    /// <summary>
    /// Analytics Manager - Tracks user interactions and app performance
    /// Collects metrics for testing and optimization
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

        [Header("Analytics Settings")]
        [SerializeField] private bool enableAnalytics = true;
        [SerializeField] private bool logToConsole = true;

        private AnalyticsData analyticsData;
        private float sessionStartTime;
        private Dictionary<string, int> eventCounts = new Dictionary<string, int>();
        private Dictionary<string, float> eventTimings = new Dictionary<string, float>();

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                Initialize();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Initialize()
        {
            analyticsData = new AnalyticsData();
            sessionStartTime = Time.time;
            LoadAnalyticsData();
        }

        /// <summary>
        /// Track custom event
        /// </summary>
        public void TrackEvent(string eventName, Dictionary<string, object> parameters = null)
        {
            if (!enableAnalytics) return;

            // Increment event count
            if (eventCounts.ContainsKey(eventName))
            {
                eventCounts[eventName]++;
            }
            else
            {
                eventCounts[eventName] = 1;
            }

            // Log event
            if (logToConsole)
            {
                string paramStr = parameters != null ? string.Join(", ", parameters) : "no params";
                Debug.Log($"Analytics Event: {eventName} ({paramStr})");
            }

            // Store event
            analyticsData.totalEvents++;
            analyticsData.eventLog.Add(new AnalyticsEvent
            {
                eventName = eventName,
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                parameters = parameters
            });
        }

        /// <summary>
        /// Track product scan
        /// </summary>
        public void TrackProductScan(string productId, float detectionTime)
        {
            TrackEvent("product_scan", new Dictionary<string, object>
            {
                { "product_id", productId },
                { "detection_time", detectionTime }
            });

            analyticsData.totalScans++;
            analyticsData.averageScanTime = 
                (analyticsData.averageScanTime * (analyticsData.totalScans - 1) + detectionTime) / analyticsData.totalScans;
        }

        /// <summary>
        /// Track recommendation interaction
        /// </summary>
        public void TrackRecommendation(string productId, string recommendedProductId, bool accepted)
        {
            TrackEvent("recommendation_interaction", new Dictionary<string, object>
            {
                { "product_id", productId },
                { "recommended_product_id", recommendedProductId },
                { "accepted", accepted }
            });

            analyticsData.totalRecommendations++;
            if (accepted)
            {
                analyticsData.acceptedRecommendations++;
            }
        }

        /// <summary>
        /// Track screen view
        /// </summary>
        public void TrackScreenView(string screenName)
        {
            TrackEvent("screen_view", new Dictionary<string, object>
            {
                { "screen_name", screenName }
            });
        }

        /// <summary>
        /// Track user action timing
        /// </summary>
        public void TrackTiming(string category, string variable, float timeMs)
        {
            string key = $"{category}_{variable}";
            eventTimings[key] = timeMs;

            TrackEvent("timing", new Dictionary<string, object>
            {
                { "category", category },
                { "variable", variable },
                { "time_ms", timeMs }
            });
        }

        /// <summary>
        /// Track error
        /// </summary>
        public void TrackError(string errorType, string errorMessage)
        {
            TrackEvent("error", new Dictionary<string, object>
            {
                { "error_type", errorType },
                { "error_message", errorMessage }
            });

            analyticsData.totalErrors++;
        }

        /// <summary>
        /// Track AR performance
        /// </summary>
        public void TrackARPerformance(float fps, float latency)
        {
            TrackEvent("ar_performance", new Dictionary<string, object>
            {
                { "fps", fps },
                { "latency", latency }
            });

            analyticsData.averageFPS = 
                (analyticsData.averageFPS * 0.9f) + (fps * 0.1f); // Moving average
        }

        /// <summary>
        /// Get session duration
        /// </summary>
        public float GetSessionDuration()
        {
            return Time.time - sessionStartTime;
        }

        /// <summary>
        /// Get event count
        /// </summary>
        public int GetEventCount(string eventName)
        {
            return eventCounts.ContainsKey(eventName) ? eventCounts[eventName] : 0;
        }

        /// <summary>
        /// Get analytics summary
        /// </summary>
        public AnalyticsSummary GetAnalyticsSummary()
        {
            return new AnalyticsSummary
            {
                sessionDuration = GetSessionDuration(),
                totalEvents = analyticsData.totalEvents,
                totalScans = analyticsData.totalScans,
                totalRecommendations = analyticsData.totalRecommendations,
                acceptedRecommendations = analyticsData.acceptedRecommendations,
                recommendationAcceptanceRate = analyticsData.totalRecommendations > 0 
                    ? (float)analyticsData.acceptedRecommendations / analyticsData.totalRecommendations 
                    : 0f,
                averageScanTime = analyticsData.averageScanTime,
                averageFPS = analyticsData.averageFPS,
                totalErrors = analyticsData.totalErrors
            };
        }

        /// <summary>
        /// Save analytics data
        /// </summary>
        public void SaveAnalyticsData()
        {
            try
            {
                string json = JsonUtility.ToJson(analyticsData);
                PlayerPrefs.SetString("AnalyticsData", json);
                PlayerPrefs.Save();
                Debug.Log("Analytics data saved");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error saving analytics data: {e.Message}");
            }
        }

        /// <summary>
        /// Load analytics data
        /// </summary>
        private void LoadAnalyticsData()
        {
            try
            {
                string json = PlayerPrefs.GetString("AnalyticsData", "");
                if (!string.IsNullOrEmpty(json))
                {
                    analyticsData = JsonUtility.FromJson<AnalyticsData>(json);
                    Debug.Log("Analytics data loaded");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading analytics data: {e.Message}");
                analyticsData = new AnalyticsData();
            }
        }

        /// <summary>
        /// Clear analytics data
        /// </summary>
        public void ClearAnalyticsData()
        {
            analyticsData = new AnalyticsData();
            eventCounts.Clear();
            eventTimings.Clear();
            PlayerPrefs.DeleteKey("AnalyticsData");
            Debug.Log("Analytics data cleared");
        }

        private void OnApplicationQuit()
        {
            SaveAnalyticsData();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveAnalyticsData();
            }
        }
    }

    [Serializable]
    public class AnalyticsData
    {
        public int totalEvents = 0;
        public int totalScans = 0;
        public int totalRecommendations = 0;
        public int acceptedRecommendations = 0;
        public float averageScanTime = 0f;
        public float averageFPS = 30f;
        public int totalErrors = 0;
        public List<AnalyticsEvent> eventLog = new List<AnalyticsEvent>();
    }

    [Serializable]
    public class AnalyticsEvent
    {
        public string eventName;
        public long timestamp;
        public Dictionary<string, object> parameters;
    }

    [Serializable]
    public class AnalyticsSummary
    {
        public float sessionDuration;
        public int totalEvents;
        public int totalScans;
        public int totalRecommendations;
        public int acceptedRecommendations;
        public float recommendationAcceptanceRate;
        public float averageScanTime;
        public float averageFPS;
        public int totalErrors;

        public override string ToString()
        {
            return $"Session Duration: {sessionDuration:F1}s\n" +
                   $"Total Events: {totalEvents}\n" +
                   $"Total Scans: {totalScans}\n" +
                   $"Recommendations: {acceptedRecommendations}/{totalRecommendations} ({recommendationAcceptanceRate:P0})\n" +
                   $"Avg Scan Time: {averageScanTime:F2}s\n" +
                   $"Avg FPS: {averageFPS:F1}\n" +
                   $"Errors: {totalErrors}";
        }
    }
}
