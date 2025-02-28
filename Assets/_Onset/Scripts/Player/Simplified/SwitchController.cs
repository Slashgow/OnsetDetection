using System;
using UnityEngine;
using UnityLibrary;

public class SwitchController : MonoBehaviour
{
    [SerializeField]
    private SmoothMouseLook smoothMouseLook;

    [SerializeField]
    private SimpleThirdPersonController simpleThirdPersonController;

    public event Action<string> OnSwitch;

    private void Start()
    {
        smoothMouseLook.enabled = false;
        simpleThirdPersonController.enabled = true;
        OnSwitch?.Invoke("new input");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Switch();
        }
    }
    public void Switch()
    {
        if (smoothMouseLook.enabled)
        {
            smoothMouseLook.enabled = false;
            simpleThirdPersonController.enabled = true;
            OnSwitch?.Invoke("new input");
        }

        else
        {
            OnSwitch?.Invoke("old input");
            smoothMouseLook.enabled = true;
            simpleThirdPersonController.enabled = false;
        }

    }
}
