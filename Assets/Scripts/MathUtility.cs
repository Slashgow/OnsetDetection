using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public static class MathUtility 
{
    public static double GetHalfWaveRectifier(double x)
    {
        return (x + Math.Abs(x)) / 2;
    }

    public static double CalculateAverage(this IEnumerable<double> values)
    {
        return values.Average();
    }

    public static double StandardDeviation(this IEnumerable<double> values)
    {
        double avg = values.Average();
        return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
    }

    public static double CalculateAverage(this List<SpectralFluxInfo> spectralFluxInfos)
    {
        double sum = 0;
        for (int i = 0; i < spectralFluxInfos.Count; i++)
        {
            sum += spectralFluxInfos[i].spectralFlux;
        }
        return sum;
    }

    public static double StandardDeviation(this List<SpectralFluxInfo> spectralFluxInfos, double avg)
    {
        return Math.Sqrt(spectralFluxInfos.Average(v => Math.Pow(v.spectralFlux - avg, 2)));
    }


}
