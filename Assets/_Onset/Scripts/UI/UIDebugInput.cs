using TMPro;
using UnityEngine;

public class UIDebugInput : MonoBehaviour
{
    [SerializeField]
    private SwitchController switchController;

    [SerializeField]
    private TextMeshProUGUI debugTextX, debugTextY, debugTextInput;

    [SerializeField]
    private SimplePlayerInput playerInput;

    private void Awake()
    {
        switchController.OnSwitch += SwitchController_OnSwitch;
    }

    private void OnDestroy()
    {
        switchController.OnSwitch -= SwitchController_OnSwitch;
    }

    private void SwitchController_OnSwitch(string obj)
    {
        debugTextInput.text = obj;
    }

    private void Update()
    {
        debugTextX.text = $"x: {playerInput.look.x}";
        debugTextY.text = $" y: {playerInput.look.y}";
    }
}