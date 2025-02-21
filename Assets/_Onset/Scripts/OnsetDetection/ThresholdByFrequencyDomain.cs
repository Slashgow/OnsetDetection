using System;
using UnityEngine;

[Serializable]
public class ThresholdByFrequencyDomain 
{
    [SerializeField, Range(0f, 0.001f)]
    private float threshold;
    public float Threshold => threshold;

    [SerializeField]
    private FrequencyDomain domain;
    public FrequencyDomain Domain => domain;
}
