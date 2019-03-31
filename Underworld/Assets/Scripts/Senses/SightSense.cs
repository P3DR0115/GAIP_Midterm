using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BehaviorState
{
    Idle,
    Patrol,
    Chase,
    Investigate,

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

    public void Awake()
    {
        currentState = BehaviorState.Idle;
        patrolTimeoutDuration = 4.0f;
        chaseTimeoutDuration = 2.5f;
        maxPatrolRange = 10;
        agent = GetComponentInParent<NavMeshAgent>();
        lastKnownPlayerPosition = this.transform; // initially doesn't know where player is.

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
                    GeneratePatrolPoint();
                    patrolUntil = Time.time + patrolTimeoutDuration;
                    currentState = BehaviorState.Patrol;
                    break;
                }
            case BehaviorState.Chase:
                {
                    agent.destination = Player.transform.position;
                    CheckChaseTime();
                    break;
                }
            case BehaviorState.Investigate:
                {
                    agent.destination = lastKnownPlayerPosition.position;

                    currentState = BehaviorState.Patrol;
                    break;
                }
            case BehaviorState.Patrol:
                {
                    agent.destination = PatrolPoint;

                    if (Time.time > patrolUntil || (Mathf.Approximately(this.transform.position.x, PatrolPoint.x) && Mathf.Approximately(this.transform.position.z, PatrolPoint.z)))
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            currentState = BehaviorState.Chase;
            chaseUntil = Time.time + chaseTimeoutDuration;

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            currentState = BehaviorState.Chase;
            chaseUntil = Time.time + chaseTimeoutDuration;

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
