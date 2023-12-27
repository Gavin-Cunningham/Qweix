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
*  Last Modification : 10/09/2023
*  Additional Notes  : -(10/04/2023) Added originTransform and changed instantiate to place at transform of parent(the originTransform)
*                      -Added ability to set Team with spawner (how this information gets here will be changed later)
*                      -Added reset of Countdown timer after Spawn() call to stop infinite spawning
*                      -(10/09/2023) [Gavin] Added Tooltips to all public and Serialized Fields
*  External Documentation URL : https://trello.com/c/mpPbmfpc/11-timedspawnercomponent
*****************************************************************************
       (c) Copyright 2022-2023 by Qweix - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Unity.Netcode;

public class TimedSpawner_Component : NetworkBehaviour
{
    [Tooltip("What prefab should this object spawn?")]
    public GameObject objectToSpawn;
    [Tooltip("How many seconds between each prefab spawned?")]
    public float spawnRate;
    private float spawnCountdown;
    [Tooltip("What is the maximum amount spawns this object should do? (ignored if Spawn Forever is checked)")]
    public int maxSpawnCount;
    private int spawnCount;
    [Tooltip("If checked the object will keep spawning prefabs until it is destroyed or otherwise removed.")]
    public bool spawnForever;
    private Transform originTransform;
    [Tooltip("What team should the spawned prefabs take?")]
    public int team;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsHost) { return; }

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
        if (!IsHost) { return; }

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
