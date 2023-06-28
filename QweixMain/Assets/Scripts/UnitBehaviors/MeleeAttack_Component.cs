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
*  Programmer(s)     : Gabe Burch
*  Last Modification : 06/07/2023
*  Additional Notes  : 
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
        if (targeting_Component.currentTarget != null && hasDealtDamage == false)
        {
            attackTarget.SendMessage("TakeDamage", attackDamage);

            attackState = AttackState.WaitingToFinishAnimation;

            hasDealtDamage = true;
        }
    }
}
