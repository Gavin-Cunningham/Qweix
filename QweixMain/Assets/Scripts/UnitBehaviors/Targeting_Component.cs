/****************************************************************************
*
*  File              : Targeting_Component.cs
*  Date Created      : 06/19/2023 
*  Description       : This Component manages the Targeting Systems of Units
*
*  Programmer(s)     : Tim Garfinkel
*  Last Modification : 07/03/2023
*  Additional Notes  : 
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
        if (gameObject.GetComponent<NavMeshAgent>() != null)
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
        }
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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == currentTarget.GetComponent<Collider2D>())
        {
            targetInRange = true;

            if (agent != null)
            {
                agent.isStopped = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other == currentTarget.GetComponent<Collider2D>())
        {
            targetInRange = false;

            if (agent != null)
            {
                agent.isStopped = false;
            }
        }
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
