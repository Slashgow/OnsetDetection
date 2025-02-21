using System;
using UnityEngine;
using UnityTimer;

public class Note : MonoBehaviour
{
    private PoolingSystem poolingSystem;
    public PoolingSystem PoolingSystem => poolingSystem;

    public float TimeRemainingBeforeHit {  get; private set; }

    private float timeToHit;
    public bool WasHit { get; set; }
    public NoteHitClassification NoteHitClassification { get; set; }

    public event Action<NoteHitClassification> OnMissNote;

    public void Setup(PoolingSystem poolingSystem, float timeToHit)
    {
        NoteHitClassification = NoteHitClassification.MISS;
        WasHit = false;
        this.poolingSystem = poolingSystem;
        this.timeToHit = timeToHit;
        TimeRemainingBeforeHit = 0.0f;

        Timer.Register(this.timeToHit + 0.5f, onUpdate: secondsElapsed => TimeRemainingBeforeHit = this.timeToHit - secondsElapsed, onComplete: () =>
        {
            if (!WasHit)
            {
                NoteHitClassification = NoteHitClassification.MISS;
                OnMissNote?.Invoke(NoteHitClassification);
            }
        });
    }
}
