using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private bool isAiming;

    private readonly int DEFAULT_STATE = Animator.StringToHash("DefaultState");
    private readonly int AIM_STATE = Animator.StringToHash("AimState");

    public void SwitchState(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isAiming = true;
            Debug.Log("aim Start");
            animator.Play(AIM_STATE);
            SettingManager.Instance.SetSensitivity(isAiming);
        }
        else if (context.canceled)
        {
            isAiming= false;
            Debug.Log("aim Stop");
            animator.Play(DEFAULT_STATE);
            SettingManager.Instance.SetSensitivity(isAiming);
        }
    }
}
