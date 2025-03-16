using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public int BaseGoldCost;
    public int CurrentCost;
    public float castTime;
    public float delayCasting;
    public Sprite icon;

    public string Title;
    public string Description;
    public Type type;

    protected string genericKey;

    public bool needsToBeAimed = false;

    public Vector2 angle;

    public enum Type
    {
        Defense,
        Utility,
        Attack,
    }

    public Action OnStartCast;
    public Action OnCast;
    public Action OnEndCast;

    public Action OnRemoveEffect;


    protected PlayerMovement playerMovement;
    protected Character character;


    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        character = playerMovement.GetComponent<Character>();
        genericKey = Time.time + "|" + UnityEngine.Random.value + "|" + name;
    }

    public static Color GetColorByType(Type type)
    {
        return type switch
        {
            Type.Attack => new Color(1, .43137f, .29411f),
            Type.Defense => new Color(0.3137254901960784f, 0.788235294117647f, 0.39215686274509803f),
            Type.Utility => new Color(0.00392156862745098f, 0.7098039215686275f, 1),
            _ => new Color()
        };
    }

    private void Start()
    {
        
    }
    protected virtual void Update()
    {
        if (character.dead)
        {
            castDelay.Kill();
        }
    }

    public virtual Spell InstantiateSpell()
    {
        Spell newSpell = Instantiate(this);
        newSpell.CurrentCost = (int) (BaseGoldCost * UnityEngine.Random.Range(.5f, 1.5f));
        return newSpell;
    }

    Sequence castDelay;
    public virtual void StartCastSpell()
    {
        OnStartCast?.Invoke();
        System.Action cast = Cast;
        castDelay = cast.Delay(delayCasting);
    }

    public virtual void Cast()
    {
        OnCast?.Invoke();
    }

    public virtual void EndCastSpell()
    {
        OnEndCast?.Invoke();
    }

}
