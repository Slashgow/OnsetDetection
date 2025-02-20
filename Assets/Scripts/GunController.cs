using System;
using System.Collections;
using UnityTimer;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private PoolingSystem poolingSystem;

    [SerializeField]
    private Transform muzzleFlashTransform;

    [SerializeField]
    private float delayBeforeHiddingMuzzleFlash;

    private Timer timer;
    public void Shoot(InputAction.CallbackContext context)
    {
        Debug.Log("shoot");
        if (context.performed)
        {
            GameObject muzzleFlashInstance = Instantiate(poolingSystem.GetPrefabFromPool(), muzzleFlashTransform);
            timer = Timer.Register(delayBeforeHiddingMuzzleFlash, () => poolingSystem.AddToPool(muzzleFlashInstance));
        }
    }

}
