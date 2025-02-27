using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoSingleton<UISettings>
{
    [SerializeField]
    private Slider mouseSensitivitySlider;

    [SerializeField]
    private Slider aimSensitivitySlider;

    private void Start()
    {
        mouseSensitivitySlider.value = SettingManager.Instance.CurrentMouseSensitivity;
        aimSensitivitySlider.value = SettingManager.Instance.CurrentAimSensitivity;
    }

    public void SetMouseSensitivity(float value)
    {
        SettingManager.Instance.SetMouseSensitivity(value);
    }

    public void SetAimSensitivity(float value)
    {
        SettingManager.Instance.SetAimSensitivity(value);
    }
}
