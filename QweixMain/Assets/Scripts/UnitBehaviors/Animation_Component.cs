/****************************************************************************
*
*  File              : Animation_Component.cs
*  Date Created      : 03/21/2023 
*  Description       : This script ties the animations from the animator to
*  the movement from the navmesh system.
*
*  Programmer(s)     : Gavin Cunningham
*  Last Modification : 12/03/2023
*  Additional Notes  : -(08/18/2023) [Gavin] Added TargetInRange and TargetLeftRange
*                      -(12/02/2023) [Gavin] Made BeginAttackAnimation() and TargetLeftRange() virtual to allow inheritors to edit.
*  External Documentation URL :
*****************************************************************************
       (c) Copyright 2022-2023 by Qweix - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Animation_Component : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Animator controller;
    private Transform targetTransform;
    private Transform parentTransform;

    //Gets the components necessary to reference
    private void Start()
    {
        if (GetComponent<NavMeshAgent>() != null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        controller = GetComponent<Animator>();
        parentTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (agent != null)
        {
            FindMoveDirection();
        }
        if (targetTransform != null)
        {
            FindTargetDirection();
        }
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
            controller.SetFloat("XMove", xMove);
            controller.SetFloat("YMove", yMove);
        }

    }

    //Called by a send.message from the Targeting_Component
    private void SetNewTarget(GameObject newTarget)
    {
        targetTransform = newTarget.GetComponent<Transform>();
    }

    //This is to pick the right direction for attack animations
    private void FindTargetDirection()
    {
        Vector3 targetVector = targetTransform.position - parentTransform.position;
        Vector3 targetDirection = Vector3.Normalize(targetVector);
        controller.SetFloat("XTarget", targetDirection.x);
        controller.SetFloat("YTarget", targetDirection.y);
    }

    //Called by the Attack_Component its children
    public virtual void BeginAttackAnimation()
    {
        controller.SetBool("isAttacking", true);
        controller.Play("Attack");
    }

    //Called by the animator to let the Attack_Component know the animation is done
    public void AnimationFinished()
    {
            controller.SetBool("isAttacking", false);
    }

    //Called by Targeting_Component with send.message.
    public void TargetEnterRange()
    {
        controller.SetBool("isStopped", true);
    }

    //Called by Targeting_Component with send.message
    public virtual void TargetLeftRange()
    {
        controller.SetBool("isStopped", false);
    }
}
