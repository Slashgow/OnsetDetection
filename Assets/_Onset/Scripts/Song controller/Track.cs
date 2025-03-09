using UnityEngine;

public abstract class Track : MonoBehaviour
{
    public abstract float DistanceToHitPoint {  get; }
    public abstract void InitTarget();
}
