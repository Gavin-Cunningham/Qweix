/****************************************************************************
*
*  File              : RangedAttack_Component.cs
*  Date Created      : 06/07/2023
*  Description       : Derived from Attack_Component class
*                      
*                      Object will fire a projectile at its current target if it is within range
*
*                      Triggers its attack animation and fires projectile
*                      at appropriate point during the animation sequence
*
*                      Waits for a predetermined amount of time between attacks
*                      
*  Requirements      : Targeting_Component
*                      Animation_Component
*
*  Programmer(s)     : Gabe Burch
*  Last Modification : 08/17/2023
*  Additional Notes  : -(08/17/23) [Gavin] Added the parentTransform to allow the projectile to spawn on the unit. Added override Start to setup.
*  External Documentation URL : https://trello.com/c/oSgNaDkq/10-rangedattackcomponent
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack_Component : Attack_Component
{
	[SerializeField]
	public GameObject projectile;
    private Transform parentTransform;

    public override void Start()
    {
        parentTransform = GetComponent<Transform>();
    }

    public override void AnimationTrigger()
	{
		FireProjectile();
	}

	private void FireProjectile()
	{
		GameObject proj = Instantiate(projectile, parentTransform);
        //GameObject proj = Instantiate(projectile);

        proj.SendMessage("SetTarget", attackTarget);
	}
}
