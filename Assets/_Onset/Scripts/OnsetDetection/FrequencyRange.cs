using System;
using UnityEngine;

[Serializable]
public struct FrequencyRange 
{
    [SerializeField, Range(0f, 22050f)]
    private float minFrequency;
    public float MinFrequency => minFrequency;

    [SerializeField, Range(0f, 22050f)] 
    private float maxFrequency;
    public float MaxFrequency => maxFrequency;
}
