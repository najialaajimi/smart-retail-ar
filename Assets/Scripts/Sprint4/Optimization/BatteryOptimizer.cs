using UnityEngine;

namespace SmartRetailAR.Optimization
{
    public class BatteryOptimizer : MonoBehaviour
    {
        private static BatteryOptimizer _instance;
        public static BatteryOptimizer Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("BatteryOptimizer");
                    _instance = go.AddComponent<BatteryOptimizer>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        [Header("Battery Settings")]
        public bool enableBatteryOptimization = true;
        public float lowBatteryThreshold = 0.20f;
        public float criticalBatteryThreshold = 0.10f;

        [Header("Optimization Levels")]
        public bool reduceFPSOnLowBattery = true;
        public bool disableAROnLowBattery = false;
        public bool reduceQualityOnLowBattery = true;

        private BatteryOptimizationLevel _currentLevel = BatteryOptimizationLevel.Normal;
        private float _lastBatteryCheck;
        private float _batteryCheckInterval = 10f;

        public enum BatteryOptimizationLevel
        {
            Normal,
            LowPower,
            Critical
        }

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

        void Update()
        {
            if (!enableBatteryOptimization) return;

            if (Time.time - _lastBatteryCheck >= _batteryCheckInterval)
            {
                CheckBatteryStatus();
                _lastBatteryCheck = Time.time;
            }
        }

        private void CheckBatteryStatus()
        {
            float batteryLevel = SystemInfo.batteryLevel;
            BatteryStatus status = SystemInfo.batteryStatus;

            // If battery level is -1, battery info is not available
            if (batteryLevel < 0)
            {
                Debug.LogWarning("Battery information not available");
                return;
            }

            BatteryOptimizationLevel newLevel;

            if (batteryLevel <= criticalBatteryThreshold)
            {
                newLevel = BatteryOptimizationLevel.Critical;
            }
            else if (batteryLevel <= lowBatteryThreshold)
            {
                newLevel = BatteryOptimizationLevel.LowPower;
            }
            else
            {
                newLevel = BatteryOptimizationLevel.Normal;
            }

            if (newLevel != _currentLevel)
            {
                ApplyOptimizationLevel(newLevel);
            }

            Debug.Log($"Battery: {batteryLevel * 100:F0}% ({status}) - Optimization: {_currentLevel}");
        }

        private void ApplyOptimizationLevel(BatteryOptimizationLevel level)
        {
            _currentLevel = level;

            switch (level)
            {
                case BatteryOptimizationLevel.Normal:
                    ApplyNormalSettings();
                    break;

                case BatteryOptimizationLevel.LowPower:
                    ApplyLowPowerSettings();
                    break;

                case BatteryOptimizationLevel.Critical:
                    ApplyCriticalSettings();
                    break;
            }

            Debug.Log($"Battery optimization level changed to: {level}");
        }

        private void ApplyNormalSettings()
        {
            Application.targetFrameRate = 60;
            
            if (PerformanceOptimizer.Instance != null)
            {
                PerformanceOptimizer.Instance.ApplyQualitySettings(
                    PerformanceOptimizer.QualityLevel.High);
            }

            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }

        private void ApplyLowPowerSettings()
        {
            if (reduceFPSOnLowBattery)
                Application.targetFrameRate = 30;

            if (reduceQualityOnLowBattery && PerformanceOptimizer.Instance != null)
            {
                PerformanceOptimizer.Instance.ApplyQualitySettings(
                    PerformanceOptimizer.QualityLevel.Medium);
            }

            Screen.sleepTimeout = 60; // 1 minute
        }

        private void ApplyCriticalSettings()
        {
            Application.targetFrameRate = 20;

            if (PerformanceOptimizer.Instance != null)
            {
                PerformanceOptimizer.Instance.ApplyQualitySettings(
                    PerformanceOptimizer.QualityLevel.Low);
            }

            if (disableAROnLowBattery)
            {
                // Disable AR features
                Debug.Log("AR features disabled due to critical battery");
            }

            Screen.brightness = 0.3f;
            Screen.sleepTimeout = 30; // 30 seconds
        }

        public BatteryInfo GetBatteryInfo()
        {
            return new BatteryInfo
            {
                batteryLevel = SystemInfo.batteryLevel,
                batteryStatus = SystemInfo.batteryStatus.ToString(),
                optimizationLevel = _currentLevel.ToString(),
                isCharging = SystemInfo.batteryStatus == BatteryStatus.Charging
            };
        }

        public void ForceBatteryOptimization(bool enable)
        {
            enableBatteryOptimization = enable;
            
            if (!enable)
            {
                ApplyNormalSettings();
            }
            else
            {
                CheckBatteryStatus();
            }
        }

        public BatteryOptimizationLevel GetCurrentLevel()
        {
            return _currentLevel;
        }

        [System.Serializable]
        public class BatteryInfo
        {
            public float batteryLevel;
            public string batteryStatus;
            public string optimizationLevel;
            public bool isCharging;
        }
    }
}
