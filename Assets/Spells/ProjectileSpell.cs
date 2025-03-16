using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpell : Spell
{
    public Projectile projectile;
   
    public bool spawnAtFeet = false;
    public bool playerFollowProjectile;
    public bool makePlayerInvincible;
    public float spawnOffset;
    public bool realign;



    public override void Cast()
    {
        base.Cast();
        CreateProjectile(0f);

    }
    Projectile p;
    protected void CreateProjectile(float extraAngle)
    {
        Vector3 location = transform.position;
        if (spawnAtFeet)
        {
            location = character.transform.position;
        }
        p = Instantiate(projectile, location, Quaternion.LookRotation(transform.forward, angle));
        p.transform.eulerAngles += new Vector3(0f, 0f, extraAngle);
        p.transform.position += spawnOffset * transform.up;
        if (realign)
        {
            p.transform.up = Vector3.up;
        }
    }

    private void LateUpdate()
    {
        if (p != null && playerFollowProjectile)
        {
            Vector2 offset = new();
            if (!spawnAtFeet)
            {
                offset = character.transform.position - character.center.position;
            }
            character.transform.position = p.transform.position + (Vector3) offset;
            character.GetComponent<Rigidbody2D>().velocity = new Vector2();
        }
        if (p != null && makePlayerInvincible)
        {
            character.invincible.Add(genericKey);
        }
        else
        {
            character.invincible.Remove(genericKey);
        }
    }

}
