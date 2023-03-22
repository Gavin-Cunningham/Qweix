using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class AnimationControl : MonoBehaviour
{
    NavMeshAgent agent;
    Animator controller;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<Animator>();
    }

    private void Update()
    {
        FindMoveDirection();
    }

    private void FindMoveDirection()
    {
        float Xmove = agent.desiredVelocity.x;
        float Ymove = agent.desiredVelocity.y;
        controller.SetFloat("XInput", Xmove);
        controller.SetFloat("YInput", Ymove);
    }
}
