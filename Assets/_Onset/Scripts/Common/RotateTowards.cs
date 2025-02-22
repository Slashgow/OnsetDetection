using UnityEngine;

public class RotateTowards : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField, Range(0f,50f)]
    private float distance = 20f;

    void Update()
    {
        this.transform.LookAt(target, Vector3.up);
    }
}
