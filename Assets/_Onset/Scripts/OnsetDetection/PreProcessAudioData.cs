using DSPLib;
using System.Numerics;
using System;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;

public class PreProcessAudioData : MonoBehaviour
{
    [SerializeField]
    private AudioClipMapper audioClipMapper;
    //public AudioClip AudioClip => audioClipData;

    [SerializeField, Range(0, 5)]
    private int windowSize = 3;

    [SerializeField, Range(0, 5)]
    private int windowMultiplier = 3;

    [SerializeField, Range(0f, 0.001f)]
    private float threshold = 0.00001f;

    [SerializeField]
    private bool useFrequencyDomainClassification;
    public bool UseFrequencyDomainClassification => useFrequencyDomainClassification;

    [SerializeField]
    private List<ThresholdByFrequencyDomain> thresholdByFrequencyDomainList;

    private float[] multiChannelSamples;
    private float[] averageMonoSamples;
    private int numberOfTotalSamples;
    private int numberOfChannels;
    private float clipLength;
    private int sampleRate;

    public OnsetDetection OnsetDetection { get; private set; }
    public OnsetDetectionFrequencyClassified OnsetDetectionFrequencyClassified { get; private set; }

    public event Action OnFinishAnalyseFullSpectrum;
    public bool IsAnalysedFinished { get; private set; }
    private bool wasRaised;

    private void Start()
    {
        SetAudioClipData();

        Thread backgroundThread  = new Thread(GetFullSpectrumThreaded);
        Debug.Log("Starting Background Thread");
        backgroundThread.Start();
    }

    private void Update()
    {
        if (IsAnalysedFinished && !wasRaised)
        {
            SaveSongDataToJson();
            OnFinishAnalyseFullSpectrum?.Invoke();
            wasRaised = true;
        }
    }
    public void GetFullSpectrumThreaded()
    {
        try
        {
            if(!useFrequencyDomainClassification)
                OnsetDetection = new OnsetDetection(windowSize, windowMultiplier, threshold);
            else 
                OnsetDetectionFrequencyClassified = new OnsetDetectionFrequencyClassified(windowSize, windowMultiplier, thresholdByFrequencyDomainList, 1024, sampleRate);

            ConvertSamplesFromMonoToStereo();
            PerformFFTOnAverageMonoSamples();

            if(!useFrequencyDomainClassification)
                OnsetDetection.AnalyseAllSpectrum();
            else 
                OnsetDetectionFrequencyClassified.AnalyseAllSpectrum();

            Debug.Log("All spectrum analysed");
            IsAnalysedFinished = true;
        }
        catch (Exception e)
        {
            // Catch exceptions here since the background thread won't always surface the exception to the main thread
            Debug.Log(e.ToString());
        }
    }

    public void SetAudioClipData()
    {
        numberOfChannels = audioClipMapper.AudioClip.channels;
        numberOfTotalSamples = audioClipMapper.AudioClip.samples;
        clipLength = audioClipMapper.AudioClip.length;
        sampleRate = audioClipMapper.AudioClip.frequency;

        multiChannelSamples = new float[audioClipMapper.AudioClip.samples *  audioClipMapper.AudioClip.channels];
        audioClipMapper.AudioClip.GetData(multiChannelSamples, 0);

        Debug.Log("Set Data is Done");
    }

    public void ConvertSamplesFromMonoToStereo()
    {
        averageMonoSamples = new float[numberOfTotalSamples];

        int indexAverageMonoSamples = 0;
        float combinedChannelAverage = 0f;

        for (int i = 0; i <multiChannelSamples.Length; i++)
        {
            combinedChannelAverage += multiChannelSamples[i];

            if( (i + 1) % this.numberOfChannels == 0)
            {
                averageMonoSamples[indexAverageMonoSamples] = combinedChannelAverage / this.numberOfChannels;
                indexAverageMonoSamples++;
                combinedChannelAverage = 0f;
            }
        }

        Debug.Log("Combine Channels done");
        Debug.Log(averageMonoSamples.Length);
    }

    private void PerformFFTOnAverageMonoSamples()
    {
        // Once we have our audio sample data prepared, we can execute an FFT to return the spectrum data over the time domain
        int spectrumSampleSize = 1024;
        int iterations = averageMonoSamples.Length / spectrumSampleSize;

        FFT fft = new FFT();
        fft.Initialize((UInt32)spectrumSampleSize);

        Debug.Log(string.Format("Processing {0} time domain samples for FFT", iterations));
        double[] sampleChunk = new double[spectrumSampleSize];
        for (int i = 0; i < iterations; i++)
        {
            // Grab the current 1024 chunk of audio sample data
            Array.Copy(averageMonoSamples, i * spectrumSampleSize, sampleChunk, 0, spectrumSampleSize);

            // Apply our chosen FFT Window
            double[] windowCoefs = DSP.Window.Coefficients(DSP.Window.Type.Hanning, (uint)spectrumSampleSize);
            double[] scaledSpectrumChunk = DSP.Math.Multiply(sampleChunk, windowCoefs);
            double scaleFactor = DSP.Window.ScaleFactor.Signal(windowCoefs);

            // Perform the FFT and convert output (complex numbers) to Magnitude
            Complex[] fftSpectrum = fft.Execute(scaledSpectrumChunk);
            double[] scaledFFTSpectrum = DSPLib.DSP.ConvertComplex.ToMagnitude(fftSpectrum);
            scaledFFTSpectrum = DSP.Math.Multiply(scaledFFTSpectrum, scaleFactor);

            // These 1024 magnitude values correspond (roughly) to a single point in the audio timeline
            float currentSongTime = GetTimeFromIndex(i) * spectrumSampleSize;

            if (!useFrequencyDomainClassification)
                OnsetDetection.PopulateCompleteSpectrumData(new SpectrumData(scaledFFTSpectrum, currentSongTime));
            else
                OnsetDetectionFrequencyClassified.PopulateCompleteSpectrumData(new SpectrumData(scaledFFTSpectrum, currentSongTime));
        }

        Debug.Log("Spectrum Analysis done");
        Debug.Log("Background Thread Completed");

    }

    public int GetIndexFromTime(float curTime)
    {
        //float lengthPerSample = this.clipLength / (float)this.numberOfTotalSamples;

        return Mathf.FloorToInt(curTime * this.sampleRate);
    }

    public float GetTimeFromIndex(int index)
    {
        return ((1f / (float)this.sampleRate) * index);
    }

    public void SaveSongDataToJson()
    {
        if (!useFrequencyDomainClassification)
        {
            string songData = JsonUtility.ToJson(this.OnsetDetection);
            string creditsData = JsonUtility.ToJson(this.audioClipMapper.AudioClipData);

            string filePathMap = SaveSongFolderURL.GetAbsoluteURLSongMap(audioClipMapper.AudioClipData);
            string filePathCredits = SaveSongFolderURL.GetAbsoluteURLSongCredits(audioClipMapper.AudioClipData);
            Debug.Log(filePathMap);

            if (System.IO.File.Exists(filePathMap))
                System.IO.File.Delete(filePathMap);

            if (System.IO.File.Exists(filePathCredits))
                System.IO.File.Delete(filePathCredits);


            System.IO.File.WriteAllText(filePathCredits, creditsData);
            System.IO.File.WriteAllText(filePathMap, songData);
            
            
            Debug.Log("Sauvegarde effectué");
        }
        
    }

}
