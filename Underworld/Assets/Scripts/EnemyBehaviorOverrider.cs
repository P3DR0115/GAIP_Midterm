using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorOverrider : MonoBehaviour
{
    // separate lists because try and catch wouldn't work when the 
    // enemies were in the same list with different components to look for.
    public List<GameObject> SeeingEnemyList;
    public List<GameObject> HearingEnemyList;
    public float alertAllCooldown;
    public float nextAlert;
    public bool alertAll;

    private void Awake()
    {
        alertAll = false;
        FindAllEnemies();
    }

    private void FindAllEnemies()
    {
        SeeingEnemyList = new List<GameObject>();
        HearingEnemyList = new List<GameObject>();
        alertAllCooldown = 10f;

        GameObject[] allEntities = FindObjectsOfType<GameObject>();

        foreach (GameObject go in allEntities)
        {
            if (go.tag == "SeeingEnemy")
            {
                SeeingEnemyList.Add(go);
            }
            else if (go.tag == "HearingEnemy")
            {
                HearingEnemyList.Add(go);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckAnyEnemiesChasing();

        AlertAllEnemies();
    }

    private void CheckAnyEnemiesChasing()
    {
        if(Time.time >= nextAlert)
        {
            for (int i = 0; i < SeeingEnemyList.Count; i++)
            {

                if (SeeingEnemyList[i].GetComponentInChildren<SightSense>().currentState == BehaviorState.Chase)
                {
                    alertAll = true;
                    nextAlert = Time.time + alertAllCooldown;
                }
                
            }

            for(int j = 0; j < HearingEnemyList.Count; j++)
            {
                if (HearingEnemyList[j].GetComponentInChildren<HearingSense>().currentState == BehaviorState.Investigate)
                {
                    alertAll = true;
                    nextAlert = Time.time + alertAllCooldown;
                }
            }
        }
        
    }

    private void AlertAllEnemies()
    {
        if (alertAll)
        {
            for (int i = 0; i < SeeingEnemyList.Count; i++)
            {
                SeeingEnemyList[i].GetComponentInChildren<SightSense>().currentState = BehaviorState.Alert;
                
            }

            for (int j = 0; j < HearingEnemyList.Count; j++)
            {
                HearingEnemyList[j].GetComponentInChildren<HearingSense>().currentState = BehaviorState.Alert;
            }
            alertAll = false;
        }
    }
}