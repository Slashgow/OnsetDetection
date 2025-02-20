using System;
using UnityEngine;

[Serializable]
public class TransFormByFrequencyDomain
{
    [SerializeField]
    private Transform transform;
    public Transform Transform => transform;

    [SerializeField]
    private FrequencyDomain domain;
    public FrequencyDomain Domain => domain;
}
