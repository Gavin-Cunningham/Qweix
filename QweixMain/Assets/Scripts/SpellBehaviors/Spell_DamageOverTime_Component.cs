using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_DamageOverTime_Component : Spell_BaseTriggered_Component
{
    [SerializeField] private float totalDamage = 1.0f;
    [SerializeField] private float ticksPerSecond = 1.0f;
    [SerializeField] private float spellEffectTime = 5.0f;
    private float spellTimeRemaining;

    protected override void SpellEffect()
    {
        spellTimeRemaining = spellEffectTime;
        InvokeRepeating("DamageInAOE", 0.0f, (1.0f / ticksPerSecond));
    }

    private void DamageInAOE()
    {
        spellTimeRemaining -= (1.0f / ticksPerSecond);
        if (spellTimeRemaining <= 0.0f) { return; }

        float damagePerTick = totalDamage / (spellEffectTime * ticksPerSecond);

        Collider2D[] targetArray = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), radiusAOE);

        if (targetArray == null) { return; }
        foreach (Collider2D collider in targetArray)
        {
            if (collider.isTrigger) { continue; }
            if (collider.transform.GetComponent<Targeting_Component>().teamCheck != teamCheck)
            {
                collider.transform.SendMessage("TakeDamage", damagePerTick, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
