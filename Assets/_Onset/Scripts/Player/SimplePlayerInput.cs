using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePlayerInput : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 look;

#if ENABLE_INPUT_SYSTEM
  
    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput(context.ReadValue<Vector2>());
    }
#endif

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }
	
}
