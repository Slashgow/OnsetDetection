using PathCreation;
using UnityEngine;

public class Track : MonoBehaviour
{
    [SerializeField]
    private bool autoUpdate;

    [SerializeField]
    private PathCreator pathCreatorToHitNote;
    public PathCreator PathCreatorToHitNote => pathCreatorToHitNote;

    [SerializeField]
    private PathCreator pathCreatorFromHitNoteToPlanet;
    public PathCreator PathCreatorFromHitNoteToPlanet => pathCreatorFromHitNoteToPlanet;

    private void OnValidate()
    {
        if (pathCreatorFromHitNoteToPlanet != null && pathCreatorToHitNote != null)
        {
            PathCreatorFromHitNoteToPlanet.bezierPath.SetPoint(0, pathCreatorToHitNote.transform.InverseTransformPoint(pathCreatorToHitNote.path.GetPoint(pathCreatorToHitNote.path.NumPoints-1)));
        }
    }
}
