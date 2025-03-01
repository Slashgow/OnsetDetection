using UnityEngine;
using UnityEngine.InputSystem;

public class StarPowerEnabler : MonoBehaviour
{
    private bool isStarPowerUnlocked;
    private void Start()
    {
        ScoreManager.Instance.OnUnlockedStarPower += ScoreManager_OnUnlockedStarPower;
    }
    private void OnDisable()
    {
        ScoreManager.Instance.OnUnlockedStarPower -= ScoreManager_OnUnlockedStarPower;
    }
    private void ScoreManager_OnUnlockedStarPower() => isStarPowerUnlocked = true;

    public void EnableStarPower(InputAction.CallbackContext context)
    {
        Debug.Log("try enable star power");
        if (isStarPowerUnlocked)
        {
            ScoreManager.Instance.UseStarPower();
            isStarPowerUnlocked=false;
        }
            
    }
}
