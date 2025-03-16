using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAddShowOnCast : SpellAdd
{
    public GameObject objectToShow;
    public float duration;
    public bool followPlayer;
    public bool deleteOnRemoveEffect;

    GameObject obj;
    protected override void OnStartCast()
    {
        base.OnStartCast();

        DestroyThingie();
        if (followPlayer)
        {
            obj = Instantiate(objectToShow, transform);
        }
        else
        {
            obj = Instantiate(objectToShow, transform.position, transform.rotation);
        }

        System.Action delete = () =>
        {
            DestroyThingie();
        };
        if (deleteOnRemoveEffect)
        {
            spell.OnRemoveEffect += DestroyThingie;
        }
        delete.Delay(duration);
    }

    void DestroyThingie()
    {
        if (obj == null)
            return;
        if (obj.TryGetComponent<DisconnectParticles>(out DisconnectParticles dis))
        {
            dis.Disconnect();
        }
        Destroy(obj);
        if (deleteOnRemoveEffect)
        {
            spell.OnRemoveEffect -= DestroyThingie;
        }
    }
}
