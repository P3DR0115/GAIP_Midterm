using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorOverrider : MonoBehaviour
{
    public List<GameObject> EnemyList;
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
        EnemyList = new List<GameObject>();
        alertAllCooldown = 10f;

        GameObject[] allEntities = FindObjectsOfType<GameObject>();

        foreach (GameObject go in allEntities)
        {
            if (go.tag == "Enemy")
            {
                EnemyList.Add(go);
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
            for (int i = 0; i < EnemyList.Count; i++)
            {
                try
                {
                    if (EnemyList[i].GetComponentInChildren<SightSense>() != null)
                    {
                        if (EnemyList[i].GetComponentInChildren<SightSense>().currentState == BehaviorState.Chase)
                        {
                            alertAll = true;
                            nextAlert = Time.time + alertAllCooldown;
                        }
                        else if (EnemyList[i].GetComponentInChildren<HearingSense>().currentState == BehaviorState.Investigate || EnemyList[i].GetComponentInChildren<HearingSense>().currentState == BehaviorState.Chase)
                        {
                            alertAll = true;
                            nextAlert = Time.time + alertAllCooldown;
                        }
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        if (EnemyList[i].GetComponentInChildren<HearingSense>().currentState == BehaviorState.Chase)
                        {
                            alertAll = true;
                            nextAlert = Time.time + alertAllCooldown;
                        }
                    }
                    catch (Exception ew)
                    {
                        // Unexpected sense type?
                    }
                }
            }
        }
        
    }

    private void AlertAllEnemies()
    {
        if (alertAll)
        {
            for (int i = 0; i < EnemyList.Count; i++)
            {
                try
                {
                    EnemyList[i].GetComponentInChildren<SightSense>().currentState = BehaviorState.Alert;
                    EnemyList[i].GetComponentInChildren<HearingSense>().currentState = BehaviorState.Alert;
                }
                catch (Exception e)
                {
                    try
                    {
                        EnemyList[i].GetComponentInChildren<HearingSense>().currentState = BehaviorState.Alert;
                    }
                    catch (Exception ew)
                    {
                        // Unexpected sense type?
                    }
                }
            }
            alertAll = false;
        }
    }
}