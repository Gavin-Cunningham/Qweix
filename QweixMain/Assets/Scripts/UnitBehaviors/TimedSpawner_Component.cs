/****************************************************************************
*
*  File              : TimedSpawner_Component.cs
*  Date Created      : 06/12/2023
*  Description       : Object will spawn copies of another particular object at set intervals
*                      
*                      Spawned objects could be troops, mines, obstacles, etc
*
*                      Object can spawn either a set number of times, or continue spawning until it is dead
*
*  Programmer(s)     : Gabe Burch
*  Last Modification : 06/12/2023
*  Additional Notes  : 
*  External Documentation URL : https://trello.com/c/mpPbmfpc/11-timedspawnercomponent
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSpawner_Component : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnRate;
    private float spawnCountdown;
    public int maxSpawnCount;
    private int spawnCount;
    public bool spawnForever;

    // Start is called before the first frame update
    void Start()
    {
        spawnCountdown = spawnRate;

        if (objectToSpawn == null)
        {
            Debug.Log("Object To Spawn not set");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (spawnForever || (spawnCount <= maxSpawnCount))
        {
            if (spawnCountdown <= 0)
            {
                Spawn();
            }
            else
            {
                spawnCountdown -= Time.deltaTime;
            }
        }
    }

    private void Spawn()
    {
        Instantiate(objectToSpawn);
        spawnCount++;
    }
}
