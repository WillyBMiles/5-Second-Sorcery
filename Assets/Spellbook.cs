using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Spellbook : MonoBehaviour
{
    public List<Spell> allSpells;
    public List<Spell> spellsInTimeline;
    public List<Spell> castedSpells;
    Spell castingSpell = null;

    public Spell CurrentSpell => castingSpell;
    public Spell NextSpell => spellsInTimeline.Where(spell => !castedSpells.Contains(spell) && spell != castingSpell).FirstOrDefault();

    public float timeToCast;

    bool hasCasted = false;
    PlayerMovement playerMovement;
    Character character;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        character = playerMovement.GetComponent<Character>();
    }
    private void Update()
    {
        transform.position = character.center.position;
        foreach (var item in allSpells)
        {
            item.transform.position =
                character.center.position;
        }
    }

    public void StartRound()
    {
        castingSpell = null;
        nextSpell = 0f;
        castedSpells.Clear();
        timeToCast = NextSpell == null ? 0f : NextSpell.delayCasting;
    }


    float nextSpell = 0f;
    public void UpdateSpellbook(float timer)
    {
        if (character.dead)
            return;

        if (timer >= nextSpell)
        {
            if (castingSpell != null)
            {
                castingSpell.EndCastSpell();
                castedSpells.Add(castingSpell);
                castingSpell = null;
            }
            var remainingSpells = spellsInTimeline.Where(spell => !castedSpells.Contains(spell));
            if (remainingSpells.Count() > 0)
            {
                castingSpell = remainingSpells.FirstOrDefault();
                castingSpell.StartCastSpell();
                nextSpell += castingSpell.castTime;
                hasCasted = false;
                castingSpell.OnCast += HasCasted;
            }
        }
    }

    public Spell GetShownSpell()
    {
        if (castingSpell == null)
        {
            return NextSpell;
        }

        return hasCasted ? NextSpell : CurrentSpell;
    }

    public void HasCasted()
    {
        hasCasted = true;
        if (NextSpell != null)
            timeToCast = nextSpell + NextSpell.delayCasting;

    }

}
