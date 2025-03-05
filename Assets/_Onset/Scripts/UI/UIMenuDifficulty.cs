using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class UIMenuDifficulty : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown difficultyDropdown;

    [SerializeField]
    private LocalizedString beginnerLocalizedString, mediumLocalizedString, hardLocalizedString, extremeLocalizedString;

    private void Start()
    {
        InitializeDropdown();
        LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
        difficultyDropdown.onValueChanged.AddListener(UpdateDifficulty);
    }
    private void OnDestroy()
    {
        LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;
        difficultyDropdown.onValueChanged.RemoveListener(UpdateDifficulty);
    }

    private void UpdateDifficulty(int valueIndex) => DifficultyManager.Instance.SetDifficulty(valueIndex);

    public void InitializeDropdown()
    {
        difficultyDropdown.ClearOptions();
        var difficulties = Enum.GetValues(typeof(Difficulty));
        List<string> dropdownOptions = new List<string>();

        foreach (var difficulty in difficulties)
        {
            difficultyDropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedStringDifficulty((Difficulty)difficulty)));
            Debug.Log(difficulty.ToString());
        }
        difficultyDropdown.value = DifficultyManager.Instance.CurrentDifficultyIndex;
        difficultyDropdown.RefreshShownValue();
    }

    public string GetLocalizedStringDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.BEGINNER: return beginnerLocalizedString.GetLocalizedString();
            case Difficulty.MEDIUM: return mediumLocalizedString.GetLocalizedString();
            case Difficulty.HARD: return hardLocalizedString.GetLocalizedString();
            case Difficulty.EXTREME: return extremeLocalizedString.GetLocalizedString();
            default:
                return beginnerLocalizedString.GetLocalizedString();
        }
    }

    private void LocalizationSettings_SelectedLocaleChanged(Locale locale)
    {
        InitializeDropdown();
    }

}
