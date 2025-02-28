using UnityEngine;
public class SettingManager : PersistentMonoSingleton<SettingManager>
{
    [SerializeField, Range(0.05f,2f)]
    private float defaultMouseSensitivty;

    [SerializeField, Range(0.05f, 2f)]
    private float defaultAimSensitivty;


    public float CurrentMouseSensitivity { get; private set; }
    public float CurrentAimSensitivity { get; private set; }
    public float CurrentSensitivity => IsAiming ? CurrentAimSensitivity : CurrentMouseSensitivity;

    private const string MOUSE_SENSITIVITY_ID = "MOUSE_SENSITIVITY";
    private const string AIM_SENSITIVITY_ID = "AIM_SENSITIVITY";

    public bool IsAiming {  get; private set; }

    protected override void Awake()
    {
        base.Awake();

        if (PlayerPrefs.HasKey(MOUSE_SENSITIVITY_ID))
            CurrentMouseSensitivity = PlayerPrefs.GetFloat(MOUSE_SENSITIVITY_ID);
        else
            CurrentMouseSensitivity = defaultMouseSensitivty;

        if (PlayerPrefs.HasKey(AIM_SENSITIVITY_ID))
            CurrentAimSensitivity = PlayerPrefs.GetFloat(AIM_SENSITIVITY_ID);
        else
            CurrentAimSensitivity = defaultAimSensitivty;
    }

    private void Start()
    {
        SetMouseSensitivity(CurrentMouseSensitivity);
        SetAimSensitivity(CurrentAimSensitivity);
        SetSensitivity(false);
        Debug.Log("Update Sensitivity");
    }

    public void SetSensitivity(bool isAiming) => IsAiming = isAiming;
    public void SetMouseSensitivity(float value)
    {
        CurrentMouseSensitivity = value;
        PlayerPrefs.SetFloat(MOUSE_SENSITIVITY_ID, value);
    }
    public void SetAimSensitivity(float value)
    {
        CurrentAimSensitivity = value;
        PlayerPrefs.SetFloat(AIM_SENSITIVITY_ID, value);
    }
}
