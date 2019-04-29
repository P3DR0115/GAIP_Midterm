using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorOverrider : MonoBehaviour
{
    public List<GameObject> EnemyList;
    public bool alertAll;

    private void Awake()
    {
        alertAll = false;
        FindAllEnemies();
    }

    private void FindAllEnemies()
    {
        EnemyList = new List<GameObject>();

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
        for(int i = 0; i < EnemyList.Count; i++)
        {
            try
            {
                if (EnemyList[i].GetComponentInChildren<SightSense>().currentState == BehaviorState.Chase)
                {
                    alertAll = true;
                }
            }
            catch (Exception e)
            {
                try
                {
                    if (EnemyList[i].GetComponentInChildren<GameObject>().GetComponent<HearingSense>().currentState == BehaviorState.Chase)
                    {
                        alertAll = true;
                    }
                }
                catch (Exception ew)
                {
                    // Unexpected sense type?
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
                    EnemyList[i].GetComponent<SightSense>().currentState = BehaviorState.Alert;
                }
                catch (Exception e)
                {
                    try
                    {
                        EnemyList[i].GetComponent<HearingSense>().currentState = BehaviorState.Alert;
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