using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemolitionistPlantBomb_Component : UnitSwap_Component
{
    private Targeting_Component targetingComponent;

    private void Start()
    {
        TryGetComponent<Targeting_Component>(out Targeting_Component targetingComponent);
    }

    private void Update()
    {
        if (targetingComponent != null && targetingComponent.targetInRange)
        {
            PlaySwapAnimation("Plant");
        }
    }
}
