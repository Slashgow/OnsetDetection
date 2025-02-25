using UnityEngine;
public class SettingManager : PersistentMonoSingleton<SettingManager>
{
    [SerializeField, Range(0.3f,4f)]
    private float defaultMouseSensitivty;
    public float CurrentMouseSensitivity { get; private set; }

    private const string MOUSE_SENSITIVITY_ID = "MOUSE_SENSITIVITY";

    protected override void Awake()
    {
        base.Awake();

        if (PlayerPrefs.HasKey(MOUSE_SENSITIVITY_ID))
            CurrentMouseSensitivity = PlayerPrefs.GetFloat(MOUSE_SENSITIVITY_ID);
        else
            CurrentMouseSensitivity = defaultMouseSensitivty;
    }

    private void Start()
    {
        SetMouseSensitivity(CurrentMouseSensitivity);
    }

    public void SetMouseSensitivity(float value)
    {
        CurrentMouseSensitivity = value;
        PlayerPrefs.SetFloat(MOUSE_SENSITIVITY_ID, value);
    }
}
