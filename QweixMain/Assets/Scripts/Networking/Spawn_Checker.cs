using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Spawn_Checker : MonoBehaviour
{
	[SerializeField] float TowerZone;
	bool SpawnPointChecker(Vector3 spawnPoint, int team)
	{
		GameObject[] Towers;
		bool canSpawn = false;
		//navmeshhit test to see if the location is on navmesh
		NavMeshHit hit;
		if (NavMesh.SamplePosition(spawnPoint, out hit, .1f, NavMesh.AllAreas))
		{
			//the mouse location was on navmesh
			canSpawn = true;
		}
		else
        {
			//the mouse location was not on navmesh
			canSpawn = false;
			return canSpawn;
		}

		//gather an array of objects with the tags Towers and KingTower
		Towers = FindGameObjectsWithTags(new string[] {"Towers","KingTower"});

		//loops to check to see if the spawn location is to close to enemy towers
		foreach(GameObject tower in Towers)
        {
			if(team != tower.GetComponent<Targeting_Component>().teamCheck)
            {
				float distance = Vector3.Distance(tower.transform.position, spawnPoint);
				if (distance < TowerZone)
				{
					canSpawn = false;
					return canSpawn;
				}
			}			
        }

		return canSpawn;
	}


	GameObject[] FindGameObjectsWithTags(params string[] tags)
    {
		var allTowers = new List<GameObject>();

		foreach (string tag in tags)
        {
			allTowers.AddRange(GameObject.FindGameObjectsWithTag(tag).ToList());
        }

		return allTowers.ToArray();
    }


	public void SpawnUnitServerRPC(Vector3 spawnLocation, int team)
    {
		//call spawnpoiontchecker methid with the location that the mouse is
		bool spawnAllowed = SpawnPointChecker(spawnLocation, team);
		//returns a bool of if the mouse is on navmesh or not and calls the clientRPC to pass back the bool result
		SpawnUnitClientRPC(spawnAllowed);
    }
	

	public void SpawnUnitClientRPC(bool spawnAllowed)
	{
		//call something in clientside
	}

}
