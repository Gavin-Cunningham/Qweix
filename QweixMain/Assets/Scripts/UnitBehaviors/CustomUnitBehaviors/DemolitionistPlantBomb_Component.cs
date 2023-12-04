/****************************************************************************
*
*  File              : DemolitionistPlantBomb_Component.cs
*  Date Created      : 11/22/2023
*  Description       : This extension of UnitSwap_Component creates the demolitionist's
*  bomb and sets it up. It requires the DemolitionistBomb_Component on the bomb prefab.
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

public class DemolitionistPlantBomb_Component : UnitSwap_Component
{
    private Targeting_Component targetingComponent;
    [Tooltip("How far from the character should the bomb be instantiated?")]
    [SerializeField] private float bombTargetDistance = 0.5f;
    [Tooltip("Put the prefab of the bomb here. Should have DemolitionistBomb_Component on it.")]
    [SerializeField] private GameObject bombPrefab;
    [Tooltip("How long after placing should the bomb blow up?")]
    [SerializeField] private float bombCountdownTime = 5.0f;
    [Tooltip("How much damage should the bomb do to its target?")]
    [SerializeField] private float bombDamage = 5.0f;

    private void Start()
    {
        targetingComponent = GetComponent<Targeting_Component>();

    }

    private void Update()
    {
        if (targetingComponent != null && targetingComponent.targetInRange)
        {
            PlaySwapAnimation("Plant");
        }
    }

    public override void UnitSwapEvent()
    {
        //Prevents multiple calls by the animator.
        if(!unitSwapEventCalled)
        {
            unitSwapEventCalled = true;
            PlantBomb();
            SwapUnits();
        }
    }

    private void PlantBomb()
    {
        //Find the spot the bomb should be placed at based of the direction of the target and the bombTargetDistance.
        Vector3 targetHeading = targetingComponent.currentTarget.transform.position - transform.position;
        targetHeading = targetHeading.normalized;
        Vector3 bombTarget = transform.position - (targetHeading * -bombTargetDistance);

        //Place the bomb on the map and set up its target, time remaining and damage from our local variables.
        GameObject bomb = Instantiate(bombPrefab, bombTarget, new Quaternion(0, 0, 0, 0));
        DemolitionistBomb_Component bombComponent = bomb.GetComponent<DemolitionistBomb_Component>();
        bombComponent.currentTarget = targetingComponent.currentTarget;
        bombComponent.countdownTime = bombCountdownTime;
        bombComponent.bombDamage = bombDamage;
    }
}
