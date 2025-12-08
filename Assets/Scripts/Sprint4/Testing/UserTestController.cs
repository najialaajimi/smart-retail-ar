using UnityEngine;
using UnityEngine.UI;
using SmartRetailAR.Utils;
using System.Collections.Generic;

namespace SmartRetailAR.Testing
{
    public class UserTestController : MonoBehaviour
    {
        [Header("UI Elements")]
        public Text instructionsText;
        public Text taskNumberText;
        public Button nextTaskButton;
        public Button completeButton;
        public Slider progressSlider;
        public Text feedbackText;

        [Header("Test Tasks")]
        public List<TestTask> testTasks = new List<TestTask>();

        private int _currentTaskIndex = 0;
        private float _taskStartTime;

        [System.Serializable]
        public class TestTask
        {
            public string taskName;
            public string description;
            public string instructions;
            public string expectedAction;
            public float timeLimit; // seconds, 0 = no limit
        }

        void Start()
        {
            SetupDefaultTasks();
            SetupButtons();
            StartNextTask();
        }

        private void SetupDefaultTasks()
        {
            if (testTasks.Count == 0)
            {
                testTasks = new List<TestTask>
                {
                    new TestTask
                    {
                        taskName = "Scanner un produit",
                        description = "Utilisez le scanner QR pour scanner un produit",
                        instructions = "1. Appuyez sur 'Scanner'\n2. Pointez vers un code QR\n3. Attendez la reconnaissance",
                        expectedAction = "qr_scan_complete",
                        timeLimit = 60f
                    },
                    new TestTask
                    {
                        taskName = "Voir les informations",
                        description = "Consultez les informations détaillées du produit",
                        instructions = "1. Lisez les informations nutritionnelles\n2. Consultez les scores\n3. Vérifiez l'origine",
                        expectedAction = "product_info_viewed",
                        timeLimit = 45f
                    },
                    new TestTask
                    {
                        taskName = "Trouver une alternative",
                        description = "Trouvez un produit alternatif plus sain",
                        instructions = "1. Appuyez sur 'Alternatives'\n2. Comparez les produits\n3. Sélectionnez le meilleur",
                        expectedAction = "alternative_selected",
                        timeLimit = 90f
                    },
                    new TestTask
                    {
                        taskName = "Utiliser l'AR",
                        description = "Scannez un produit en réalité augmentée",
                        instructions = "1. Activez la caméra AR\n2. Pointez vers un produit\n3. Visualisez les informations AR",
                        expectedAction = "ar_scan_complete",
                        timeLimit = 120f
                    }
                };
            }
        }

        private void SetupButtons()
        {
            if (nextTaskButton != null)
                nextTaskButton.onClick.AddListener(NextTask);

            if (completeButton != null)
            {
                completeButton.onClick.AddListener(CompleteTest);
                completeButton.gameObject.SetActive(false);
            }
        }

        private void StartNextTask()
        {
            if (_currentTaskIndex >= testTasks.Count)
            {
                CompleteTest();
                return;
            }

            TestTask task = testTasks[_currentTaskIndex];
            _taskStartTime = Time.time;

            if (instructionsText != null)
                instructionsText.text = task.instructions;

            if (taskNumberText != null)
                taskNumberText.text = $"Tâche {_currentTaskIndex + 1}/{testTasks.Count}: {task.taskName}";

            if (progressSlider != null)
                progressSlider.value = (float)_currentTaskIndex / testTasks.Count;

            TestManager.Instance.LogTestEvent("TaskStarted", "UserTest", new Dictionary<string, object>
            {
                ["taskName"] = task.taskName,
                ["taskIndex"] = _currentTaskIndex
            });

            Debug.Log($"Started task: {task.taskName}");
        }

        private void NextTask()
        {
            float duration = Time.time - _taskStartTime;
            TestTask task = testTasks[_currentTaskIndex];

            TestManager.Instance.LogTestEvent("TaskCompleted", "UserTest", new Dictionary<string, object>
            {
                ["taskName"] = task.taskName,
                ["duration"] = duration,
                ["success"] = true
            });

            _currentTaskIndex++;
            
            if (_currentTaskIndex >= testTasks.Count)
            {
                if (completeButton != null)
                    completeButton.gameObject.SetActive(true);
                if (nextTaskButton != null)
                    nextTaskButton.gameObject.SetActive(false);
            }
            else
            {
                StartNextTask();
            }
        }

        private void CompleteTest()
        {
            TestManager.Instance.LogTestEvent("UserTestCompleted", "UserTest", new Dictionary<string, object>
            {
                ["totalTasks"] = testTasks.Count,
                ["completedTasks"] = _currentTaskIndex
            });

            if (feedbackText != null)
                feedbackText.text = "Test terminé! Merci de votre participation.";

            Debug.Log("User test completed");
            
            // Navigate to feedback scene
            Invoke("GoToFeedback", 2f);
        }

        private void GoToFeedback()
        {
            NavigationManager.Instance.LoadFeedbackScene();
        }

        public void RecordTaskAction(string action)
        {
            if (_currentTaskIndex < testTasks.Count)
            {
                TestTask task = testTasks[_currentTaskIndex];
                if (task.expectedAction == action)
                {
                    NextTask();
                }
            }
        }

        void Update()
        {
            // Check time limit
            if (_currentTaskIndex < testTasks.Count)
            {
                TestTask task = testTasks[_currentTaskIndex];
                if (task.timeLimit > 0)
                {
                    float elapsed = Time.time - _taskStartTime;
                    if (elapsed >= task.timeLimit)
                    {
                        if (feedbackText != null)
                            feedbackText.text = "Temps écoulé pour cette tâche";
                        
                        Invoke(nameof(NextTask), 2f);
                    }
                }
            }
        }
    }
}
