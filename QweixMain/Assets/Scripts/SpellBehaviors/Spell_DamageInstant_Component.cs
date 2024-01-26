using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Spell_DamageInstant_Component : Spell_BaseTriggered_Component
{
    [SerializeField] private float spellDamage = 1.0f;
    protected override void SpellEffect()
    {
        Collider2D[] targetArray = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), radiusAOE);

        if (targetArray == null ) { return; }
        foreach (Collider2D collider in targetArray)
        {
            if (collider.isTrigger) { continue; }
            if (collider.transform.GetComponent<Targeting_Component>().teamCheck != teamCheck)
            {
                collider.transform.SendMessage("TakeDamage", spellDamage, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
