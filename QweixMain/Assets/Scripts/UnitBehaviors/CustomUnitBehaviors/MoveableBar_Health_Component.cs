using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBar_Health_Component : Health_Component
{
    public Vector2 healthbarPosition;
    private Transform healthbarCanvas;

    protected override void Start()
    {
        base.Start();
        healthbarCanvas = healthBarBorder.transform.parent;
    }
    private void Update()
    {
        healthbarCanvas.localPosition = healthbarPosition;
    }
}
