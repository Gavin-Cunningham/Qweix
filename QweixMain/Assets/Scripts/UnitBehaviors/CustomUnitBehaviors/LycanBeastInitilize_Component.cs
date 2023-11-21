using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LycanBeastInitilize_Component : MonoBehaviour
{
    NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = true;
    }

    public void FinishInitialAnimation()
    {
        navMeshAgent.isStopped = false;
    }
}
