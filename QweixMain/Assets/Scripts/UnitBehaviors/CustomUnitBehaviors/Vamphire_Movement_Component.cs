/****************************************************************************
*
*  File              : Vamphire_Movement_Component.cs
*  Date Created      : 12/01/2023 
*  Description       : This extends the behavior of the standard Movement_Component to 
*  account for the Vamphire's additional "descending" and "ascending" states.
*
*  Programmer(s)     : Gavin Cunningham
*  Last Modification : 
*  Additional Notes  : 

*  External Documentation URL :
*****************************************************************************
       (c) Copyright 2022-2023 by Qweix - All Rights Reserved      
****************************************************************************/


using NavMeshPlus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vamphire_Movement_Component : Movement_Component
{
    protected override void TargetLeftRange()
    {
        //agent resuming movement is handled by the FinishedRising() animation event call
    }

    //This is an animation event call. Once we are finished rising again, then we can move.
    public void FinishedRising()
    {
        agent.isStopped = false;
    }
}
