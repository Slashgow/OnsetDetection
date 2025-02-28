using System.Collections.Generic;
using UnityEngine;

public class NoteHitRater : MonoSingleton<NoteHitRater>
{
    [SerializeField]
    private List<NoteHitClassificationData> noteHitClassificationDatas = new List<NoteHitClassificationData>();


    public NoteHitClassification GetHitClassification(float timeReminingBeforeHit)
    {
        //Debug.Log($"time remaining before hit {timeReminingBeforeHit}");
        foreach(NoteHitClassificationData hitClassificationData in noteHitClassificationDatas)
        {
            if(hitClassificationData.HitClassification != NoteHitClassification.MISS 
                && timeReminingBeforeHit < hitClassificationData.StartTimeRemaining && timeReminingBeforeHit >= hitClassificationData.EndTimeRemaining)
            {
                return hitClassificationData.HitClassification;
            }
        }
        return NoteHitClassification.LATE;
    }
}
