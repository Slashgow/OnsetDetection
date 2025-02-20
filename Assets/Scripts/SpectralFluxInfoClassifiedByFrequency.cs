
using System.Collections.Generic;

public class SpectralFluxInfoClassifiedByFrequency 
{
    public FrequencyDomain frequencyDomain;
    public List<SpectralFluxInfo> spectralFluxInfoList;

    public SpectralFluxInfoClassifiedByFrequency(FrequencyDomain frequencyDomain, List<SpectralFluxInfo> spectralFluxInfoList)
    {
        this.frequencyDomain = frequencyDomain;
        this.spectralFluxInfoList = spectralFluxInfoList;
    }
}
