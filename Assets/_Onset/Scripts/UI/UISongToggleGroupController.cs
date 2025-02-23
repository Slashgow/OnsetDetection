using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class UISongToggleGroupController : MonoBehaviour
{
    [SerializeField]
    private ToggleGroup toggleGroup;

    public List<Toggle> Toggles { get; set; } = new List<Toggle>();

    private List<Toggle> activeToggles = new List<Toggle>();
    private Toggle lastToggleOn;

    public void RegisterValueChanged()
    {
        foreach(Toggle toggle in Toggles)
        {
            toggle.onValueChanged.AddListener(ActiveOnlyLastToggle);
        }
    }

    private void ActiveOnlyLastToggle(bool isOn)
    {
        if (!isOn)
            return;
        Debug.Log("active only last toggle");

        activeToggles.Clear();
        foreach(Toggle toggle in Toggles)
        {
            if(toggle.isOn)
                activeToggles.Add(toggle);
        }

        foreach(Toggle toggle in activeToggles)
        {
            if(lastToggleOn != null && toggle != lastToggleOn)
                lastToggleOn.isOn = false;
        }

        lastToggleOn = activeToggles.First(toggle => toggle.isOn);

    }
}
