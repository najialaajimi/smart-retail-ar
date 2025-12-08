using UnityEngine;
using SmartRetailAR.Data;
using SmartRetailAR.Sprint1.Core;
using SmartRetailAR.Sprint4.Analytics;
using System.Collections;

namespace SmartRetailAR.Sprint4.Testing
{
    /// <summary>
    /// Automated testing system for validating app functionality
    /// Sprint 4: Testing and validation
    /// </summary>
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
        
        [Header("Test Configuration")]
        [SerializeField] private bool autoRunTests = false;
        [SerializeField] private int testIterations = 10;
        
        [Header("Test Results")]
        public int totalTests = 0;
        public int passedTests = 0;
        public int failedTests = 0;
        
        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        void Start()
        {
            if (autoRunTests)
            {
                StartCoroutine(RunAllTests());
            }
        }
        
        public IEnumerator RunAllTests()
        {
            Debug.Log("===== Starting Test Suite =====");
            
            yield return StartCoroutine(TestDatabaseLoading());
            yield return StartCoroutine(TestProductRetrieval());
            yield return StartCoroutine(TestQRScanning());
            yield return StartCoroutine(TestRecommendations());
            yield return StartCoroutine(TestPerformance());
            
            Debug.Log($"===== Test Suite Complete =====");
            Debug.Log($"Total: {totalTests} | Passed: {passedTests} | Failed: {failedTests}");
            
            PrintTestReport();
        }
        
        IEnumerator TestDatabaseLoading()
        {
            Debug.Log("Testing: Database Loading");
            totalTests++;
            
            var db = ProductDatabaseManager.Instance;
            yield return new WaitForSeconds(0.5f);
            
            if (db.IsLoaded && db.GetAllProducts().Count > 0)
            {
                passedTests++;
                Debug.Log("✓ Database loading test PASSED");
            }
            else
            {
                failedTests++;
                Debug.LogError("✗ Database loading test FAILED");
            }
        }
        
        IEnumerator TestProductRetrieval()
        {
            Debug.Log("Testing: Product Retrieval");
            totalTests++;
            
            var product = ProductDatabaseManager.Instance.GetProductById("PROD001");
            
            if (product != null)
            {
                passedTests++;
                Debug.Log($"✓ Product retrieval test PASSED: Found {product.name}");
            }
            else
            {
                failedTests++;
                Debug.LogError("✗ Product retrieval test FAILED");
            }
            
            yield return null;
        }
        
        IEnumerator TestQRScanning()
        {
            Debug.Log("Testing: QR Scanning (Simulated)");
            totalTests++;
            
            // Simulate multiple scans
            bool allScansSuccessful = true;
            
            for (int i = 0; i < 5; i++)
            {
                AnalyticsManager.Instance.StartScanTimer();
                yield return new WaitForSeconds(0.2f);
                
                var product = ProductDatabaseManager.Instance.GetProductById($"PROD00{i + 1}");
                if (product != null)
                {
                    AnalyticsManager.Instance.TrackProductScan(product.id, true);
                }
                else
                {
                    allScansSuccessful = false;
                    AnalyticsManager.Instance.TrackProductScan($"PROD00{i + 1}", false);
                }
            }
            
            float recognitionRate = AnalyticsManager.Instance.GetRecognitionRate();
            
            if (recognitionRate >= 95f)
            {
                passedTests++;
                Debug.Log($"✓ QR Scanning test PASSED: Recognition rate {recognitionRate:F1}%");
            }
            else
            {
                failedTests++;
                Debug.LogError($"✗ QR Scanning test FAILED: Recognition rate {recognitionRate:F1}% < 95%");
            }
        }
        
        IEnumerator TestRecommendations()
        {
            Debug.Log("Testing: Recommendation Engine");
            totalTests++;
            
            var product = ProductDatabaseManager.Instance.GetProductById("PROD001");
            
            if (product != null)
            {
                var recommendations = Sprint3.Recommendations.RecommendationEngine.Instance
                    .GetRecommendations(product, 5);
                
                if (recommendations != null && recommendations.Count > 0)
                {
                    passedTests++;
                    Debug.Log($"✓ Recommendations test PASSED: Found {recommendations.Count} alternatives");
                }
                else
                {
                    failedTests++;
                    Debug.LogError("✗ Recommendations test FAILED: No alternatives found");
                }
            }
            else
            {
                failedTests++;
                Debug.LogError("✗ Recommendations test FAILED: Product not found");
            }
            
            yield return null;
        }
        
        IEnumerator TestPerformance()
        {
            Debug.Log("Testing: Performance Metrics");
            totalTests++;
            
            yield return new WaitForSeconds(2f);
            
            float avgLatency = AnalyticsManager.Instance.GetAverageLatency();
            
            if (avgLatency <= 1.0f || avgLatency == 0f) // 0 means no data yet
            {
                passedTests++;
                Debug.Log($"✓ Performance test PASSED: Latency {avgLatency:F2}s");
            }
            else
            {
                failedTests++;
                Debug.LogError($"✗ Performance test FAILED: Latency {avgLatency:F2}s > 1s");
            }
        }
        
        void PrintTestReport()
        {
            string report = "\n===== TEST REPORT =====\n";
            report += $"Total Tests: {totalTests}\n";
            report += $"Passed: {passedTests}\n";
            report += $"Failed: {failedTests}\n";
            report += $"Success Rate: {(passedTests / (float)totalTests * 100):F1}%\n";
            report += "=======================\n";
            
            Debug.Log(report);
            
            // Also check KPIs
            AnalyticsManager.Instance.LogKPIs();
        }
        
        /// <summary>
        /// Run a specific test by name
        /// </summary>
        public void RunTest(string testName)
        {
            switch (testName.ToLower())
            {
                case "database":
                    StartCoroutine(TestDatabaseLoading());
                    break;
                case "product":
                    StartCoroutine(TestProductRetrieval());
                    break;
                case "scanning":
                    StartCoroutine(TestQRScanning());
                    break;
                case "recommendations":
                    StartCoroutine(TestRecommendations());
                    break;
                case "performance":
                    StartCoroutine(TestPerformance());
                    break;
                case "all":
                    StartCoroutine(RunAllTests());
                    break;
                default:
                    Debug.LogWarning($"Unknown test: {testName}");
                    break;
            }
        }
    }
}
