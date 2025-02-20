using UnityEngine;

public class Translate : MonoBehaviour
{
    [SerializeField]
    private float speed;
    void Update()
    {
        this.transform.Translate(Vector3.forward *  speed * Time.deltaTime);
    }
}
