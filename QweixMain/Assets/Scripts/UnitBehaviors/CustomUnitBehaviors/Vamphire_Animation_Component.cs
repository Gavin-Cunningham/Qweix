using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vamphire_Animation_Component : Animation_Component
{
    public override void BeginAttackAnimation()
    {
        if (!controller.GetBool("isDropped"))
        {
            controller.SetBool("isDropped", true);
            controller.SetBool("isAttacking", true);
            controller.Play("Drop");
        }
        else
        {
            controller.SetBool("isAttacking", true);
            //controller.Play("Attack");
        }
    }

    public override void TargetLeftRange()
    {
        if (controller.GetBool("isDropped"))
        {
            controller.SetBool("isDropped", false);
            controller.Play("Rise");
        }

    }

    public void FinishedRising()
    {
        controller.SetBool("isStopped", false);
    }
}
