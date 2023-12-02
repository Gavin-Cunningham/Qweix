using NavMeshPlus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vamphire_Movement_Component : Movement_Component
{
    protected override void TargetLeftRange()
    {
        //This is purposefully empty
    }

    public void FinishedRising()
    {
        agent.isStopped = false;
    }
}
