using System;
using UnityEngine;

public class CampaignSong : MonoBehaviour
{

}

public class CampaignManager : MonoBehaviour
{
    public int TotalScore { get; private set; }

    public Func<int, bool> IsTotalScoreHigherThan10000 = score => score >= 10000;
}
