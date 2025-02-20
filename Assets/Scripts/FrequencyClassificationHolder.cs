using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FrequencyClassificationHolder : MonoSingleton<FrequencyClassificationHolder>
{
    [SerializeField]
    private List<FrequencyClassification> frequencyClassifications = new List<FrequencyClassification>();

    public List<FrequencyClassification> FrequencyClassifications => frequencyClassifications;

    public FrequencyClassification GetFrequencyClassification(FrequencyDomain domain)
    {
        return frequencyClassifications.First(frequencyClassification => frequencyClassification.FrequencyDomain == domain);
    }
}
