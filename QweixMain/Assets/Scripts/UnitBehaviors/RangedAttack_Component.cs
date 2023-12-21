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
*  Programmer(s)     : Gabe Burch, Gavin Cunningham
*  Last Modification : 10/09/2023
*  Additional Notes  : -(08/17/23) [Gavin] Added the parentTransform to allow the projectile to spawn on the unit. Added override Start to setup.
*                      -(10/04/2023) [Gavin] Added "Don't require listener" to sendMessage calls. This is the workaround in leiu of using UnityEvents.
*                      -Changed damage amount to be fed by RangedAttack_Component to Projectile_Component on projectile. Keeps unit settings on unit.
*                      -(10/09/2023) [Gavin] Added Tooltips to all public and Serialized Fields
*  External Documentation URL : https://trello.com/c/oSgNaDkq/10-rangedattackcomponent
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class RangedAttack_Component : Attack_Component
{
    [Tooltip("How much damage will the projectile do to the target upon impact?")]
    [SerializeField] 
    private float attackDamage = 1.0f;
    [Tooltip("What prefab projectile will this unit shoot?")]
    [SerializeField] 
    public GameObject projectile;
    private Transform originTransform;

    public override void Start()
    {
        if (!IsHost) { return; }

        originTransform = GetComponent<Transform>();
        base.Start();
    }

    public override void AnimationTrigger()
	{
		FireProjectile();
    }

	private void FireProjectile()
	{
        if (!IsHost) { return; }
        if (!canAttack) { return; }

		GameObject proj = Instantiate(projectile, new Vector3 (originTransform.position.x, originTransform.position.y, 0.0f), new Quaternion (0, 0, 0, 0));
        proj.GetComponent<NetworkObject>().Spawn(true);

        proj.SendMessage("SetTarget", attackTarget, SendMessageOptions.DontRequireReceiver);
        proj.SendMessage("SetDamage", attackDamage, SendMessageOptions.DontRequireReceiver);

        attackState = AttackState.WaitingToFinishAnimation;
        canAttack = false;
    }
}
