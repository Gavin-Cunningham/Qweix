using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitSwapPostInitialize_Component : MonoBehaviour
{
    NavMeshAgent navMeshAgent;

    [SerializeField] bool hasInitialAnimation = false;
    [SerializeField] float initializeTimer = 0.10f;
    private float initializeTimerCountdown;

    [NonSerialized] public float damageAmount;
    private bool initializeUnitCalled = false;

    private void Start()
    {
        initializeTimerCountdown = initializeTimer;
    }

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = true;
    }

    void Update()
    {
        if (!hasInitialAnimation && initializeTimerCountdown < 0.0f)
        {
            InitializeUnit();
        }
        initializeTimerCountdown -= Time.deltaTime;
    }

    public void FinishInitialAnimation()
    {
            InitializeUnit();
    }

    private void InitializeUnit()
    {
        if(initializeUnitCalled) { return; }
        initializeUnitCalled = true;

        navMeshAgent.isStopped = false;

        SendMessage("setTarget", GetComponent<Targeting_Component>().currentTarget, SendMessageOptions.DontRequireReceiver);

        if (damageAmount > 0.0f)
        {
            SendMessage("TakeDamage", damageAmount, SendMessageOptions.DontRequireReceiver);
        }
    }
}
