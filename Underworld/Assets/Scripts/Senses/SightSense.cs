using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BehaviorState
{
    Idle, // rotate in place? Scan 90 degrees.
    Patrol,
    Chase,
    Investigate,
    LookAround,

}

public class SightSense : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject Player;
    public Transform lastKnownPlayerPosition;
    public Vector3 PatrolPoint;   
    public int maxPatrolRange; // Range for the random number
    public float patrolTimeoutDuration;
    public float patrolUntil;

    public BehaviorState currentState;
    public float chaseTimeoutDuration; // how long the chase is in effect (e.g. chase for 5 seconds)
    public float chaseUntil; // Once the game time exceeds the chaseTimeoutTimer, will stop chasing.

    public float investigateTimeoutDuration;
    public float investigateUntil;

    public float rotationSpeed;
    public float idleTimeoutDuration;
    public float idleUntil;

    public Transform startMarker;
    public Transform endMarker;
    public float startTime;
    public float lookAroundUntil;
    public float lookAroundTimeoutDuration;
    private float journeyLength;
    float distCovered;
    float fracJourney;

    public void Awake()
    {
        currentState = BehaviorState.Idle;
        patrolTimeoutDuration = 4.0f;
        chaseTimeoutDuration = 2.5f;
        maxPatrolRange = 10;
        investigateTimeoutDuration = 3.0f;
        rotationSpeed = 2.4f;
        idleTimeoutDuration = 4.5f;
        lookAroundTimeoutDuration = 2f;


        agent = GetComponentInParent<NavMeshAgent>();
        lastKnownPlayerPosition = this.transform; // initially doesn't know where player is. Didn't want a null value.

        FindPlayer();
    }

    private void FindPlayer()
    {
        GameObject[] temp = FindObjectsOfType<GameObject>();

        foreach (GameObject GO in temp)
        {
            if (GO.tag == "Player")
            {
                this.Player = GO;
            }

        }
    }

    void Start()
    {
        //agent.destination = Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        BehaviorDecision();

    }

    private void BehaviorDecision()
    {
        switch(currentState)
        {
            case BehaviorState.Idle:
                {
                    // Mainly used as a Transition to patrol.
                    GeneratePatrolPoint();
                    patrolUntil = Time.time + patrolTimeoutDuration;
                    currentState = BehaviorState.Patrol;
                    break;
                }
            case BehaviorState.Chase:
                {
                    // Move Towards the player's position.
                    agent.destination = Player.transform.position;
                    CheckChaseTime();
                    break;
                }
            case BehaviorState.Investigate:
                {
                    // Investigate the player's last known position.
                    agent.destination = lastKnownPlayerPosition.position;

                    if (Time.time > investigateUntil || (Mathf.Approximately(this.transform.position.x, lastKnownPlayerPosition.position.x) && Mathf.Approximately(this.transform.position.z, lastKnownPlayerPosition.position.z)))
                    {
                        currentState = BehaviorState.Idle;
                    }
                    break;
                }
            case BehaviorState.Patrol:
                {
                    // Move Towards a randomly generated coordinate.
                    agent.destination = PatrolPoint;

                    // If enough time has passed that the AI should've reached the patrol point
                    // or it is close enough (in case it is unreachable) then change state.
                    if (Time.time > patrolUntil || (Mathf.Approximately(this.transform.position.x, PatrolPoint.x) && Mathf.Approximately(this.transform.position.z, PatrolPoint.z)))
                    {
                        //startMarker = endMarker = this.gameObject.transform;
                        //startMarker.rotation = this.gameObject.transform.rotation;
                        //endMarker.rotation = this.gameObject.transform.rotation;
                        //endMarker.rotation = Quaternion.LookRotation(this.gameObject.transform.right);
                        
                        
                        //endMarker.rotation = new Quaternion(this.gameObject.transform.right.x, this.gameObject.transform.right.y, this.gameObject.transform.right.z, startMarker.rotation.w);
                        //journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
                        //endMarkerjourneyLength = Vector3.Angle(startMarker.position, endMarker.position);

                        startTime = Time.time;
                        lookAroundUntil = Time.time + lookAroundTimeoutDuration;
                        
                        currentState = BehaviorState.Idle;
                    }

                    break;
                }

            case BehaviorState.LookAround:
                {
                    distCovered = (Time.time - startTime) * rotationSpeed;

                    // Fraction of journey completed = current distance divided by total distance.
                    fracJourney = distCovered / journeyLength;

                    // Set our position as a fraction of the distance between the markers.
                    //this.gameObject.transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
                    this.gameObject.transform.rotation = Quaternion.RotateTowards(this.gameObject.transform.rotation, endMarker.rotation, rotationSpeed);

                    if(Time.time > lookAroundUntil)
                    {
                        currentState = BehaviorState.Idle;
                    }
                    break;
                }
        }
        
    }

    private void CheckChaseTime()
    {
        if (Time.time > chaseUntil)
        {
            lastKnownPlayerPosition = Player.transform;
            investigateUntil = Time.time + investigateTimeoutDuration;
            currentState = BehaviorState.Investigate;

            //if (chasedBefore)
            //    lastKnownPlayerPosition = Player.transform;
        }
    }

    private void GeneratePatrolPoint()
    {
        PatrolPoint = new Vector3();

        System.Random j, k;
        j = new System.Random();
        k = new System.Random();

        PatrolPoint.x = j.Next(maxPatrolRange);
        PatrolPoint.z = k.Next(maxPatrolRange);

        // Switch to determine which quadrant the movement will be towards.
        switch(j.Next(4))
        {
            case 0:
                {
                    // Both Positive
                    break;
                }
            case 1:
                {
                    // x negative
                    PatrolPoint.x *= -1;
                    break;
                }
            case 2:
                {
                    // z negative
                    PatrolPoint.z *= -1;
                    break;
                }
            case 3:
                {
                    // Both Negative
                    PatrolPoint.x *= -1;
                    PatrolPoint.z *= -1;
                    break;
                }
        }
    }

    bool Los, LosF, LosL, LosR; // Line of Sight variables for Forward, Left, Right and overall.

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CheckLineOfSight();

            if(Los)
            {
                currentState = BehaviorState.Chase;
                chaseUntil = Time.time + chaseTimeoutDuration;
            }

        }
    }
    
    private void CheckLineOfSight()
    {
        Quaternion rotation = new Quaternion();
        LosF = LosL = LosR = false;

        rotation.SetLookRotation(Player.transform.position);
        LosF = Physics.Raycast(this.gameObject.transform.position, rotation.eulerAngles, out RaycastHit hitF, 12.375f);

        //if (hitF.rigidbody.gameObject.tag == "Player")
        //    LosF = true;

        rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z - 15);
        LosL = Physics.Raycast(this.gameObject.transform.position, this.gameObject.transform.forward, out RaycastHit hitL, 12.375f);

        rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z + 15);
        LosR = Physics.Raycast(this.gameObject.transform.position, this.gameObject.transform.forward, out RaycastHit hitR, 12.375f);

        

        if (LosF || LosL || LosR)
        {
            Los = true;
        }
        else
        {
            Los = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if(currentState == BehaviorState.Chase)
            {
                chaseUntil = Time.time + chaseTimeoutDuration;
            }
            else
            {
                CheckLineOfSight();
                if (Los)
                {
                    currentState = BehaviorState.Chase;
                    chaseUntil = Time.time + chaseTimeoutDuration;
                }

            }

        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player")
        {
            lastKnownPlayerPosition = Player.transform;
            //currentState = BehaviorState.Investigate;

        }
    }


}
