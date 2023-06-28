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
*  Programmer(s)     : Gabe Burch
*  Last Modification : 06/07/2023
*  Additional Notes  : 
*  External Documentation URL : https://trello.com/c/hIyVrf0V/6-attackcomponent
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Component : MonoBehaviour
{
    [SerializeField]
    private protected float attackFrequency;

    [SerializeField]
    private protected float attackCountdown;

    private protected GameObject attackTarget;

    private protected Targeting_Component targeting_Component;
    private protected Animation_Component animation_Component;

    private protected AttackState attackState;

    private protected bool hasDealtDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        targeting_Component = GetComponentInChildren<Targeting_Component>();

        if (targeting_Component == null)
        {
            Debug.Log("Targeting_Component not found");
        }

        animation_Component = GetComponentInChildren<Animation_Component>();

        if (animation_Component == null)
        {
            Debug.Log("Animation_Component not found");
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
                    BeginAttackAnimation();

                    attackState = AttackState.WaitingForAnimationTrigger;
                }

                break;


            case AttackState.WaitingForAnimationTrigger:

                break;


            case AttackState.WaitingToFinishAnimation:

                break;


            case AttackState.CoolingDown:

                attackCountdown -= Time.deltaTime;
                if (attackCountdown <= 0)
                {
                    attackState = AttackState.PursuingTarget;
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

        animation_Component.AnimationFinished();

        attackState = AttackState.CoolingDown;

        hasDealtDamage = false;
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