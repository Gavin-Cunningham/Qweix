using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

public class DemolitionistPlantBomb_Component : UnitSwap_Component
{
    private Targeting_Component targetingComponent;
    [SerializeField] private float bombTargetDistance = 0.5f;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float bombCountdownTime = 5.0f;
    [SerializeField] private float bombDamage = 5.0f;

    private void Start()
    {
        targetingComponent = GetComponent<Targeting_Component>();

    }

    private void Update()
    {
        if (targetingComponent != null && targetingComponent.targetInRange)
        {
            PlaySwapAnimation("Plant");
        }
    }

    public override void UnitSwapEvent()
    {
        if(!unitSwapEventCalled)
        {
            unitSwapEventCalled = true;
            PlantBomb();
            SwapUnits();
        }
    }

    private void PlantBomb()
    {
        Vector3 targetHeading = targetingComponent.currentTarget.transform.position - transform.position;
        targetHeading = targetHeading.normalized;
        Vector3 bombTarget = transform.position - (targetHeading * -bombTargetDistance);

        GameObject bomb = Instantiate(bombPrefab, bombTarget, new Quaternion(0, 0, 0, 0));
        bomb.GetComponent<NetworkObject>().Spawn(true);
        DemolitionistBomb_Component bombComponent = bomb.GetComponent<DemolitionistBomb_Component>();
        bombComponent.currentTarget = targetingComponent.currentTarget;
        bombComponent.countdownTime = bombCountdownTime;
        bombComponent.bombDamage = bombDamage;
    }
}
