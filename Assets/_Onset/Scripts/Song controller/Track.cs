using PathCreation;
using UnityEngine;

public class Track : MonoBehaviour
{
    [SerializeField]
    private bool autoUpdate;

    [SerializeField]
    private GameObject targetPrefab;

    [SerializeField]
    private PathCreator pathCreatorToHitNote;
    public PathCreator PathCreatorToHitNote => pathCreatorToHitNote;

    [SerializeField]
    private PathCreator pathCreatorFromHitNoteToPlanet;
    public PathCreator PathCreatorFromHitNoteToPlanet => pathCreatorFromHitNoteToPlanet;

    public void InitTarget()
    {
        GameObject targetPrefabInstance = Instantiate(targetPrefab, pathCreatorFromHitNoteToPlanet.transform);
        targetPrefabInstance.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
        targetPrefabInstance.transform.position = PathCreatorFromHitNoteToPlanet.path.GetPoint(0);
      
    }

    private void OnValidate()
    {
        if (pathCreatorFromHitNoteToPlanet != null && pathCreatorToHitNote != null)
        {
            PathCreatorFromHitNoteToPlanet.bezierPath.SetPoint(0, pathCreatorToHitNote.transform.InverseTransformPoint(pathCreatorToHitNote.path.GetPoint(pathCreatorToHitNote.path.NumPoints-1)));
        }
    }
}
