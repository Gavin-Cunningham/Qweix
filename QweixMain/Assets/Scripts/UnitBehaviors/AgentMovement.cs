/****************************************************************************
*
*  File              : AgentMovement.cs
*  Date Created      : 03/20/2023 
*  Description       : This script is a temp system to show and test the Navmesh.
*  It will make the character move around the scene following a mouse click.
*
*  Programmer(s)     : Tim Garfinkel
*  Last Modification : 03/20/2023
*  Additional Notes  : 
*  External Documentation URL :
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour
{

    private Vector3 target;
    NavMeshAgent agent;


    void Awake()
    {
        //prevents agent surface rotaion
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

    }

    void Update()
    {
        SetTargetPosition();
        SetAgentPosition();
    }


    void SetTargetPosition()
    {
        //sets where the agent should move to
        if (Input.GetMouseButtonDown(0))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void SetAgentPosition()
    {
        //sets the target desdination for the agent to move to
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }
}
