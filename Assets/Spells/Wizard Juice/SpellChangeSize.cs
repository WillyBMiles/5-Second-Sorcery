using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellChangeSize : SpellDuration
{
    public float multiplier = 1f;
    public int extraHealth;

    public override void Cast()
    {
        base.Cast();
        character.transform.localScale *= multiplier;
        character.health += extraHealth;
    }

    protected override void OnDurationEnd()
    {
        base.OnDurationEnd();
        character.transform.localScale /= multiplier;
    }
}
