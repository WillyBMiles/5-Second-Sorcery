using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiProjectile : ProjectileSpell
{
    public float spreadDegrees;

    public override void Cast()
    {
        base.Cast();
        CreateProjectile(-spreadDegrees);
        CreateProjectile(spreadDegrees);
    }
}
