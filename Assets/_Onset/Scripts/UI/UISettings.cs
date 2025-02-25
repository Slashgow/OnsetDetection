using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoSingleton<UISettings>
{
    [SerializeField]
    private Slider mouseSensitivitySlider;

    private void Start()
    {
        mouseSensitivitySlider.value = SettingManager.Instance.CurrentMouseSensitivity;
    }

    public void SetMouseSensitivity(float value)
    {
        SettingManager.Instance.SetMouseSensitivity(value);
    }
}
