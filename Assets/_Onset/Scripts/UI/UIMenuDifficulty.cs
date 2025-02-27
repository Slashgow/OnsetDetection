using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMenuDifficulty : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown difficultyDropdown;

    private void Start()
    {
        InitializeDropdown();

        difficultyDropdown.onValueChanged.AddListener(UpdateDifficulty);
    }
    private void OnDestroy()
    {
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
            difficultyDropdown.options.Add(new TMP_Dropdown.OptionData(difficulty.ToString()));
            Debug.Log(difficulty.ToString());
        }
        difficultyDropdown.value = DifficultyManager.Instance.CurrentDifficultyIndex;
        difficultyDropdown.RefreshShownValue();
    }
}
