using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeController : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subtitleText;
    public Button scanButton;
    public Button profileButton;
    public Button helpButton;
    
    [Header("Animation Settings")]
    public float buttonFadeInDelay = 0.3f;
    public float animationDuration = 0.5f;

    private UserPreferences userPreferences;

    void Start()
    {
        InitializeUI();
        LoadUserData();
        SetupButtons();
        PlayIntroAnimation();
    }

    void InitializeUI()
    {
        if (titleText != null)
        {
            titleText.text = "Smart Retail AR";
        }

        if (subtitleText != null)
        {
            subtitleText.text = "Scannez pour découvrir";
        }
    }

    void LoadUserData()
    {
        userPreferences = UserPreferences.Load();
        
        // Show tutorial if first time
        if (userPreferences.settings.showTutorial)
        {
            // TODO: Show tutorial overlay
            Debug.Log("First time user - tutorial should be shown");
        }
    }

    void SetupButtons()
    {
        if (scanButton != null)
        {
            scanButton.onClick.AddListener(OnScanButtonClicked);
        }

        if (profileButton != null)
        {
            profileButton.onClick.AddListener(OnProfileButtonClicked);
        }

        if (helpButton != null)
        {
            helpButton.onClick.AddListener(OnHelpButtonClicked);
        }
    }

    void PlayIntroAnimation()
    {
        // TODO: Implement fade-in animations for UI elements
        Debug.Log("Playing intro animation");
    }

    void OnScanButtonClicked()
    {
        Debug.Log("Navigate to Scanner");
        PlayButtonClickFeedback();
        NavigationManager.Instance.NavigateToScanner();
    }

    void OnProfileButtonClicked()
    {
        Debug.Log("Navigate to Profile");
        PlayButtonClickFeedback();
        NavigationManager.Instance.NavigateToProfile();
    }

    void OnHelpButtonClicked()
    {
        Debug.Log("Show help/tutorial");
        PlayButtonClickFeedback();
        // TODO: Show help overlay or navigate to help scene
    }

    void PlayButtonClickFeedback()
    {
        // Play sound and haptic feedback if enabled
        if (userPreferences != null && userPreferences.settings.soundEnabled)
        {
            // TODO: Play click sound
        }

        if (userPreferences != null && userPreferences.settings.hapticFeedbackEnabled)
        {
            // TODO: Trigger haptic feedback
        }
    }

    void OnDestroy()
    {
        // Cleanup event listeners
        if (scanButton != null)
        {
            scanButton.onClick.RemoveListener(OnScanButtonClicked);
        }

        if (profileButton != null)
        {
            profileButton.onClick.RemoveListener(OnProfileButtonClicked);
        }

        if (helpButton != null)
        {
            helpButton.onClick.RemoveListener(OnHelpButtonClicked);
        }
    }
}
