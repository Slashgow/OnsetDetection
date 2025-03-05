using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UILocalization : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown languageSelectionDropdown;

    AsyncOperationHandle initializeOperation;

    void Start()
    {
        // First we setup the dropdown component.
        languageSelectionDropdown.onValueChanged.AddListener(OnSelectionChanged);

        // Clear the options an add a loading message while we wait for the localization system to initialize.
        languageSelectionDropdown.ClearOptions();
        languageSelectionDropdown.options.Add(new TMP_Dropdown.OptionData("Loading..."));
        languageSelectionDropdown.interactable = false;

        // SelectedLocaleAsync will ensure that the locales have been initialized and a locale has been selected.
        initializeOperation = LocalizationSettings.SelectedLocaleAsync;
        if (initializeOperation.IsDone)
        {
            InitializeCompleted(initializeOperation);
        }
        else
        {
            initializeOperation.Completed += InitializeCompleted;
        }
    }

    void InitializeCompleted(AsyncOperationHandle obj)
    {
        // Create an option in the dropdown for each Locale
        var options = new List<string>();
        int selectedOption = 0;
        var locales = LocalizationSettings.AvailableLocales.Locales;
        for (int i = 0; i < locales.Count; ++i)
        {
            var locale = locales[i];
            if (LocalizationSettings.SelectedLocale == locale)
                selectedOption = i;

            var displayName = locales[i].Identifier.CultureInfo != null ? locales[i].Identifier.CultureInfo.NativeName : locales[i].ToString();
            options.Add(displayName);
        }

        // If we have no Locales then something may have gone wrong.
        if (options.Count == 0)
        {
            options.Add("No Locales Available");
            languageSelectionDropdown.interactable = false;
        }
        else
        {
            languageSelectionDropdown.interactable = true;
        }

        languageSelectionDropdown.ClearOptions();
        languageSelectionDropdown.AddOptions(options);
        languageSelectionDropdown.SetValueWithoutNotify(selectedOption);

        LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
    }

    void OnSelectionChanged(int index)
    {
        // Unsubscribe from SelectedLocaleChanged so we don't get an unnecessary callback from the change we are about to make.
        LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;

        var locale = LocalizationSettings.AvailableLocales.Locales[index];
        LocalizationSettings.SelectedLocale = locale;

        // Resubscribe to SelectedLocaleChanged so that we can stay in sync with changes that may be made by other scripts.
        LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
    }

    void LocalizationSettings_SelectedLocaleChanged(Locale locale)
    {
        // We need to update the dropdown selection to match.
        var selectedIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
        languageSelectionDropdown.SetValueWithoutNotify(selectedIndex);
    }
}
