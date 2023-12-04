/****************************************************************************
*
*  File              : UnitSwap_Component.cs
*  Date Created      : 11/06/2023
*  Description       : This script locks down the unit's movemnet, plays the swap animation
*  and then spawns in the new unit. Should be used with UnitSwapPostInitialization_Component.
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

public class UnitSwap_Component : MonoBehaviour
{
    [Tooltip("What unit will we be swapping this unit to?")]
    [SerializeField] private GameObject newUnitPrefab;

    //Prevents multiple calls by the animator events
    protected bool unitSwapEventCalled = false;

    //Lock out the components that can interfere and then play the swap animation.
    public virtual void PlaySwapAnimation(string swapAnimationName)
    {
        GetComponent<Animation_Component>().enabled = false;
        GetComponent<Movement_Component>().enabled = false;
        GetComponent<NavMeshAgent>().isStopped = true;
        GetComponent<Animator>().Play(swapAnimationName);
    }

    public virtual void UnitSwapEvent()
    {
        if (!unitSwapEventCalled)
        {
            unitSwapEventCalled = true;
            SwapUnits();
        }
    }

    protected void SwapUnits()
    {
        //Spawn in new unit which will replace old one.
        GameObject NewUnit = Instantiate(newUnitPrefab, transform.position, new Quaternion(0, 0, 0, 0));
        //Copy team to new unit.
        NewUnit.GetComponent<Targeting_Component>().teamCheck = GetComponent<Targeting_Component>().teamCheck;

        //Copy over Health percentage from old unit to new.
        Health_Component newUnitHealth = newUnitPrefab.GetComponent<Health_Component>();
        Health_Component oldUnitHealth = GetComponent<Health_Component>();
        float damageAmount = newUnitHealth.maxHealth * (1 - (oldUnitHealth.currentHealth / oldUnitHealth.maxHealth));
        NewUnit.GetComponent<UnitSwapPostInitialize_Component>().damageAmount = damageAmount;


        //Destroy old unit so that the new unit may reign!
        Destroy(gameObject);
    }
}
