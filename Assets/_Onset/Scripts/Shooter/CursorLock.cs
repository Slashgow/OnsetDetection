using UnityEngine;

public class CursorLock : MonoBehaviour
{
    private void OnApplicationFocus(bool focus)
    {
        if(focus)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }

}
