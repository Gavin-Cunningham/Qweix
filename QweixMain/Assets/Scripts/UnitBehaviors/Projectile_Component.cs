/****************************************************************************
*
*  File              : Projectile_Component.cs
*  Date Created      : 06/12/2023
*  Description       : Projectile will travel toward its target and will deal combat damage on contact
*
*                      Flight time refers to how many seconds it will take the projectile to reach its target
*
*                      There are three types of projectiles - straight, low lob, and high lob
*                      
*  Requirements      : Collider
*
*  Programmer(s)     : Gabe Burch, Gavin Cunningham
*  Last Modification : 10/09/2023
*  Additional Notes  : -[Gavin] Need to have projectile check whether the targetGameObject is still there and delete itself if it is already dead.
*                      -(10/04/2023) [Gavin] Added "Don't require listener" to sendMessage calls. This is the workaround in leiu of using UnityEvents.
*                      -Changed damage amount to be fed by RangedAttack_Component to Projectile_Component on projectile. Keeps unit settings on unit.
*                      -(10/09/2023) [Gavin] Added Tooltips to all public and Serialized Fields
*  External Documentation URL : https://trello.com/c/8pkDW1QT/13-projectilecomponent
*****************************************************************************
       (c) Copyright 2022-2023 by Qweix - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Projectile_Component : NetworkBehaviour
{
    [Tooltip("What will the projectile fly towards. Should be set by parent upon spawning. Do not change here.")]
    [SerializeField]
    private GameObject target;

    private Collider2D projectileCollider;

    private float projectileDamage;

    [Tooltip("How long with the projectile be in the air")]
    public float flightTime;
    private float timeElapsed;

    private Vector3 initialPosition;

    [Tooltip("Will the projectile move straight at the target or fly in a low or high arch?")]
    public ProjectileType projectileType;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsHost) { return; }

        projectileCollider = GetComponent<Collider2D>();

        if (projectileCollider == null)
        {
            Debug.Log("Collider not found");
        }

        initialPosition = transform.position;

        timeElapsed = 0f;

        //First float is time till first call, second float is time between calls
        InvokeRepeating("IsTargetActive", 0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsHost) { return; }

        timeElapsed += Time.deltaTime;

        switch (projectileType)
        {
            case ProjectileType.Straight:
                if (target != null)
                {
                    transform.position = Vector3.Lerp(initialPosition, target.transform.position, timeElapsed / flightTime);
                }

                break;


            case ProjectileType.LowLob:

                if (target != null)
                {
                    transform.position = Vector3.Lerp(initialPosition, target.transform.position, timeElapsed / flightTime);

                    Vector3 arcCenter = (initialPosition + target.transform.position) / 2f;
                    arcCenter -= new Vector3(0, Vector3.Distance(initialPosition, target.transform.position), 0);

                    Vector3 initialRelCenter = initialPosition - arcCenter;
                    Vector3 targetRelCenter = target.transform.position - arcCenter;

                    transform.position = Vector3.Slerp(initialRelCenter, targetRelCenter, timeElapsed / flightTime);
                    transform.position += arcCenter;
                }
                break;


            case ProjectileType.HighLob:
                if (target != null)
                {
                    transform.position = Vector3.Lerp(initialPosition, target.transform.position, timeElapsed / flightTime);

                    Vector3 arcCenter = (initialPosition + target.transform.position) / 2f;
                    arcCenter -= new Vector3(0, Vector3.Distance(initialPosition, target.transform.position) / 5, 0);

                    Vector3 initialRelCenter = initialPosition - arcCenter;
                    Vector3 targetRelCenter = target.transform.position - arcCenter;

                    transform.position = Vector3.Slerp(initialRelCenter, targetRelCenter, timeElapsed / flightTime);
                    transform.position += arcCenter;
                }
                break;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
    }

    //Destroys projectile when target is killed before projectile arrives. Called by Invoke Repeating.
    private void IsTargetActive()
    {
        if (target == null || !target.activeInHierarchy)
        {
            GetComponent<NetworkObject>().Despawn(true);
            //Destroy(this.gameObject);
        }
    }

    private void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    private void SetDamage(float damage)
    {
        projectileDamage = damage;
    }

    private void ApplyDamage()
    {
        if(target != null)
        {
            target.SendMessage("TakeDamage", projectileDamage, SendMessageOptions.DontRequireReceiver );
            GetComponent<NetworkObject>().Despawn(true);
            //Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.isTrigger) { return; }

        if (hit.gameObject == target)
        {
            ApplyDamage();
        }
    }

    //This safely engages and disengages the component from the OnUnitDeath event
    private void OnEnable()
    {
        Health_Component.OnUnitDeath += UnitDeath;
    }

    private void OnDisable()
    {
        Health_Component.OnUnitDeath -= UnitDeath;
    }

    //This helps the unit realize its target is dead
    private void UnitDeath(GameObject deadUnit)
    {
        if(target == deadUnit)
        {
            GetComponent<NetworkObject>().Despawn(true);
            //Destroy(this.gameObject);
        }
    }
}

public enum ProjectileType
{
    Straight,
    LowLob,
    HighLob
}
