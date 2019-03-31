using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SightSense : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject Player;
    public Transform lastKnownPlayerPosition;
    //public BoxCollider VisionBox;
    public bool chase; // used to currently chase.
    public bool chasedBefore; // used to determine last known position.
    float chaseTimeoutTimer; // how long the chase is in effect (e.g. chase for 5 seconds)
    public float chaseUntil; // Once the game time exceeds the chaseTimeoutTimer, will stop chasing.

    public void Awake()
    {
        chase = false;
        chasedBefore = false;
        chaseTimeoutTimer = 2.8f;
        agent = GetComponentInParent<NavMeshAgent>();
        lastKnownPlayerPosition = this.transform; // initially doesn't know where player is.
        //VisionBox = GetComponentInChildren<BoxCollider>();
        GameObject[] temp = FindObjectsOfType<GameObject>();

        foreach(GameObject GO in temp)
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
        CheckChaseTime();

        ChaseDecision();

    }

    private void ChaseDecision()
    {
        if (chase)
        {
            chasedBefore = true;
            agent.destination = Player.transform.position;
        }
        else
        {
            agent.destination = lastKnownPlayerPosition.position;
        }
    }

    private void CheckChaseTime()
    {
        if (Time.deltaTime > chaseUntil)
        {
            chase = false;

            if (chasedBefore)
                lastKnownPlayerPosition = Player.transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            chase = true;
            chaseUntil = Time.deltaTime + chaseTimeoutTimer;

        }
    }


}
