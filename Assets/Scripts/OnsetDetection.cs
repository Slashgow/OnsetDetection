using System;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
public class OnsetDetection 
{
    protected int windowSize;
    protected int windowMultiplier;
    protected float threshold;

    protected List<SpectrumData> completeSpectrumData;

    private List<SpectralFluxInfo> spectralFluxInfoList;
    public List<SpectralFluxInfo> SpectralFluxInfoList => spectralFluxInfoList;

    private double averageOnsetDetectionFunction;
    private double standardDeviationOnsetDetectionFunction;

    public OnsetDetection(int windowSize, int windowMultiplier, float threshold)
    {
        this.windowSize = windowSize;
        this.windowMultiplier = windowMultiplier;
        this.threshold = threshold;

        completeSpectrumData = new List<SpectrumData>();
        spectralFluxInfoList = new List<SpectralFluxInfo>();

    }

    public void PopulateCompleteSpectrumData(SpectrumData spectrumData) => completeSpectrumData.Add(spectrumData);

    public virtual void AnalyseAllSpectrum()
    {

        for (int n=1 ; n<completeSpectrumData.Count; n++)
        {
            CalculateOnsetDetectionFunction(completeSpectrumData[n].spectrum, completeSpectrumData[n - 1].spectrum, completeSpectrumData[n].time);
        }

        averageOnsetDetectionFunction = CalculateAverageOnsetDetectionFunction();
        standardDeviationOnsetDetectionFunction = CalculateStandardDeviationOnsetDetectionFunction();

        StandardizeOnsetDetectionFunction();

        SetPeaks(windowSize, windowMultiplier, threshold);
    }

    protected virtual void CalculateOnsetDetectionFunction(double[] currentSpectrum, double[] previousSpectrum, float time)
    {
        Debug.Log($"current Spectrum Size {currentSpectrum.Length}");
        double spectralFlux = 0;
        for (int k = 0; k < currentSpectrum.Length; k++)
        {
            spectralFlux += MathUtility.GetHalfWaveRectifier(Math.Abs(currentSpectrum[k]) - Math.Abs(previousSpectrum[k]));
        }
        spectralFluxInfoList.Add(new SpectralFluxInfo(spectralFlux, time));
    }

    protected virtual void StandardizeOnsetDetectionFunction()
    {
        for (int k = 0; k < spectralFluxInfoList.Count; k++)
        {
            spectralFluxInfoList[k].spectralFlux = ( spectralFluxInfoList[k].spectralFlux - averageOnsetDetectionFunction ) / standardDeviationOnsetDetectionFunction;
            spectralFluxInfoList[k].isStandardized = true;
        }
    }

    protected virtual void SetPeaks(int windowSize, int windowMultiplier, float threshold)
    {
        for(int n = windowSize * windowMultiplier; n < spectralFluxInfoList.Count - windowSize; n++)
        {
            spectralFluxInfoList[n].isPeak = true;

            // 1st condition
            for (int k = n - windowSize; k <= n + windowSize; k++)
            {
                if (spectralFluxInfoList[n].spectralFlux < spectralFluxInfoList[k].spectralFlux)
                    spectralFluxInfoList[n].isPeak = false;
            }

            //2nd condition
            double sum = 0;
            for (int k = n - windowMultiplier * windowSize; k <= n + windowSize; k++)
            {
                sum += spectralFluxInfoList[k].spectralFlux;
            }

            if(spectralFluxInfoList[n].spectralFlux < sum / (windowMultiplier * windowSize + windowSize + 1) + threshold)
            {
                spectralFluxInfoList[n].isPeak = false;
            }
           
        }
    }

    private double CalculateAverageOnsetDetectionFunction() => spectralFluxInfoList.CalculateAverage();
    private double CalculateStandardDeviationOnsetDetectionFunction() => spectralFluxInfoList.StandardDeviation(averageOnsetDetectionFunction);


}
