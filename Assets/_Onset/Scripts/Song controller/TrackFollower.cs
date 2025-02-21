using PathCreation;
using UnityEngine;

public class TrackFollower : MonoBehaviour
{
    [SerializeField]
    private EndOfPathInstruction endOfPathInstruction;

    private PathCreator pathCreatorToHitPoint;
    private PathCreator pathCreatorToPlanet;
    private float speed;

    private bool hasReachedHitPoint;
    private float distanceTravelled;
    private PathCreator pathCreatorToFollow;

    public void Setup(PathCreator pathCreatorToHitPoint, PathCreator pathCreatorToPlanet, float speed)
    {
        this.pathCreatorToHitPoint = pathCreatorToHitPoint;
        this.pathCreatorToPlanet = pathCreatorToPlanet;
        this.speed = speed;
        distanceTravelled = 0f;
        hasReachedHitPoint = false;

        transform.position = pathCreatorToHitPoint.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        transform.rotation = pathCreatorToHitPoint.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

        pathCreatorToFollow = this.pathCreatorToHitPoint;
    }

   //private void Start()
   //{
   //    Debug.Log($"length {pathCreatorToHitPoint.path.length}");
   //    Debug.Log($"time before end : {pathCreatorToHitPoint.path.length * speed}");
   //
   //}

    private void Update()
    {
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreatorToFollow.path.GetPointAtDistance( distanceTravelled, endOfPathInstruction);
        transform.rotation = pathCreatorToFollow.path.GetRotationAtDistance( distanceTravelled, endOfPathInstruction );

        if(distanceTravelled >= pathCreatorToHitPoint.path.length)
        {
            pathCreatorToFollow = pathCreatorToPlanet;
            distanceTravelled = 0;
            hasReachedHitPoint = true;
        }

        else if(hasReachedHitPoint && distanceTravelled >= pathCreatorToPlanet.path.length)
        {
            this.GetComponent<Note>().PoolingSystem.AddToPool(this.gameObject);
        }
    }
}
