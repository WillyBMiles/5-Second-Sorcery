using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpeedBoost : SpellDuration
{
    public float mult;

    string key => "SPELL_BOOST" + name;
    public override void StartCastSpell()
    {
        base.StartCastSpell();
        character.speedMults[key] = mult;
    }
    protected override void OnDurationEnd()
    {
        base.OnDurationEnd();
        character.speedMults.Remove(key);
    }
}
