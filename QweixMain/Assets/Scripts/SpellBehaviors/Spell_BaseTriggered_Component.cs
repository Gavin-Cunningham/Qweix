using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Spell_BaseTriggered_Component : NetworkBehaviour
{
    [SerializeField] private TriggerType trigger = TriggerType.Instant;
    private enum TriggerType
    {
        Instant,
        AnimationEvent,
        Time,
    }

    [SerializeField] private float triggerTime = 1.0f;
    [SerializeField] protected float radiusAOE = 1.5f;
    [NonSerialized] public int teamCheck = 0;

    private void Start()
    {
        if (!IsServer) { return; }
        if (trigger == TriggerType.Instant) { SpellEffect(); }
    }

    private void SpellAnimationEvent()
    {
        if (!IsServer) { return; }
        if (trigger == TriggerType.AnimationEvent) {  SpellEffect(); }
    }

    private void Update()
    {
        if (!IsServer) { return; }
        if (trigger != TriggerType.Time) { return; }

        if (triggerTime !<= 0.0f) { SpellEffect(); }
        triggerTime -= Time.deltaTime;
    }

    protected virtual void SpellEffect()
    {
        return;
    }
}
