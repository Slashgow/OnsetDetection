using System;
using UnityEngine;

[Serializable]
public class SongSpawnDataCircle : SongSpawnData
{
    [SerializeField]
    private float timeToHit;
    public override float TimeToHitFirstTrack => timeToHit;

    public override float TimeToHit(Track track) => timeToHit;
}
