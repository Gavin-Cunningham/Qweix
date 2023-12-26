using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

public class UnitSwap_Component : NetworkBehaviour
{
    [SerializeField] private GameObject newUnitPrefab;

    protected bool unitSwapEventCalled = false;

    public virtual void PlaySwapAnimation(string swapAnimationName)
    {
        GetComponent<Animation_Component>().enabled = false;
        GetComponent<Movement_Component>().enabled = false;
        GetComponent<NavMeshAgent>().isStopped = true;
        GetComponent<Animator>().Play(swapAnimationName);
    }

    public virtual void UnitSwapEvent()
    {
        if (!IsHost) { return; }

        if (!unitSwapEventCalled)
        {
            unitSwapEventCalled = true;
            SwapUnits();
        }
    }

    protected void SwapUnits()
    {
        //Spawn in new unit which will replace old one.
        GameObject NewUnit = Instantiate(newUnitPrefab, transform.position, new Quaternion(0, 0, 0, 0));
        NewUnit.GetComponent<NetworkObject>().Spawn(true);
        //Copy team to new unit.
        NewUnit.GetComponent<Targeting_Component>().teamCheck = GetComponent<Targeting_Component>().teamCheck;

        //Copy over Health percentage from old unit to new.
        Health_Component newUnitHealth = newUnitPrefab.GetComponent<Health_Component>();
        Health_Component oldUnitHealth = GetComponent<Health_Component>();
        float damageAmount = newUnitHealth.maxHealth * (1 - (oldUnitHealth.currentHealth.Value / oldUnitHealth.maxHealth));
        NewUnit.GetComponent<UnitSwapPostInitialize_Component>().damageAmount = damageAmount;


        //Destroy old unit so that the new unit may reign!

        gameObject.GetComponent<NetworkObject>().Despawn(true);
        //Destroy(gameObject);
    }
}
