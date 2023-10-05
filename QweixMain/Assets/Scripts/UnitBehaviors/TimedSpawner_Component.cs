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
*  Programmer(s)     : Gabe Burch, Gavin Cunningham
*  Last Modification : 06/12/2023
*  Additional Notes  : -(10/04/2023) Added originTransform and changed instantiate to place at transform of parent(the originTransform)
*                      -Added ability to set Team with spawner (how this information gets here will be changed later)
*                      -Added reset of Countdown timer after Spawn() call to stop infinite spawning
*  External Documentation URL : https://trello.com/c/mpPbmfpc/11-timedspawnercomponent
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TimedSpawner_Component : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnRate;
    private float spawnCountdown;
    public int maxSpawnCount;
    private int spawnCount;
    public bool spawnForever;
    private Transform originTransform;
    public int team;

    // Start is called before the first frame update
    void Start()
    {
        spawnCountdown = spawnRate;

        if (objectToSpawn == null)
        {
            Debug.Log("Object To Spawn not set");
        }

        originTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnForever || (spawnCount <= maxSpawnCount))
        {
            if (spawnCountdown <= 0)
            {
                Spawn();
                spawnCountdown = spawnRate;
            }
            else
            {
                spawnCountdown -= Time.deltaTime;
            }
        }
    }

    private void Spawn()
    {
        GameObject spawnedUnit = Instantiate(objectToSpawn, new Vector3(originTransform.position.x, originTransform.position.y, 0.0f), new Quaternion(0, 0, 0, 0));

        Targeting_Component UnitTC = spawnedUnit.GetComponent<Targeting_Component>();
        if (UnitTC != null)
        {
            UnitTC.teamCheck = team;
        }

        spawnCount++;
    }
}
