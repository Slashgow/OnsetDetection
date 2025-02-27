using StarterAssets;
using Unity.Cinemachine.Samples;
using UnityEngine;

public class SimpleAimController : MonoBehaviour
{


    ThirdPersonController thirdPersonController;
    //void OnEnable()
    //{
    //    thirdPersonController = GetComponentInParent<ThirdPersonController>();
    //    if (thirdPersonController == null)
    //        Debug.LogError("SimplePlayerController not found on parent object");
    //    else
    //    {
    //        thirdPersonController.PreUpdate -= UpdatePlayerRotation;
    //        thirdPersonController.PreUpdate += UpdatePlayerRotation;
    //
    //        m_ControllerTransform = m_Controller.transform;
    //    }
    //}
    //
    //void OnDisable()
    //{
    //    if (m_Controller != null)
    //    {
    //        m_Controller.PreUpdate -= UpdatePlayerRotation;
    //        m_Controller.PostUpdate -= PostUpdate;
    //        m_ControllerTransform = null;
    //    }
    //}
}
