using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDuration : Spell
{

    public float duration;

    float currentDuration;

    public override void Cast()
    {
        base.Cast();
        currentDuration = duration;
        StartCoroutine(UpdateCoroutine());
        
    }


    IEnumerator UpdateCoroutine()
    {
        while (currentDuration > 0f)
        {
            UpdateWhileActive();
            yield return new WaitForEndOfFrame();
            currentDuration -= Time.deltaTime;
        }
        OnDurationEnd();
       
    }

    protected virtual void UpdateWhileActive()
    {

    }

    protected virtual void OnDurationEnd()
    {

    }
}
