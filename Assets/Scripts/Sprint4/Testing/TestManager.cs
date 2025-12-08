using UnityEngine;
using System;
using System.Collections.Generic;

namespace SmartRetailAR.Testing
{
    public class TestManager : MonoBehaviour
    {
        private static TestManager _instance;
        public static TestManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("TestManager");
                    _instance = go.AddComponent<TestManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        [Header("Test Settings")]
        public bool testModeEnabled = false;
        public string testUserId = "test_user_001";
        public TestMode currentTestMode = TestMode.Standard;

        [Header("A/B Testing")]
        public bool enableABTesting = true;
        public string abTestVariant = "A";

        public enum TestMode
        {
            Standard,
            Performance,
            UsabilityStudy,
            BetaTesting
        }

        public event Action<TestEvent> OnTestEvent;

        private List<TestEvent> _testEvents = new List<TestEvent>();
        private float _testStartTime;

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
            _testStartTime = Time.time;
            Debug.Log($"Test Manager initialized - Mode: {currentTestMode}");
        }

        public void LogTestEvent(string eventName, string category, Dictionary<string, object> data = null)
        {
            TestEvent testEvent = new TestEvent
            {
                eventName = eventName,
                category = category,
                timestamp = DateTime.Now,
                userId = testUserId,
                testMode = currentTestMode.ToString(),
                data = data ?? new Dictionary<string, object>()
            };

            _testEvents.Add(testEvent);
            OnTestEvent?.Invoke(testEvent);
            
            Debug.Log($"Test Event: {eventName} ({category})");
        }

        public void StartTest(TestMode mode, string userId = null)
        {
            currentTestMode = mode;
            if (!string.IsNullOrEmpty(userId))
                testUserId = userId;
            
            _testStartTime = Time.time;
            _testEvents.Clear();

            LogTestEvent("TestStarted", "Session", new Dictionary<string, object>
            {
                ["mode"] = mode.ToString(),
                ["userId"] = testUserId
            });
        }

        public void EndTest()
        {
            float duration = Time.time - _testStartTime;
            
            LogTestEvent("TestEnded", "Session", new Dictionary<string, object>
            {
                ["duration"] = duration,
                ["eventCount"] = _testEvents.Count
            });

            ExportTestResults();
        }

        public void SetABVariant(string variant)
        {
            abTestVariant = variant;
            LogTestEvent("ABVariantSet", "ABTest", new Dictionary<string, object>
            {
                ["variant"] = variant
            });
        }

        public string GetABVariant()
        {
            return abTestVariant;
        }

        public List<TestEvent> GetTestEvents()
        {
            return new List<TestEvent>(_testEvents);
        }

        private void ExportTestResults()
        {
            string json = JsonUtility.ToJson(new TestResults
            {
                userId = testUserId,
                testMode = currentTestMode.ToString(),
                startTime = _testStartTime,
                duration = Time.time - _testStartTime,
                eventCount = _testEvents.Count
            }, true);

            Debug.Log($"Test Results: {json}");
            // In production: Send to analytics server
        }

        public void SimulateUserAction(string action, float delay = 0f)
        {
            if (delay > 0f)
            {
                Invoke(nameof(ExecuteSimulatedAction), delay);
            }
            else
            {
                ExecuteSimulatedAction(action);
            }
        }

        private void ExecuteSimulatedAction(string action = "default")
        {
            LogTestEvent($"SimulatedAction_{action}", "Simulation");
        }

        [System.Serializable]
        public class TestEvent
        {
            public string eventName;
            public string category;
            public DateTime timestamp;
            public string userId;
            public string testMode;
            public Dictionary<string, object> data;
        }

        [System.Serializable]
        private class TestResults
        {
            public string userId;
            public string testMode;
            public float startTime;
            public float duration;
            public int eventCount;
        }
    }
}
