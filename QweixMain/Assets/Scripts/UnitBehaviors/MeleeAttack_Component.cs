/****************************************************************************
*
*  File              : MeleeAttack_Component.cs
*  Date Created      : 06/07/2023 
*  Description       : Derived from Attack_Component class
*                      
*                      Object will attack its current target if it is within range
*
*                      Triggers its attack animation and applies damage at
*                      appropriate point during the animation sequence
*
*                      Waits for a predetermined amount of time between attacks
*                      
*  Requirements      : Targeting_Component
*                      Animation_Component
*
*  Programmer(s)     : Gabe Burch, Gavin Cunningham
*  Last Modification : 10/04/2023
*  Additional Notes  : -(10/04/2023) [Gavin] Added "Don't require listener" to sendMessage calls. This is the workaround in leiu of using UnityEvents.
*  External Documentation URL : https://trello.com/c/vd4jnEws/35-meleeattackcomponent
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack_Component : Attack_Component
{
    [SerializeField]
    private float attackDamage;
    
    public override void AnimationTrigger()
    {
        ApplyDamage();
    }

    public void ApplyDamage()
    {
        if (!canAttack) { return; }

        if (targeting_Component.currentTarget != null && canAttack == true)
        {
            attackTarget.SendMessage("TakeDamage", attackDamage, SendMessageOptions.DontRequireReceiver);

            attackState = AttackState.WaitingToFinishAnimation;
            canAttack = false;
        }
    }
}
