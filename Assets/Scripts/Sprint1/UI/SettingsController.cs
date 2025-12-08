using UnityEngine;
using UnityEngine.UI;
using SmartRetailAR.Utils;

namespace SmartRetailAR.UI
{
    public class SettingsController : MonoBehaviour
    {
        [Header("UI Elements")]
        public Toggle notificationsToggle;
        public Toggle soundToggle;
        public Toggle arEnabledToggle;
        public Slider volumeSlider;
        public Dropdown languageDropdown;
        public Button backButton;
        public Button clearDataButton;
        public Button aboutButton;
        public Text versionText;

        private void Start()
        {
            LoadSettings();
            SetupButtons();
            SetupToggles();

            if (versionText != null)
                versionText.text = $"Version {Application.version}";
        }

        private void SetupButtons()
        {
            if (backButton != null)
                backButton.onClick.AddListener(() => NavigationManager.Instance.GoBack());

            if (clearDataButton != null)
                clearDataButton.onClick.AddListener(ClearAllData);

            if (aboutButton != null)
                aboutButton.onClick.AddListener(ShowAbout);
        }

        private void SetupToggles()
        {
            if (notificationsToggle != null)
                notificationsToggle.onValueChanged.AddListener(SetNotifications);

            if (soundToggle != null)
                soundToggle.onValueChanged.AddListener(SetSound);

            if (arEnabledToggle != null)
                arEnabledToggle.onValueChanged.AddListener(SetAREnabled);

            if (volumeSlider != null)
                volumeSlider.onValueChanged.AddListener(SetVolume);

            if (languageDropdown != null)
                languageDropdown.onValueChanged.AddListener(SetLanguage);
        }

        private void LoadSettings()
        {
            if (notificationsToggle != null)
                notificationsToggle.isOn = PlayerPrefs.GetInt("Notifications", 1) == 1;

            if (soundToggle != null)
                soundToggle.isOn = PlayerPrefs.GetInt("Sound", 1) == 1;

            if (arEnabledToggle != null)
                arEnabledToggle.isOn = PlayerPrefs.GetInt("AREnabled", 1) == 1;

            if (volumeSlider != null)
                volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.8f);
        }

        private void SetNotifications(bool enabled)
        {
            PlayerPrefs.SetInt("Notifications", enabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void SetSound(bool enabled)
        {
            PlayerPrefs.SetInt("Sound", enabled ? 1 : 0);
            AudioListener.volume = enabled ? PlayerPrefs.GetFloat("Volume", 0.8f) : 0f;
            PlayerPrefs.Save();
        }

        private void SetAREnabled(bool enabled)
        {
            PlayerPrefs.SetInt("AREnabled", enabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void SetVolume(float volume)
        {
            PlayerPrefs.SetFloat("Volume", volume);
            AudioListener.volume = volume;
            PlayerPrefs.Save();
        }

        private void SetLanguage(int languageIndex)
        {
            PlayerPrefs.SetInt("Language", languageIndex);
            PlayerPrefs.Save();
            // TODO: Implement language change
        }

        private void ClearAllData()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("All data cleared");
        }

        private void ShowAbout()
        {
            Debug.Log("Smart Retail AR - Version complète");
        }
    }
}
