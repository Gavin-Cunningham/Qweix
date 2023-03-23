/****************************************************************************
*
*  File              : AnimationControl.cs
*  Date Created      : 03/21/2023 
*  Description       : This script is designed to take care of its things
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
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class AnimationControl : MonoBehaviour
{
    NavMeshAgent agent;
    Animator controller;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<Animator>();
    }

    private void Update()
    {
        FindMoveDirection();
    }

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
}
