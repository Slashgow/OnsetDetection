using System;

[Serializable]
public class SpectralFluxInfo 
{
    public float time;
    public double spectralFlux;
    public bool isPeak;
    public bool isStandardized;

    public SpectralFluxInfo(double spectralFlux, float time)
    {
        this.spectralFlux = spectralFlux;
        this.time = time;
    }
}
