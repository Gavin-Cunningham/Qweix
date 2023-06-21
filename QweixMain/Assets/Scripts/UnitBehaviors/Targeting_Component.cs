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
    GameObject KingTower;


    GameObject currentTarget;
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
        gameObject.SendMessage("SetNewTarget", KingTower);
        myType = UnitType.isFlying;
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
            GameObject enemyGO = enemy.transform.parent.gameObject;
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

    void onTriggerEnter2D(Collider other)
    {
        if (other == currentTarget)
        {
            targetInRange = true;
            agent.isStopped = true;
        }
    }

    void onTriggerExit2D(Collider other)
    {
        if (other == currentTarget)
        {
            targetInRange = false;
            agent.isStopped = false;
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