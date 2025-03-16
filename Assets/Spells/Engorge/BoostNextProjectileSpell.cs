using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostNextProjectileSpell : SpellDuration
{
    public float sizeMult;
    public int damageAdd;
    public bool mustDoDamage;

    public override void StartCastSpell()
    {
        base.StartCastSpell();
        Projectile.CreateProjectile += CreateProjectile;
    }
    protected override void OnDurationEnd()
    {
        base.OnDurationEnd();
        OnRemoveEffect?.Invoke();
        Projectile.CreateProjectile -= CreateProjectile;
    }


    void CreateProjectile(Projectile projectile)
    {
        if (!projectile.isPlayer)
            return;

        if (mustDoDamage && projectile.damage <= 0)
        {
            return;
        }

        projectile.damage += damageAdd;
        projectile.transform.localScale *= sizeMult;
        OnRemoveEffect?.Invoke();

        Action delay = () => 
        Projectile.CreateProjectile -= CreateProjectile;
        delay.Delay(.15f);
    }
}
