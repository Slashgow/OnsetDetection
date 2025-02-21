using System;
using UnityEngine;

[Serializable]
public class FrequencyClassification 
{
    [SerializeField]
    private FrequencyDomain frequencyDomain;
    public FrequencyDomain FrequencyDomain => frequencyDomain;

    [SerializeField]
    private FrequencyRange frequencyRange;
    public FrequencyRange FrequencyRange => frequencyRange;
}
