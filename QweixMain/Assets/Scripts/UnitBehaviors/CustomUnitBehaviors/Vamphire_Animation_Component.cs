/****************************************************************************
*
*  File              : Vamphire_Animation_Component.cs
*  Date Created      : 12/01/2023 
*  Description       : This extends the behavior of the standard Animation_Component to 
*  account for the Vamphire's additional "descending" and "ascending" states.
*
*  Programmer(s)     : Gavin Cunningham
*  Last Modification : 12/01/2023 
*  Additional Notes  : 

*  External Documentation URL :
*****************************************************************************
       (c) Copyright 2022-2023 by Qweix - All Rights Reserved      
****************************************************************************/


using Mono.CSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vamphire_Animation_Component : Animation_Component
{
    //If we haven't dropped down yet, play the drop down animation. Otherwise just attack again.
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

    //If the target left, make sure we rise before we start moving.
    public override void TargetLeftRange()
    {
        if (controller.GetBool("isDropped"))
        {
            controller.SetBool("isDropped", false);
            controller.Play("Rise");
        }

    }

    //If we finished rising then set isStopped to false in the animator. This is an animation event call.
    public void FinishedRising()
    {
        controller.SetBool("isStopped", false);
    }
}
