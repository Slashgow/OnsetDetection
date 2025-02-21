using UnityEngine;
using UnityTimer;

public class Projectile : MonoBehaviour
{
    [SerializeField, Range(0f,500f)]
    private float speed = 100f;

    [SerializeField]
    private GameObject vfxHitPrefab;

    [SerializeField, Range(0f, 1f)]
    private float delayBeforeHidingVFX = 0.3f;

    private Vector3 targetPosition;
    private PoolingSystem projectilePoolingSystem;

    private float distanceBefore;
    private float distanceAfter;
    private Vector3 direction;
    private Timer timer;
    public void Setup(Vector3 targetPosition, PoolingSystem poolingSystem)
    {
        this.targetPosition = targetPosition;
        this.projectilePoolingSystem = poolingSystem;
    }

    private void Update()
    {
        distanceBefore = Vector3.Distance(transform.position, targetPosition);

        direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        distanceAfter = Vector3.Distance(transform.position, targetPosition);

        if(distanceBefore < distanceAfter)
        {
            GameObject vfxHitGameObjectInstance = Instantiate(vfxHitPrefab, targetPosition, Quaternion.identity);
            timer = Timer.Register(delayBeforeHidingVFX, () => Destroy(vfxHitGameObjectInstance));

            projectilePoolingSystem.AddToPool(this.gameObject);
        }


    }
}
