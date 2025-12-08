using UnityEngine;
using UnityEngine.UI;
using SmartRetailAR.Utils;

namespace SmartRetailAR.UI
{
    public class OnboardingController : MonoBehaviour
    {
        [Header("UI Elements")]
        public GameObject[] onboardingPages;
        public Button nextButton;
        public Button skipButton;
        public Button previousButton;
        public Text pageIndicator;

        private int _currentPageIndex = 0;

        private void Start()
        {
            UpdatePageDisplay();

            if (nextButton != null)
                nextButton.onClick.AddListener(NextPage);

            if (skipButton != null)
                skipButton.onClick.AddListener(SkipOnboarding);

            if (previousButton != null)
                previousButton.onClick.AddListener(PreviousPage);
        }

        private void UpdatePageDisplay()
        {
            for (int i = 0; i < onboardingPages.Length; i++)
            {
                if (onboardingPages[i] != null)
                    onboardingPages[i].SetActive(i == _currentPageIndex);
            }

            if (pageIndicator != null)
                pageIndicator.text = $"{_currentPageIndex + 1}/{onboardingPages.Length}";

            if (previousButton != null)
                previousButton.interactable = _currentPageIndex > 0;

            if (nextButton != null)
            {
                Text buttonText = nextButton.GetComponentInChildren<Text>();
                if (buttonText != null)
                    buttonText.text = _currentPageIndex == onboardingPages.Length - 1 ? "Commencer" : "Suivant";
            }
        }

        private void NextPage()
        {
            if (_currentPageIndex < onboardingPages.Length - 1)
            {
                _currentPageIndex++;
                UpdatePageDisplay();
            }
            else
            {
                CompleteOnboarding();
            }
        }

        private void PreviousPage()
        {
            if (_currentPageIndex > 0)
            {
                _currentPageIndex--;
                UpdatePageDisplay();
            }
        }

        private void SkipOnboarding()
        {
            CompleteOnboarding();
        }

        private void CompleteOnboarding()
        {
            PlayerPrefs.SetInt("HasCompletedOnboarding", 1);
            PlayerPrefs.Save();
            NavigationManager.Instance.LoadHomeScene();
        }
    }
}
