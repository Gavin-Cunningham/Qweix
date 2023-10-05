/****************************************************************************
*
*  File              : Movement_Component.cs
*  Date Created      : 06/19/2023 
*  Description       : This Component is a go between for behaviours and the NavMeshComponent
*
*  Programmer(s)     : Tim Garfinkel, Gavin Cunningham
*  Last Modification : 10/04/2023
*  Additional Notes  : -(08/18/2023) [Gavin] Added TargetInRange and TargetLeftRange
*                      -(10/04/2023) [Gavin] Managed to finally find the correct Exception check for TargetIn and LeftRange
*  External Documentation URL :
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement_Component : MonoBehaviour
{

    GameObject currentTarget;
    NavMeshAgent agent;
    Vector3 currentDestination;


    void Awake()
    {
        if (gameObject.GetComponent<NavMeshAgent>() != null)
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
        }
        agent.updateRotation = false;
        agent.updateUpAxis = false;

    }

    void Update()
    {
        SetTargetPosition();
        SetAgentPosition();
    }

    public void SetNewTarget(GameObject newTarget)
    {
        currentTarget = newTarget;
    }

    void SetTargetPosition()
    {
        if (currentTarget != null)
            currentDestination = currentTarget.transform.position;

    }

    void SetAgentPosition()
    {
        if (currentTarget != null)
            agent.SetDestination(new Vector3(currentDestination.x, currentDestination.y, transform.position.z));

    }

    void TargetEnterRange()
    {
        if (agent.navMeshOwner != null)
        {
            agent.isStopped = true;
        }
    }

    void TargetLeftRange()
    {
        if (agent.navMeshOwner != null)
        {
            agent.isStopped = false;
        }
    }
}
