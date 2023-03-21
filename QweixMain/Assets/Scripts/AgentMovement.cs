using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour
{

    private Vector3 target;
    NavMeshAgent agent;


    void Awake()
    {
        //prevents agent surface rotaion
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

    }

    void Update()
    {
        SetTargetPosition();
        SetAgentPosition();
    }


    void SetTargetPosition()
    {
        //sets where the agent should move to
        if (Input.GetMouseButtonDown(0))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void SetAgentPosition()
    {
        //sets the target desdination for the agent to move to
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }
}
