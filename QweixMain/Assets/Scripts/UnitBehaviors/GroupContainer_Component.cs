/****************************************************************************
*
*  File              : GroupContainer_Component
*  Date Created      : 10/26/2023 
*  Description       : This should go on an empty object which is tied to the card
*  for groups of units that spawn in together.
*
*  Programmer(s)     : 
*  Last Modification : 
*  Additional Notes  : 

*  External Documentation URL :
*****************************************************************************
       (c) Copyright 2022-2023 by Qweix - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GroupContainer_Component : NetworkBehaviour
{
    [SerializeField] List<GameObject> SpawningPrefabs;
    [SerializeField] private float spawnRadius;

    void Start()
    {
        if (!IsHost) { return; }

        Vector3 spawnLocationDiference = new Vector3(spawnRadius, 0, 0);
        Vector3 nextSpawnLocation = transform.position + spawnLocationDiference;

        int groupTeam = GetComponent<Targeting_Component>().teamCheck;
        int numberOfSpawnables = SpawningPrefabs.Count;
        int spawnRotation = 360 / numberOfSpawnables;
       
        foreach (GameObject spawnables in SpawningPrefabs)
        {
            GameObject spawnedUnit = Instantiate(spawnables, nextSpawnLocation, Quaternion.identity);

            if (spawnedUnit.TryGetComponent<Targeting_Component>(out Targeting_Component targeting_Component))
            {
                targeting_Component.teamCheck = groupTeam;
            }

            spawnedUnit.GetComponent<NetworkObject>().Spawn(true);

            spawnLocationDiference = Quaternion.Euler(0, 0, spawnRotation) * spawnLocationDiference;
            nextSpawnLocation = (spawnLocationDiference) + transform.position;

        }
    }
}
