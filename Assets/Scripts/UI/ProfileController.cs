using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ProfileController : MonoBehaviour
{
    [Header("UI References - Profile")]
    public TextMeshProUGUI userNameText;
    public Image avatarImage;
    public Button editProfileButton;
    public Button backButton;

    [Header("UI References - Preferences")]
    public Transform preferencesContainer;
    public GameObject preferenceTogglePrefab;
    public List<string> availablePreferences = new List<string>
    {
        "Végétarien", "Végétalien", "Sans gluten", "Sans lactose",
        "Bio uniquement", "Éco-responsable", "Local", "Sans allergènes"
    };

    [Header("UI References - History")]
    public Transform historyContainer;
    public GameObject historyItemPrefab;
    public TextMeshProUGUI totalScansText;

    [Header("UI References - Settings")]
    public Toggle soundToggle;
    public Toggle hapticToggle;
    public Toggle tutorialToggle;
    public Toggle darkModeToggle;
    public TMP_Dropdown languageDropdown;

    [Header("UI References - Stats")]
    public TextMeshProUGUI healthyChoicesText;
    public TextMeshProUGUI ecoChoicesText;
    public TextMeshProUGUI scansThisWeekText;

    private UserPreferences userPreferences;

    void Start()
    {
        LoadUserData();
        SetupUI();
        SetupButtons();
        DisplayProfile();
    }

    void LoadUserData()
    {
        userPreferences = UserPreferences.Load();
    }

    void SetupUI()
    {
        if (userNameText != null)
        {
            userNameText.text = userPreferences.userName;
        }

        // Settings
        if (soundToggle != null)
        {
            soundToggle.isOn = userPreferences.settings.soundEnabled;
            soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);
        }

        if (hapticToggle != null)
        {
            hapticToggle.isOn = userPreferences.settings.hapticFeedbackEnabled;
            hapticToggle.onValueChanged.AddListener(OnHapticToggleChanged);
        }

        if (tutorialToggle != null)
        {
            tutorialToggle.isOn = userPreferences.settings.showTutorial;
            tutorialToggle.onValueChanged.AddListener(OnTutorialToggleChanged);
        }

        if (darkModeToggle != null)
        {
            darkModeToggle.isOn = userPreferences.settings.darkModeEnabled;
            darkModeToggle.onValueChanged.AddListener(OnDarkModeToggleChanged);
        }

        if (languageDropdown != null)
        {
            languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
        }
    }

    void SetupButtons()
    {
        if (editProfileButton != null)
        {
            editProfileButton.onClick.AddListener(OnEditProfileClicked);
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }
    }

    void DisplayProfile()
    {
        DisplayPreferences();
        DisplayHistory();
        DisplayStats();
    }

    void DisplayPreferences()
    {
        if (preferencesContainer == null || preferenceTogglePrefab == null) return;

        // Clear existing
        foreach (Transform child in preferencesContainer)
        {
            Destroy(child.gameObject);
        }

        // Create preference toggles
        foreach (string preference in availablePreferences)
        {
            GameObject toggleObj = Instantiate(preferenceTogglePrefab, preferencesContainer);
            Toggle toggle = toggleObj.GetComponent<Toggle>();
            TextMeshProUGUI label = toggleObj.GetComponentInChildren<TextMeshProUGUI>();

            if (label != null)
            {
                label.text = preference;
            }

            if (toggle != null)
            {
                toggle.isOn = userPreferences.dietaryPreferences.Contains(preference);
                toggle.onValueChanged.AddListener((isOn) => OnPreferenceToggled(preference, isOn));
            }
        }
    }

    void DisplayHistory()
    {
        if (historyContainer == null || historyItemPrefab == null) return;

        // Clear existing
        foreach (Transform child in historyContainer)
        {
            Destroy(child.gameObject);
        }

        // Display scanned products (most recent first)
        var recentScans = userPreferences.scannedProductIds
            .TakeLast(10)
            .Reverse()
            .ToList();

        foreach (string productId in recentScans)
        {
            ProductData product = ProductDatabase.Instance.GetProductById(productId);
            if (product != null)
            {
                CreateHistoryItem(product);
            }
        }

        // Update total scans count
        if (totalScansText != null)
        {
            int totalScans = userPreferences.productScanCount.Values.Sum();
            totalScansText.text = $"Total scans: {totalScans}";
        }
    }

    void CreateHistoryItem(ProductData product)
    {
        GameObject item = Instantiate(historyItemPrefab, historyContainer);
        
        TextMeshProUGUI nameText = item.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
        if (nameText != null) nameText.text = product.name;

        TextMeshProUGUI brandText = item.transform.Find("BrandText")?.GetComponent<TextMeshProUGUI>();
        if (brandText != null) brandText.text = product.brand;

        TextMeshProUGUI countText = item.transform.Find("CountText")?.GetComponent<TextMeshProUGUI>();
        if (countText != null)
        {
            int count = userPreferences.GetScanCount(product.productId);
            countText.text = $"Scanné {count}x";
        }

        Button itemButton = item.GetComponent<Button>();
        if (itemButton != null)
        {
            itemButton.onClick.AddListener(() => OnHistoryItemClicked(product));
        }
    }

    void DisplayStats()
    {
        // Calculate stats from scanned products
        int healthyChoices = 0;
        int ecoChoices = 0;

        foreach (string productId in userPreferences.scannedProductIds)
        {
            ProductData product = ProductDatabase.Instance.GetProductById(productId);
            if (product != null && product.scores != null)
            {
                if (product.scores.healthScore >= 70)
                {
                    healthyChoices++;
                }
                if (product.scores.ecoScore >= 70)
                {
                    ecoChoices++;
                }
            }
        }

        if (healthyChoicesText != null)
        {
            healthyChoicesText.text = $"{healthyChoices}";
        }

        if (ecoChoicesText != null)
        {
            ecoChoicesText.text = $"{ecoChoices}";
        }

        if (scansThisWeekText != null)
        {
            // TODO: Calculate actual scans this week
            scansThisWeekText.text = $"{userPreferences.scannedProductIds.Count}";
        }
    }

    void OnPreferenceToggled(string preference, bool isOn)
    {
        if (isOn)
        {
            if (!userPreferences.dietaryPreferences.Contains(preference))
            {
                userPreferences.dietaryPreferences.Add(preference);
            }
        }
        else
        {
            userPreferences.dietaryPreferences.Remove(preference);
        }

        userPreferences.Save();
    }

    void OnSoundToggleChanged(bool isOn)
    {
        userPreferences.settings.soundEnabled = isOn;
        userPreferences.Save();
    }

    void OnHapticToggleChanged(bool isOn)
    {
        userPreferences.settings.hapticFeedbackEnabled = isOn;
        userPreferences.Save();
    }

    void OnTutorialToggleChanged(bool isOn)
    {
        userPreferences.settings.showTutorial = isOn;
        userPreferences.Save();
    }

    void OnDarkModeToggleChanged(bool isOn)
    {
        userPreferences.settings.darkModeEnabled = isOn;
        userPreferences.Save();
        // TODO: Apply dark mode theme
    }

    void OnLanguageChanged(int index)
    {
        string[] languages = { "fr", "en", "es", "de" };
        if (index >= 0 && index < languages.Length)
        {
            userPreferences.settings.language = languages[index];
            userPreferences.Save();
            // TODO: Apply language change
        }
    }

    void OnEditProfileClicked()
    {
        Debug.Log("Edit profile clicked");
        // TODO: Open edit profile dialog
    }

    void OnHistoryItemClicked(ProductData product)
    {
        Debug.Log($"View product from history: {product.name}");
        QRCodeManager.Instance.SetCurrentProductId(product.productId);
        NavigationManager.Instance.NavigateToProductInfo();
    }

    void OnBackButtonClicked()
    {
        NavigationManager.Instance.NavigateBack();
    }

    void OnDestroy()
    {
        if (editProfileButton != null)
        {
            editProfileButton.onClick.RemoveListener(OnEditProfileClicked);
        }

        if (backButton != null)
        {
            backButton.onClick.RemoveListener(OnBackButtonClicked);
        }

        if (soundToggle != null)
        {
            soundToggle.onValueChanged.RemoveListener(OnSoundToggleChanged);
        }

        if (hapticToggle != null)
        {
            hapticToggle.onValueChanged.RemoveListener(OnHapticToggleChanged);
        }

        if (tutorialToggle != null)
        {
            tutorialToggle.onValueChanged.RemoveListener(OnTutorialToggleChanged);
        }

        if (darkModeToggle != null)
        {
            darkModeToggle.onValueChanged.RemoveListener(OnDarkModeToggleChanged);
        }

        if (languageDropdown != null)
        {
            languageDropdown.onValueChanged.RemoveListener(OnLanguageChanged);
        }
    }
}
