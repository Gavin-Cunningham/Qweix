/****************************************************************************
*
*  File              : SpawnAnimation_Component.cs
*  Date Created      : 11/30/2023 
*  Description       : This is a generic component for units which have an
*  initial animation when they spawn. Keeps them from moving during it.
*
*  Programmer(s)     : Gavin Cunningham
*  Last Modification : 
*  Additional Notes  : 

*  External Documentation URL :
*****************************************************************************
       (c) Copyright 2022-2023 by Qweix - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnAnimation_Component : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Start()
    {
        //Make sure the unit doesn't move while animating
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;

        //Which way should they be facing?
        int team = GetComponent<Targeting_Component>().teamCheck;
        Animator animator = GetComponent<Animator>();
        animator.SetFloat("spawnSide", team);
        animator.SetFloat("XMove", ( -1 * ((team * 2) - 3)));
    }

    //Event called at the end of the spawn animation to let the unit move.
    public void SpawnAnimationEvent()
    {
        agent.isStopped = false;
    }
}
