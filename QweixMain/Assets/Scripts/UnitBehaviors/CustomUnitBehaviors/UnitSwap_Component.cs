using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitSwap_Component : MonoBehaviour
{
    [SerializeField] private GameObject newUnitPrefab;

    private bool unitSwapEventCalled = false;

    public virtual void PlaySwapAnimation(string swapAnimationName)
    {
        GetComponent<Animation_Component>().enabled = false;
        GetComponent<Movement_Component>().enabled = false;
        GetComponent<NavMeshAgent>().isStopped = true;
        GetComponent<Animator>().Play(swapAnimationName);
    }

    public virtual void UnitSwapEvent()
    {
        if (!unitSwapEventCalled)
        {
            unitSwapEventCalled = true;

            //Spawn in new unit which will replace old one.
            GameObject NewUnit = Instantiate(newUnitPrefab, transform.position, new Quaternion(0, 0, 0, 0));
            //Copy team to new unit.
            newUnitPrefab.GetComponent<Targeting_Component>().teamCheck = GetComponent<Targeting_Component>().teamCheck;

            //Copy over Health percentage from old unit to new.
            Health_Component newUnitHealth = newUnitPrefab.GetComponent<Health_Component>();
            Health_Component oldUnitHealth = GetComponent<Health_Component>();
            float damageAmount = newUnitHealth.maxHealth * (1 - (oldUnitHealth.currentHealth / oldUnitHealth.maxHealth));
            newUnitPrefab.GetComponent<UnitSwapPostInitialize_Component>().damageAmount = damageAmount;


            //Destroy old unit so that the new unit may reign!
            Destroy(gameObject);
        }
    }
}
