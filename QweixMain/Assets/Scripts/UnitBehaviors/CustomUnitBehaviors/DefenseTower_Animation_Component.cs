using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class DefenseTower_Animation_Component : Animation_Component
{
    public override void BeginAttackAnimation()
    {
        controller.SetBool("isAttacking", true);
    }

    protected override void FindMoveDirection()
    {
        return;
    }

    public override void TargetEnterRange()
    {
        return;
    }

    public override void TargetLeftRange()
    {
        return;
    }
}
