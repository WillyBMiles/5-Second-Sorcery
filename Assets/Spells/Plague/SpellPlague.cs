using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPlague : Spell
{
    public GameObject prefab;
    public float deleteAfter;
    public override void Cast()
    {
        base.Cast();
        GameObject go = Instantiate(prefab);
        System.Action delete = () =>
        {
            Destroy(go);
        };
        delete.Delay(deleteAfter);

        foreach (Character c in Character.allCharacters)
        {
            if (!c.player)
            {
                c.TakeDamage(1);
            }
        }
    }

}
