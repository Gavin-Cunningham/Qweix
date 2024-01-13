using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ICG_KingTower_Animation_Component : Animation_Component
{
    public override void BeginAttackAnimation()
    {
        if (!controller.GetBool("isOpen"))
        {
            controller.SetBool("isOpen", true);
            controller.SetBool("isAttacking", true);
            controller.Play("Opening");
        }
        else
        {
            controller.SetBool("isAttacking", true);
            //controller.Play("Attack");
        }
    }

    protected override void FindMoveDirection()
    {
        return;
    }

    public void TargetEnterRange()
    {
        return;
    }

    public void TargetLeftRange()
    {
        return;
    }
}
