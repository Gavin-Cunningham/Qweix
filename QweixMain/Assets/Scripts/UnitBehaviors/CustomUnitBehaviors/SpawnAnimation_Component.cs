using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnAnimation_Component : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        int team = GetComponent<Targeting_Component>().teamCheck;

        Animator animator = GetComponent<Animator>();
        animator.SetFloat("spawnSide", team);
        animator.SetFloat("XMove", ( -1 * ((team * 2) - 3)));
    }

    public void SpawnAnimationEvent()
    {
        agent.isStopped = false;
    }
}
