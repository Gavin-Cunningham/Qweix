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
*                      -(12/03/2023) [Gavin] Made TargetLeftRange() virtual to allow inheritors to edit.
*  External Documentation URL :
*****************************************************************************
       (c) Copyright 2022-2023 by Qweix - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

public class Movement_Component : NetworkBehaviour
{

    GameObject currentTarget;
    protected NavMeshAgent agent;
    Vector3 currentDestination;


    void Awake()
    {
        //if (!IsHost) { return; }

        if (gameObject.GetComponent<NavMeshAgent>() != null)
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
        }
        agent.updateRotation = false;
        agent.updateUpAxis = false;

    }

    void Update()
    {
        if (!IsHost) { return; }

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

    protected virtual void TargetLeftRange()
    {
        if (agent.navMeshOwner != null)
        {
            agent.isStopped = false;
        }
    }
}
