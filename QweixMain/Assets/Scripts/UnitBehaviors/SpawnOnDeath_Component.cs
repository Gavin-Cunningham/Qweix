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
*  Programmer(s)     : Gabe Burch, Gavin Cunningham
*  Last Modification : 10/09/2023
*  Additional Notes  : -(10/04/2023) [Gavin] Added originTransform and changed instantiate to place at transform of parent(the originTransform)
*					   -(10/09/2023) [Gavin] Added Tooltips to all public and Serialized Fields
*  External Documentation URL : https://trello.com/c/dPLmYeCo/12-spawnondeathcomponent
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnOnDeath_Component : MonoBehaviour
{
	[Tooltip("Prefabs which the unit will spawn at its position upon death. Does not pass the prefabs any information, such as team or damage.")]
    public List<GameObject> objectsToSpawn;
	private Transform originTransform;

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

        originTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Spawn()
	{
		foreach (GameObject objectToSpawn in objectsToSpawn)
        {
            Instantiate(objectToSpawn, new Vector3(originTransform.position.x, originTransform.position.y, 0.0f), new Quaternion(0, 0, 0, 0));
        }
	}
}
