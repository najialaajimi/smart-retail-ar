using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SmartRetailAR.Analytics
{
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
        public bool enableAnalytics = true;
        public bool enableDebugLogs = true;
        public float sendInterval = 30f; // seconds

        [Header("Firebase")]
        public bool useFirebase = false;
        public string firebaseProjectId = "";

        private Dictionary<string, object> _sessionData = new Dictionary<string, object>();
        private List<AnalyticsEvent> _eventQueue = new List<AnalyticsEvent>();
        private float _lastSendTime;
        private string _sessionId;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeAnalytics();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        void Update()
        {
            if (Time.time - _lastSendTime >= sendInterval)
            {
                SendQueuedEvents();
                _lastSendTime = Time.time;
            }
        }

        private void InitializeAnalytics()
        {
            _sessionId = System.Guid.NewGuid().ToString();
            _lastSendTime = Time.time;

            _sessionData["session_id"] = _sessionId;
            _sessionData["platform"] = Application.platform.ToString();
            _sessionData["version"] = Application.version;
            _sessionData["device_model"] = SystemInfo.deviceModel;
            _sessionData["os_version"] = SystemInfo.operatingSystem;

            LogEvent("SessionStarted", "Session", _sessionData);
            
            if (enableDebugLogs)
                UnityEngine.Debug.Log($"Analytics initialized - Session: {_sessionId}");
        }

        public void LogEvent(string eventName, string category, Dictionary<string, object> parameters = null)
        {
            if (!enableAnalytics) return;

            AnalyticsEvent analyticsEvent = new AnalyticsEvent
            {
                eventName = eventName,
                category = category,
                timestamp = DateTime.UtcNow,
                sessionId = _sessionId,
                parameters = parameters ?? new Dictionary<string, object>()
            };

            _eventQueue.Add(analyticsEvent);

            if (enableDebugLogs)
                UnityEngine.Debug.Log($"Analytics Event: {eventName} ({category})");
        }

        public void LogScreenView(string screenName)
        {
            LogEvent("ScreenView", "Navigation", new Dictionary<string, object>
            {
                ["screen_name"] = screenName,
                ["timestamp"] = DateTime.UtcNow.ToString()
            });
        }

        public void LogUserAction(string action, Dictionary<string, object> details = null)
        {
            LogEvent($"UserAction_{action}", "User", details);
        }

        public void LogProductInteraction(string productId, string interactionType)
        {
            LogEvent("ProductInteraction", "Product", new Dictionary<string, object>
            {
                ["product_id"] = productId,
                ["interaction_type"] = interactionType
            });
        }

        public void LogPurchaseIntent(string productId, float price)
        {
            LogEvent("PurchaseIntent", "Commerce", new Dictionary<string, object>
            {
                ["product_id"] = productId,
                ["price"] = price,
                ["currency"] = "EUR"
            });
        }

        public void LogScanEvent(string scanType, string productId, float duration)
        {
            LogEvent("ScanCompleted", "Scanner", new Dictionary<string, object>
            {
                ["scan_type"] = scanType,
                ["product_id"] = productId,
                ["duration"] = duration
            });
        }

        public void LogError(string errorMessage, string errorType, string stackTrace = "")
        {
            LogEvent("Error", "System", new Dictionary<string, object>
            {
                ["error_message"] = errorMessage,
                ["error_type"] = errorType,
                ["stack_trace"] = stackTrace
            });
        }

        private void SendQueuedEvents()
        {
            if (_eventQueue.Count == 0) return;

            if (useFirebase)
            {
                SendToFirebase();
            }
            else
            {
                SendToCustomBackend();
            }

            if (enableDebugLogs)
                UnityEngine.Debug.Log($"Sent {_eventQueue.Count} analytics events");

            _eventQueue.Clear();
        }

        private void SendToFirebase()
        {
            // Placeholder for Firebase Analytics integration
            // In production: Use Firebase Unity SDK
            UnityEngine.Debug.Log($"Would send {_eventQueue.Count} events to Firebase");
        }

        private void SendToCustomBackend()
        {
            // Placeholder for custom backend
            // In production: Send HTTP POST to analytics endpoint
            UnityEngine.Debug.Log($"Would send {_eventQueue.Count} events to custom backend");
        }

        public Dictionary<string, int> GetEventSummary()
        {
            Dictionary<string, int> summary = new Dictionary<string, int>();
            foreach (var evt in _eventQueue)
            {
                if (!summary.ContainsKey(evt.category))
                    summary[evt.category] = 0;
                summary[evt.category]++;
            }
            return summary;
        }

        public string GetSessionId()
        {
            return _sessionId;
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                LogEvent("AppPaused", "Lifecycle");
                SendQueuedEvents();
            }
            else
            {
                LogEvent("AppResumed", "Lifecycle");
            }
        }

        void OnApplicationQuit()
        {
            LogEvent("SessionEnded", "Session", new Dictionary<string, object>
            {
                ["duration"] = Time.time,
                ["event_count"] = _eventQueue.Count
            });
            SendQueuedEvents();
        }

        [Serializable]
        public class AnalyticsEvent
        {
            public string eventName;
            public string category;
            public DateTime timestamp;
            public string sessionId;
            public Dictionary<string, object> parameters;
        }
    }
}
