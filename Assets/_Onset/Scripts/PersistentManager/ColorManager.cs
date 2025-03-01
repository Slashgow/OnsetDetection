using UnityEngine;

public class ColorManager : PersistentMonoSingleton<ColorManager>
{
    [SerializeField]
    private Color excellentColor, greatColor, earlyColor, lateColor, missColor;

    [SerializeField]
    private Color x1Color, x2Color, x3Color, x4Color, x6Color, x8Color;

    [SerializeField]
    private Color numberOfHitColor;
    public Color NumberOfHitColor => numberOfHitColor;

    public Color GetColorMultiplier(int multiplier)
    {
        switch (multiplier)
        {
            case 1:return x1Color;
            case 2: return x2Color;
            case 3: return x3Color;
            case 4: return x4Color;
            case 6: return x6Color;
            case 8: return x8Color;
            default:
                return x1Color;
        }
    }

    public Color GetColorRated(NoteHitClassification noteHitClassification)
    {
        switch (noteHitClassification)
        {
            case NoteHitClassification.MISS: return missColor;
            case NoteHitClassification.EARLY: return earlyColor;
            case NoteHitClassification.GREAT: return greatColor;
            case NoteHitClassification.LATE: return lateColor;
            case NoteHitClassification.EXCELLENT: return excellentColor;
            default : return missColor;
        }
    }

}
