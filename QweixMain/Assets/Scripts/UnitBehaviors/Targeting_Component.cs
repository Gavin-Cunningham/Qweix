/****************************************************************************
*
*  File              : Targeting_Component.cs
*  Date Created      : 06/19/2023 
*  Description       : This Component manages the Targeting Systems of Units
*
*  Programmer(s)     : Tim Garfinkel
*  Last Modification : 08/18/2023
*  Additional Notes  : -(08/18/2023) [Gavin] Moved all NavMeshAgent logic to Movement component and replaced it with a SendMessage: TargetInRange & TargetLeftRange.
*                      -(08/18/2023) [Gavin] Also added the listener for the OnUnitDeath event from the Health_Component
*  External Documentation URL :
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Targeting_Component : MonoBehaviour
{
    //Int value 1 for player 1 and 2 for player 2
    public int teamCheck = 1;
    [NonSerialized] public bool targetInRange;


    [SerializeField] bool canTargetFlying;
    [SerializeField] bool canTargetGround;
    [SerializeField] bool canTargetBuilding;
    [SerializeField] float agroRange;
    public GameObject KingTower;


    public GameObject currentTarget;
    NavMeshAgent agent;

    public enum UnitType
    {
        isFlying,
		isGround,
		isBuilding,
	}

    [SerializeField] public UnitType myType;


    void Awake()
    {
        targetInRange = false;
        currentTarget = KingTower;
        setTarget(currentTarget);
    }

    void Update()
    {
        findTarget();
    }

    void findTarget()
    {
        float shortestDist = 1000.0f;
        GameObject closestTarget = currentTarget;

        Collider2D[] targetArray = Physics2D.OverlapCircleAll(transform.position, agroRange);

        foreach (Collider2D enemy in targetArray)
        {
            GameObject enemyGO = enemy.transform.gameObject;

            if (enemyGO.GetComponent<Targeting_Component>() == null)
            {
                continue;
            }

            if (testTarget(enemyGO) == true)
            {

                float distCheck = Vector3.Distance(enemy.transform.position, transform.position);

                if (distCheck < shortestDist)
                {
                    shortestDist = distCheck;
                    closestTarget = enemyGO;
                }
            }

            if (closestTarget != currentTarget)
            {
                currentTarget = closestTarget;
                setTarget(currentTarget);
            }
        }
    }


    void setTarget(GameObject newTarget)
    {
        gameObject.SendMessage("SetNewTarget", newTarget);

        CheckCurrentRange(newTarget);
    }

    //This is to check whether the new target is currently inside the collider so we know whether to stop.
    void CheckCurrentRange(GameObject Target)
    {
        Collider2D targetCollider = null;
        Collider2D myTrigger = null;

        Collider2D[] targetColliders = Target.GetComponents<Collider2D>();
        foreach (Collider2D collider in targetColliders)
        {
            //if its a Trigger, skip to next one
            if (collider.isTrigger) { continue; }

            targetCollider = collider;
        }

        Collider2D[] myColliders = this.GetComponents<Collider2D>();
        foreach (Collider2D collider in myColliders)
        {
            //if its not a Trigger, skip to next one
            if (!collider.isTrigger) { continue; }

            myTrigger = collider;
        }

        if(myTrigger != null && targetCollider != null)
        {
            if (myTrigger.IsTouching(targetCollider))
            {
                SendTargetEnterRange();
            }
            else
            {
                SendTargetLeftRange();
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.isTrigger) { return; }
        if(other == null) { return; }

        if (other == currentTarget.GetComponent<Collider2D>())
        {
            SendTargetEnterRange();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger) { return; }
        if (other == null) { return; }

        if (other == currentTarget.GetComponent<Collider2D>())
        {
            SendTargetLeftRange();
        }
    }

    //This safely engages and disengages the component from the OnUnitDeath event
    private void OnEnable()
    {
        Health_Component.OnUnitDeath += UnitDeath;
    }

    private void OnDisable()
    {
        Health_Component.OnUnitDeath -= UnitDeath;
    }

    //This helps the unit realize its target is dead
    private void UnitDeath(GameObject deadUnit)
    {
        if (deadUnit = currentTarget)
        {
            SendTargetLeftRange();
        }
    }

    private void SendTargetEnterRange()
    {
        targetInRange = true;

        SendMessage("TargetEnterRange");
    }

    private void SendTargetLeftRange()
    {
        targetInRange = false;

        SendMessage("TargetLeftRange");
    }

    private bool testTarget(GameObject enemy)
    {
        if (enemy.GetComponent<Targeting_Component>().teamCheck != teamCheck)
        {
            UnitType enemyTC = enemy.GetComponent<Targeting_Component>().myType;

            if (canTargetFlying && enemyTC == UnitType.isFlying)
                return true;
            if (canTargetGround && enemyTC == UnitType.isGround)
                return true;
            if (canTargetBuilding && enemyTC == UnitType.isBuilding)
                return true;
        }
        return false;
    }

}
