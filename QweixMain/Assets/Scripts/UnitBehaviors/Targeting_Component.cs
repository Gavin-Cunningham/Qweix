/****************************************************************************
*
*  File              : Targeting_Component.cs
*  Date Created      : 06/19/2023 
*  Description       : This Component manages the Targeting Systems of Units
*
*  Programmer(s)     : Tim Garfinkel, Gavin Cunningham
*  Last Modification : 10/09/2023
*  Additional Notes  : -(08/18/2023) [Gavin] Moved all NavMeshAgent logic to Movement component and replaced it with a SendMessage: TargetInRange & TargetLeftRange.
*                      -(08/18/2023) [Gavin] Also added the listener for the OnUnitDeath event from the Health_Component
*                      -(08/30/2023) [Gavin] Moved findTarget to InvokeRepeating to slow down the calls. shortestDist now only gets set to 1000 if there is no target.
*                      -(10/04/2023) [Gavin] Added "Don't require listener" to sendMessage calls. This is the workaround in leiu of using UnityEvents.
*                      -Made the Unit find the enemy KingTower itself. Prevents issues with not knowing it when it needs to.
*                      -(10/09/2023) [Gavin] Added Tooltips to all public and Serialized Fields
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
    [Tooltip("What team is the unit on. Currently 1 is left side, 2 is right side. In multiplayer it will be 1 is host and 2 is client. 0 is enemy to all.")]
    public int teamCheck = 1;
    /*[NonSerialized]*/ public bool targetInRange;

    [Tooltip("Whether unit should make a flying enemy its target. Usually only ranged or flying units can do this.")]
    [SerializeField] bool canTargetFlying;
    [Tooltip("Whether unit should make a ground enemy its target. Should always be true except for melee flying units.")]
    [SerializeField] bool canTargetGround;
    //See my notes in TestTarget as why canTargetBuilding is disabled
    //[SerializeField] bool canTargetBuilding;
    [Tooltip("How far out should the unit be looking for the next target to walk towards.")]
    [SerializeField] float agroRange;
    private GameObject KingTower;

    [Tooltip("What enemy the unit is currently targeting. Set by script, do not change.")]
    public GameObject currentTarget;
    private Collider2D myTrigger;

    public enum UnitType
    {
        isFlying,
		isGround,
		isBuilding,
	}

    [Tooltip("Does this unit fly, walk or is it stationary? isFlying will ignore terrain in future.")]
    [SerializeField] public UnitType myType;

    //Alter the second float in invoke repeatings to change how many seconds between each run of "findTarget"
    void Start()
    {
        targetInRange = false;
        GetKingTower();
        currentTarget = KingTower;
        setTarget(currentTarget);

        InvokeRepeating("findTarget", 2.0f, 0.75f);
        InvokeRepeating("CheckCurrentRangeCT", 2.5f, 0.8f);
        GetMyTrigger();
    }

    void Awake()
    {
        
    }

    //This will need to be fixed when there are more than two players.
    void GetKingTower()
    {
        GameObject[] kingTowers = GameObject.FindGameObjectsWithTag("KingTower");
        foreach (GameObject tower in kingTowers)
        {
            if (testTarget(tower))
            {
                KingTower = tower;
            }
        }

    }

    void Update()
    {
        //findTarget();
    }

    void findTarget()
    {
        float shortestDist;

        if (currentTarget != null)
        {
            shortestDist = Vector3.Distance(currentTarget.transform.position, transform.position);
        }
        else
        {
            shortestDist = 1000.0f;
        }

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
        SendMessage("SetNewTarget", newTarget, SendMessageOptions.DontRequireReceiver);

        CheckCurrentRange(newTarget);
    }

    void GetMyTrigger()
    {
        Collider2D[] myColliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in myColliders)
        {
            if (collider.isTrigger)
            {
                myTrigger = collider;
            }
        }
    }

    //This is to check whether the new target is currently inside the collider so we know whether to stop.
    void CheckCurrentRange(GameObject Target)
    {
        Collider2D targetCollider = null;

        Collider2D[] targetColliders = Target.GetComponents<Collider2D>();
        foreach (Collider2D collider in targetColliders)
        {
            //if its a Trigger, skip to next one
            if (!collider.isTrigger)
            {
                targetCollider = collider;
            }
            
        }

        if(myTrigger != null && targetCollider != null)
        {
            if (myTrigger.IsTouching(targetCollider))
            {
                SendTargetEnterRange();
            }
            else
            {
                SendTargetLeftRange("Check Current Range");
            }
        }

    }

    void CheckCurrentRangeCT()
    {
        if(currentTarget != null)
        {
            CheckCurrentRange(currentTarget);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(currentTarget == null) { return; }
        if(other.isTrigger) { return; }
        if(other == null) { return; }

        if (other == currentTarget.GetComponent<Collider2D>())
        {
            SendTargetEnterRange();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(currentTarget == null) { return;}
        if (other.isTrigger) { return; }
        if (other == null) { return; }

        if (other == currentTarget.GetComponent<Collider2D>())
        {
            SendTargetLeftRange("On Trigger Exit 2D");
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
            SendTargetLeftRange("Unit Death Event");
        }
    }

    private void SendTargetEnterRange()
    {
        targetInRange = true;

        SendMessage("TargetEnterRange", null, SendMessageOptions.DontRequireReceiver);
    }

    private void SendTargetLeftRange(string source)
    {
        targetInRange = false;
        //Debug.Log(source + " on " + this);
        SendMessage("TargetLeftRange", null, SendMessageOptions.DontRequireReceiver);
    }

    private bool testTarget(GameObject enemy)
    {
        Targeting_Component enemyTC = enemy.GetComponent<Targeting_Component>();

        if (enemyTC.teamCheck != teamCheck)
        {
            UnitType enemyType = enemyTC.myType;

            if (canTargetFlying && enemyType == UnitType.isFlying)
                return true;
            if (canTargetGround && enemyType == UnitType.isGround)
                return true;
            //We are assuming for the moment that everything can attack buildings for 2 reasons
            //  1: A game state could occur where units that cannot attack buildings are left with their only target being a tower
            //  2: It really F*@($ it up to not be able to attack the king tower
            //if (canTargetBuilding && enemyType == UnitType.isBuilding)
            //return true;
            if (enemyType == UnitType.isBuilding)
                return true;
        }
        return false;
    }

}
