/****************************************************************************
*
*  File              : Animation_Component.cs
*  Date Created      : 03/21/2023 
*  Description       : This script ties the animations from the animator to
*  the movement from the navmesh system.
*
*  Programmer(s)     : Gavin Cunningham
*  Last Modification : 03/23/2023
*  Additional Notes  : 
*  External Documentation URL :
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animation_Component : MonoBehaviour
{
    NavMeshAgent agent;
    Animator controller;

    //Gets the components necessary to reference
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<Animator>();
    }

    private void Update()
    {
        FindMoveDirection();
    }

    //Tell the animator how fast the agent is moving and what direction.
    //It doesn't update direction if speed under 0.1 so idle stances
    //don't get confused with a direction of 0, 0.
    private void FindMoveDirection()
    {
        float speed = agent.desiredVelocity.magnitude;
        float xMove = agent.desiredVelocity.x;
        float yMove = agent.desiredVelocity.y;
        controller.SetFloat("Speed", speed);
        if (speed >= 0.1f)
        {
            controller.SetFloat("XInput", xMove);
            controller.SetFloat("YInput", yMove);
        }

    }

    public void BeginAttackAnimation()
    {

    }
}
