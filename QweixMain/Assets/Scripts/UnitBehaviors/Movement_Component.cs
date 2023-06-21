using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement_Component : MonoBehaviour
{

    GameObject currentTarget;
    NavMeshAgent agent;
    Vector3 currentDestination;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;


    }

    void Update()
    {
        SetTargetPosition();
        SetAgentPosition();
    }

    public void SetNewTarget(GameObject newTarget)
    {
        currentTarget = newTarget;
    }

    void SetTargetPosition()
    {
        if (currentTarget != null)
            currentDestination = currentTarget.transform.position;

    }

    void SetAgentPosition()
    {
        if (currentTarget != null)
            agent.SetDestination(new Vector3(currentDestination.x, currentDestination.y, transform.position.z));

    }
}
