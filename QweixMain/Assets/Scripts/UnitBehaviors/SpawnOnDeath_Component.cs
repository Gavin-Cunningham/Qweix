/****************************************************************************
*
*  File              : SpawnOnDeath_Component.cs
*  Date Created      : 05/31/2023 
*  Description       : Spawns one or more other game objects when the object dies
*
*                      Most objects will have a Corpse prefab that will be spawned with this script, mostly for the death animation
*
*                      Other options to spawn may be other troops or explosions
*                      
*  Requirements      : 
*
*  Programmer(s)     : Gabe Burch
*  Last Modification : 05/31/2023
*  Additional Notes  : 
*  External Documentation URL : https://trello.com/c/dPLmYeCo/12-spawnondeathcomponent
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDeath_Component : MonoBehaviour
{
    public List<GameObject> objectsToSpawn;

    // Start is called before the first frame update
    void Start()
    {
		if (objectsToSpawn == null)
		{
			Debug.Log("Objects to Spawn list not set");
		}
		else
		{
			foreach (GameObject objectToSpawn in objectsToSpawn)
			{
				if (objectToSpawn == null)
				{
					Debug.Log("Object to Spawn value not set");
				}
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Spawn()
	{
		foreach (GameObject objectToSpawn in objectsToSpawn)
		{
			Instantiate(objectToSpawn);
		}
	}
}
