using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAdd : MonoBehaviour
{
    protected Spell spell;
    private void Awake()
    {
        spell = GetComponent<Spell>();
        spell.OnCast += OnCast;
        spell.OnStartCast += OnStartCast;
        spell.OnEndCast += OnEndCast;
    }
   
    protected virtual void OnStartCast()
    {

    }
    protected virtual void OnEndCast()
    {

    }

    protected virtual void OnCast()
    {

    }
}
