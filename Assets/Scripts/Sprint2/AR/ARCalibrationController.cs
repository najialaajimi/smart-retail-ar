using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using SmartRetailAR.Utils;

namespace SmartRetailAR.AR
{
    public class ARCalibrationController : MonoBehaviour
    {
        [Header("UI Elements")]
        public Text statusText;
        public Slider calibrationProgress;
        public Button startButton;
        public Button cancelButton;
        public Text instructionsText;

        [Header("Calibration Settings")]
        public int requiredDataPoints = 10;
        public float calibrationDuration = 5f;

        private int _dataPointsCollected = 0;
        private float _calibrationTimer = 0f;
        private bool _isCalibrating = false;
        private ARSession _arSession;

        void Start()
        {
            _arSession = FindObjectOfType<ARSession>();
            SetupButtons();
            UpdateUI();
        }

        private void SetupButtons()
        {
            if (startButton != null)
                startButton.onClick.AddListener(StartCalibration);

            if (cancelButton != null)
                cancelButton.onClick.AddListener(CancelCalibration);
        }

        void Update()
        {
            if (_isCalibrating)
            {
                PerformCalibration();
            }
        }

        private void StartCalibration()
        {
            _isCalibrating = true;
            _dataPointsCollected = 0;
            _calibrationTimer = 0f;

            if (instructionsText != null)
                instructionsText.text = "Déplacez lentement votre appareil...";

            if (statusText != null)
                statusText.text = "Calibration en cours...";

            Debug.Log("AR Calibration started");
        }

        private void PerformCalibration()
        {
            _calibrationTimer += Time.deltaTime;

            // Simulate data collection
            if (_calibrationTimer >= calibrationDuration / requiredDataPoints)
            {
                _dataPointsCollected++;
                _calibrationTimer = 0f;

                if (calibrationProgress != null)
                    calibrationProgress.value = (float)_dataPointsCollected / requiredDataPoints;

                if (statusText != null)
                    statusText.text = $"Points collectés: {_dataPointsCollected}/{requiredDataPoints}";
            }

            if (_dataPointsCollected >= requiredDataPoints)
            {
                CompleteCalibration();
            }
        }

        private void CompleteCalibration()
        {
            _isCalibrating = false;

            if (statusText != null)
                statusText.text = "Calibration terminée!";

            if (instructionsText != null)
                instructionsText.text = "Calibration réussie. Vous pouvez maintenant utiliser l'AR.";

            PlayerPrefs.SetInt("ARCalibrated", 1);
            PlayerPrefs.Save();

            Debug.Log("AR Calibration completed");

            // Return to AR scene after short delay
            Invoke("ReturnToAR", 2f);
        }

        private void CancelCalibration()
        {
            _isCalibrating = false;
            NavigationManager.Instance.GoBack();
        }

        private void ReturnToAR()
        {
            NavigationManager.Instance.LoadARCameraScene();
        }

        private void UpdateUI()
        {
            if (calibrationProgress != null)
                calibrationProgress.value = 0f;
        }

        public bool IsCalibrated()
        {
            return PlayerPrefs.GetInt("ARCalibrated", 0) == 1;
        }
    }
}
