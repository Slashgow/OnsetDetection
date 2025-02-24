using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OnsetDetectionFrequencyClassified : OnsetDetection
{
    public List<SpectralFluxInfoClassifiedByFrequency> SpectralFluxInfoListByFrequencyDomain { get; private set; }

    private Dictionary<FrequencyDomain, double> averageOnsetDetectionFunctionByDomain;
    private Dictionary<FrequencyDomain, double> standardDeviationOnsetDetectionFunctionByDomain;
    private float frequencyPerBin;
    private float sampleRate;
    private float spectrumSampleSize;
    public int FrequencyDomainCount { get; private set; }
    private List<ThresholdByFrequencyDomain> thresholdByFrequencyDomains;

    public OnsetDetectionFrequencyClassified(int windowSize, int windowMultiplier, List<ThresholdByFrequencyDomain> thresholdByFrequencyDomains, int spectrumSampleSize, int sampleRate) : base(windowSize, windowMultiplier, thresholdByFrequencyDomains[0].Threshold)
    {
        this.sampleRate = sampleRate;
        this.spectrumSampleSize = spectrumSampleSize;
        this.frequencyPerBin = sampleRate / spectrumSampleSize;
        this.thresholdByFrequencyDomains = thresholdByFrequencyDomains;
        SpectralFluxInfoListByFrequencyDomain = new List<SpectralFluxInfoClassifiedByFrequency>();
        averageOnsetDetectionFunctionByDomain = new Dictionary<FrequencyDomain, double>();
        standardDeviationOnsetDetectionFunctionByDomain = new Dictionary<FrequencyDomain, double>();

        foreach (FrequencyClassification frequencyClassification in FrequencyClassificationHolder.Instance.FrequencyClassifications)
        {
            SpectralFluxInfoListByFrequencyDomain.Add(new SpectralFluxInfoClassifiedByFrequency(frequencyClassification.FrequencyDomain, new List<SpectralFluxInfo>()));
        }
        FrequencyDomainCount = FrequencyClassificationHolder.Instance.FrequencyClassifications.Count;
    }

    public override void AnalyseAllSpectrum()
    {
        for (int n = 1; n < completeSpectrumData.Count; n++)
        {
            CalculateOnsetDetectionFunction(completeSpectrumData[n].spectrum, completeSpectrumData[n - 1].spectrum, completeSpectrumData[n].time);
        }

        CalculateAverageAndStandardDeviationForEachDomain();

        StandardizeOnsetDetectionFunction();

        SetPeaks(windowSize, windowMultiplier);
    }

    private void CalculateAverageAndStandardDeviationForEachDomain()
    {
        foreach (FrequencyClassification frequencyClassification in FrequencyClassificationHolder.Instance.FrequencyClassifications)
        {

            averageOnsetDetectionFunctionByDomain.Add(frequencyClassification.FrequencyDomain,
                SpectralFluxInfoListByFrequencyDomain[GetIndexSpectralFluxInfoClassifiedByFrequency(frequencyClassification.FrequencyDomain)].spectralFluxInfoList.CalculateAverage());

            standardDeviationOnsetDetectionFunctionByDomain.Add(frequencyClassification.FrequencyDomain,
                SpectralFluxInfoListByFrequencyDomain[GetIndexSpectralFluxInfoClassifiedByFrequency(frequencyClassification.FrequencyDomain)].spectralFluxInfoList.
                StandardDeviation(averageOnsetDetectionFunctionByDomain[frequencyClassification.FrequencyDomain]));
        }
    }

    protected override void StandardizeOnsetDetectionFunction()
    {
        for (int i = 0; i < SpectralFluxInfoListByFrequencyDomain.Count; i++)
        {
            for (int k = 0; k < SpectralFluxInfoListByFrequencyDomain[i].spectralFluxInfoList.Count; k++)
            {
                SpectralFluxInfoListByFrequencyDomain[i].spectralFluxInfoList[k].spectralFlux = (SpectralFluxInfoListByFrequencyDomain[i].spectralFluxInfoList[k].spectralFlux - 
                    averageOnsetDetectionFunctionByDomain[SpectralFluxInfoListByFrequencyDomain[i].frequencyDomain] )/
                    standardDeviationOnsetDetectionFunctionByDomain[SpectralFluxInfoListByFrequencyDomain[i].frequencyDomain];

            }
        }   
    }

    protected void SetPeaks(int windowSize, int windowMultiplier)
    {
        // Time window in which humans can not distinguish beats in seconds
        const float indistinguishableRange = 0.01f; // 10ms

        // Number of set of samples to ignore after an onset
        int immunityPeriod =Mathf.RoundToInt(spectrumSampleSize * indistinguishableRange);
        Debug.Log($"immunity Period : {immunityPeriod}");

        for (int i = 0; i < SpectralFluxInfoListByFrequencyDomain.Count; i++)
        {
            float thresholdByDomain = GetThresholdByFrequencyDomain(SpectralFluxInfoListByFrequencyDomain[i].frequencyDomain);
            for (int n = windowSize * windowMultiplier; n < SpectralFluxInfoListByFrequencyDomain[i].spectralFluxInfoList.Count - windowSize; n++)
            {
                SpectralFluxInfoListByFrequencyDomain[i].spectralFluxInfoList[n].isPeak = true;

                // 1st condition
                for (int k = n - windowSize; k <= n + windowSize; k++)
                {
                    if (SpectralFluxInfoListByFrequencyDomain[i].spectralFluxInfoList[n].spectralFlux < SpectralFluxInfoListByFrequencyDomain[i].spectralFluxInfoList[k].spectralFlux)
                        SpectralFluxInfoListByFrequencyDomain[i].spectralFluxInfoList[n].isPeak = false;
                }

                //2nd condition
                double sum = 0;
                for (int k = n - windowMultiplier * windowSize; k <= n + windowSize; k++)
                {
                    sum += SpectralFluxInfoListByFrequencyDomain[i].spectralFluxInfoList[k].spectralFlux;
                }

                if (SpectralFluxInfoListByFrequencyDomain[i].spectralFluxInfoList[n].spectralFlux < sum / (windowMultiplier * windowSize + windowSize + 1) + thresholdByDomain)
                {
                    SpectralFluxInfoListByFrequencyDomain[i].spectralFluxInfoList[n].isPeak = false;
                }

            }

            //remove peaks too close
            for (int n = windowSize * windowMultiplier; n < SpectralFluxInfoListByFrequencyDomain[i].spectralFluxInfoList.Count - windowSize; n++)
            {
                if (SpectralFluxInfoListByFrequencyDomain[i].spectralFluxInfoList[n].isPeak)
                {
                    for (int j = n + 1; j < Mathf.Min(n + immunityPeriod, SpectralFluxInfoListByFrequencyDomain[i].spectralFluxInfoList.Count - windowSize); j++)
                    {
                        if (SpectralFluxInfoListByFrequencyDomain[i].spectralFluxInfoList[j].isPeak)
                        {
                            SpectralFluxInfoListByFrequencyDomain[i].spectralFluxInfoList[j].isPeak = false;
                        }
                    }
                }
            }
        } 
    }

    protected override void CalculateOnsetDetectionFunction(double[] currentSpectrum, double[] previousSpectrum, float time)
    {
        for (int i = 0; i < SpectralFluxInfoListByFrequencyDomain.Count; i++)
        {
            FrequencyClassification frequencyClassification = FrequencyClassificationHolder.Instance.GetFrequencyClassification(SpectralFluxInfoListByFrequencyDomain[i].frequencyDomain);
            int minIndex = Mathf.Max(0, Mathf.RoundToInt(frequencyClassification.FrequencyRange.MinFrequency / frequencyPerBin - 1));
            int maxIndex = Mathf.Min(currentSpectrum.Length - 1, Mathf.RoundToInt(frequencyClassification.FrequencyRange.MaxFrequency / frequencyPerBin - 1));

            //Debug.Log($"frequencyDomain : {frequencyClassification.FrequencyDomain} | min index : {minIndex} | max index : {maxIndex}");
            double spectralFlux = 0f;

            for (int k = minIndex; k <= maxIndex; k++)
            {
                spectralFlux += MathUtility.GetHalfWaveRectifier(Math.Abs(currentSpectrum[k]) - Math.Abs(previousSpectrum[k]));
            }

            SpectralFluxInfoListByFrequencyDomain.First(spectralFluxInfoClassifiedByFrequency => 
            spectralFluxInfoClassifiedByFrequency.frequencyDomain == SpectralFluxInfoListByFrequencyDomain[i].frequencyDomain).spectralFluxInfoList.Add(new SpectralFluxInfo(spectralFlux, time));
        }
    }

    public int GetIndexSpectralFluxInfoClassifiedByFrequency(FrequencyDomain frequencyDomain) => SpectralFluxInfoListByFrequencyDomain.FindIndex(spectralFluxInfoListByFrequency => 
                                                                                                                    spectralFluxInfoListByFrequency.frequencyDomain == frequencyDomain);

    public float GetThresholdByFrequencyDomain(FrequencyDomain frequencyDomain) => thresholdByFrequencyDomains.First(thresholdByFrequencyDomain => thresholdByFrequencyDomain.Domain == frequencyDomain).Threshold;
}
