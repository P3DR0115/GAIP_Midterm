using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HearingSense : MonoBehaviour
{
    NavMeshAgent agent;
    Pathfinding pathfinding;
    GameObject PlayerObject;
    GameObject ParentObject;
    GameObject IntruderObject;
    Transform lastKnownPlayerPosition;

    private void Awake()
    {
        if (PlayerObject == null)
        {
            PlayerObject = FindObjectOfType<PlayerMovement>().gameObject;
        }

        if (ParentObject == null)
        {
            ParentObject = this.gameObject;
        }

        if (pathfinding == null)
        {
            pathfinding = FindObjectOfType<Pathfinding>();
        }

        agent = GetComponentInParent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //BehaviorDecision();
    }

    //private void BehaviorDecision()
    //{
    //    switch (currentState)
    //    {
    //        case BehaviorState.Idle:
    //            {
    //                // Mainly used as a Transition to patrol.
    //                GeneratePatrolPoint();
    //                patrolUntil = Time.time + patrolTimeoutDuration;
    //                currentState = BehaviorState.Patrol;
    //                break;
    //            }
    //        case BehaviorState.Chase:
    //            {
    //                // Move Towards the player's position.
    //                agent.destination = Player.transform.position;
    //                CheckChaseTime();
    //                break;
    //            }
    //        case BehaviorState.Investigate:
    //            {
    //                // Investigate the player's last known position.
    //                agent.destination = lastKnownPlayerPosition.position;

    //                if (Time.time > investigateUntil || (Mathf.Approximately(this.transform.position.x, lastKnownPlayerPosition.position.x) && Mathf.Approximately(this.transform.position.z, lastKnownPlayerPosition.position.z)))
    //                {
    //                    currentState = BehaviorState.Idle;
    //                }
    //                break;
    //            }
    //        case BehaviorState.Patrol:
    //            {
    //                // Move Towards a randomly generated coordinate.
    //                agent.destination = PatrolPoint;

    //                // If enough time has passed that the AI should've reached the patrol point
    //                // or it is close enough (in case it is unreachable) then change state.
    //                if (Time.time > patrolUntil || (Mathf.Approximately(this.transform.position.x, PatrolPoint.x) && Mathf.Approximately(this.transform.position.z, PatrolPoint.z)))
    //                {
    //                    //startMarker = endMarker = this.gameObject.transform;
    //                    //startMarker.rotation = this.gameObject.transform.rotation;
    //                    //endMarker.rotation = this.gameObject.transform.rotation;
    //                    //endMarker.rotation = Quaternion.LookRotation(this.gameObject.transform.right);


    //                    //endMarker.rotation = new Quaternion(this.gameObject.transform.right.x, this.gameObject.transform.right.y, this.gameObject.transform.right.z, startMarker.rotation.w);
    //                    //journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    //                    //endMarkerjourneyLength = Vector3.Angle(startMarker.position, endMarker.position);

    //                    startTime = Time.time;
    //                    lookAroundUntil = Time.time + lookAroundTimeoutDuration;

    //                    currentState = BehaviorState.Idle;
    //                }

    //                break;
    //            }

    //        case BehaviorState.LookAround:
    //            {
    //                distCovered = (Time.time - startTime) * rotationSpeed;

    //                // Fraction of journey completed = current distance divided by total distance.
    //                fracJourney = distCovered / journeyLength;

    //                // Set our position as a fraction of the distance between the markers.
    //                //this.gameObject.transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
    //                this.gameObject.transform.rotation = Quaternion.RotateTowards(this.gameObject.transform.rotation, endMarker.rotation, rotationSpeed);

    //                if (Time.time > lookAroundUntil)
    //                {
    //                    currentState = BehaviorState.Idle;
    //                }
    //                break;
    //            }
    //    }

    //}

    private void OnTriggerEnter(Collider other)
    {
        IntruderObject = other.gameObject;

        if(IntruderObject.tag == "Player")
        {
            if(PlayerObject.GetComponent<PlayerMovement>().MovementState == PlayerMovementSpeed.Run)
            {
                RotateToTarget(IntruderObject);
                pathfinding.StartPosition = this.gameObject.transform;
                pathfinding.TargetPosition = PlayerObject.transform;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            lastKnownPlayerPosition = PlayerObject.transform;
        }
    }

    private void RotateToTarget(GameObject intruder)
    {
        //IntruderObject = intruder.gameObject;

        Vector3 relativePosition = IntruderObject.transform.position - ParentObject.transform.position;
        relativePosition.y = 0; // Set to zero so that there's no DeltaY, making it only a difference in two dimensions with DeltaX and DeltaZ
        Quaternion targetRotation = new Quaternion();

        targetRotation.y = Mathf.Tan(relativePosition.z / relativePosition.x);

        targetRotation.SetLookRotation(relativePosition);
        ParentObject.transform.rotation = targetRotation; //= Mathf.Tan((relativePosition.z / relativePosition.x));
    }
}
