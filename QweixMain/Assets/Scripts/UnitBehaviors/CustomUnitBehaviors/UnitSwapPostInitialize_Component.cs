/****************************************************************************
*
*  File              : UnitSwapPostInitialize_Component.cs
*  Date Created      : 11/06/2023 
*  Description       : This script handles setting up the new object when swapping out one
*  unit prefab for another. Should be used with UnitSwap_Component or an extension of it.
*
*  Programmer(s)     : Gavin Cunningham
*  Last Modification : 
*  Additional Notes  : 

*  External Documentation URL :
*****************************************************************************
       (c) Copyright 2022-2023 by Qweix - All Rights Reserved      
****************************************************************************/



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitSwapPostInitialize_Component : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    [Tooltip("Does the unit have an animation that plays before it does anything else?")]
    [SerializeField] bool hasInitialAnimation = false;
    [Tooltip("If there is no initial animation, this give the unit a moment for scripts to run before things are set")]
    [SerializeField] float initializeTimer = 0.10f;

    //How much damage (percentage wise) did the previous unit take?
    /*[NonSerialized]*/ public float damageAmount;

    private float initializeTimerCountdown;
    private bool initializeUnitCalled = false;

    private void Start()
    {
        initializeTimerCountdown = initializeTimer;
    }

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = true;
    }

    //Run the timer if there is no animation to delay it.
    void Update()
    {
        if (!hasInitialAnimation && initializeTimerCountdown < 0.0f)
        {
            InitializeUnit();
        }
        initializeTimerCountdown -= Time.deltaTime;
    }

    //Called by an animation event if there is an initial animation.
    public void FinishInitialAnimation()
    {
        if (!hasInitialAnimation) { return; }
        InitializeUnit();
    }


    private void InitializeUnit()
    {
        //only call this once
        if(initializeUnitCalled) { return; }
        initializeUnitCalled = true;

        //take away the health damage (by percent) that the previous version of this unit took.
        if (damageAmount > 0.0f)
        {
            SendMessage("TakeDamage", damageAmount, SendMessageOptions.DontRequireReceiver);
        }

        //Allow the unit to move now
        navMeshAgent.isStopped = false;

        //Make sure the targeting_component has a target (in case the first target was killed while we were finishing our animation
        SendMessage("findTarget", GetComponent<Targeting_Component>().currentTarget, SendMessageOptions.DontRequireReceiver);
    }
}
