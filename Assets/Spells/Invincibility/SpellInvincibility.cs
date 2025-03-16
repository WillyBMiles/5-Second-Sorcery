using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellInvincibility : SpellDuration
{
    public override void StartCastSpell()
    {
        base.StartCastSpell();
        character.invincible.Add(genericKey);
    }


    protected override void OnDurationEnd()
    {
        base.OnDurationEnd();
        character.invincible.Remove(genericKey);
    }
}
