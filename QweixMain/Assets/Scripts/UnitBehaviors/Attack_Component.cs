/****************************************************************************
*
*  File              : Attack_Component.cs
*  Date Created      : 05/31/2023 
*  Description       : Base class for RangedAttack_Component and MeleeAttack_Component
*                      
*                      Object will attack its current target if it is within range
*
*                      Triggers its attack animation and applies damage or fires projectile
*                      at appropriate point during the animation sequence
*
*                      Waits for a predetermined amount of time between attacks
*                      
*  Requirements      : Targeting_Component
*                      Animation_Component
*
*  Programmer(s)     : Gabe Burch, Gavin Cunningham
*  Last Modification : 12/03/2023
*  Additional Notes  : -(08/18/2023) [Gavin] Made Start virtual so RangedAttack_Component can override.
*                      -(10/04/2023) [Gavin] Changed GetComponentInChildren to GetComponent. Made attackCountdown private.
*                      -(10/09/2023) [Gavin] Added Tooltips to all public and Serialized Fields
*                      -(12/03/2023) [Gavin] Removed redundant call to Animation_Component.AnimationFinished() (Animator already does this)
*  External Documentation URL : https://trello.com/c/hIyVrf0V/6-attackcomponent
*****************************************************************************
       (c) Copyright 2022-2023 by Qweix - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Component : MonoBehaviour
{
    //The length of the pause between attacks
    [Tooltip("How long should the unit pause between attack animations (in seconds)")]
    [SerializeField]
    private protected float attackFrequency = 1.0f;
    //The countdown variable for attackFrequency
    private protected float attackCountdown;

    private protected GameObject attackTarget;

    private protected Targeting_Component targeting_Component;
    private protected Animation_Component animation_Component;

    [Tooltip("Set by code. Used to see what the current state is while running")]
    [SerializeField] private protected AttackState attackState;
    private protected bool canAttack = true;

    // Start is called before the first frame update
    public virtual void Start()
    {
        targeting_Component = GetComponent<Targeting_Component>();

        if (targeting_Component == null)
        {
            Debug.Log(gameObject.name + "does not have a Targeting_Component");
        }

        animation_Component = GetComponent<Animation_Component>();

        if (animation_Component == null)
        {
            Debug.Log(gameObject.name + " does not have an Animation_Component");
        }

        attackState = AttackState.WaitingForTarget;
    }

    // Update is called once per frame
    void Update()
    {
        switch (attackState)
        {
            case AttackState.WaitingForTarget:

                break;


            case AttackState.PursuingTarget:

                if (targeting_Component.targetInRange)
                {
                    if (animation_Component != null)
                    {
                        BeginAttackAnimation();
                        attackState = AttackState.WaitingForAnimationTrigger;
                    }
                    else
                    {
                        AnimationTrigger();
                        attackCountdown = attackFrequency;
                        attackState = AttackState.CoolingDown;
                    }
                }

                break;


            case AttackState.WaitingForAnimationTrigger:

                break;


            case AttackState.WaitingToFinishAnimation:

                break;


            case AttackState.CoolingDown:

                attackCountdown -= Time.deltaTime;
                if (attackCountdown <= 0.0f)
                {
                    attackState = AttackState.PursuingTarget;
                    canAttack = true;
                }

                break;
        }
    }

    public void SetNewTarget(GameObject newTarget)
    {
        if(newTarget != null)
        {
            attackTarget = newTarget;
            if (attackState != AttackState.CoolingDown)
            {
                attackState = AttackState.PursuingTarget;
            }
        }
    }

    public virtual void AnimationTrigger()
    {
        
    }

    private void BeginAttackAnimation()
    {
        animation_Component.BeginAttackAnimation();

        attackState = AttackState.WaitingForAnimationTrigger;
    }

    public void AnimationFinished()
    {
            attackCountdown = attackFrequency;

            attackState = AttackState.CoolingDown;
    }
}
enum AttackState
{
    WaitingForTarget,
    PursuingTarget,
    WaitingForAnimationTrigger,
    WaitingToFinishAnimation,
    CoolingDown
}