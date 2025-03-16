using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTeleport : Spell
{
    public float distance;
    public override void Cast()
    {
        base.Cast();
        playerMovement.transform.position += (Vector3)( distance * angle);
    }
}
