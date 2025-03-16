using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public GameObject spellBoxPrefab;
    public Spellbook spellBook;

    List<Spell> spellsInShop = new();
    List<SpellBox> spellBoxes = new();
    public Transform parent;
    GameController gc;
    public RectTransform stayWithin;

    GameObject delaySpellBox;
    public Spell delaySpell;
    GameObject testSpellBox;

    public RectTransform leftAnchor;
    public RectTransform rightAnchor;

    private void Awake()
    {
        gc = FindObjectOfType<GameController>();
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (delaySpellBox == null)
        {
            Spell spell = delaySpell.InstantiateSpell();
            spellsInShop.Add(spell);
            SpellBox box = Instantiate(spellBoxPrefab, parent).GetComponent<SpellBox>();
            box.spell = spell;
            spellBoxes.Add(box);
            box.stayWithin = stayWithin;
            box.RandomizeLocation(leftAnchor, rightAnchor);
            delaySpellBox = box.gameObject;
        }

#if UNITY_EDITOR
        
        if (testSpellBox == null && gc.testSpell != null)
        {
            Spell spell = gc.testSpell.InstantiateSpell();
            spellsInShop.Add(spell);
            SpellBox box = Instantiate(spellBoxPrefab, parent).GetComponent<SpellBox>();
            box.spell = spell;
            spellBoxes.Add(box);
            box.stayWithin = stayWithin;
            box.RandomizeLocation(leftAnchor, rightAnchor);
            testSpellBox = box.gameObject;
        }
#endif
    }

    public void PurchaseItem(Spell spell)
    {
        if (!gc.SpendGold(spell.CurrentCost))
        {
            Debug.Log("Can't Afford");
            DragHereToPurcase.instance.FailPurchase();
            //PopupMaster.instance.PopupText("Can't afford that!!", PopupMaster.Color.White);
            return;
        }
        DragHereToPurcase.instance.Purchase();
        PopupMaster.instance.PopupText("-" + spell.CurrentCost, PopupMaster.Color.Gold);
        spellBook.allSpells.Add(spell);
        SpellBox box = spellBoxes.FirstOrDefault(box => box.spell == spell);
        if (box != default)
        {
            GameObject.Destroy(box.gameObject);
            spellBoxes.Remove(box);
            spellsInShop.Remove(spell);
        }
    }

    
    public void GenerateSpells(int amount)
    {
        List<Spell> tempSpells = new();
        FillWithSpells(tempSpells);
        for (int i = 0; i < amount; i++)
        {
            if (tempSpells.Count == 0)
                FillWithSpells(tempSpells);
            Spell baseSpell = tempSpells.RandomValue();
            Spell spell = baseSpell.InstantiateSpell();
            spellsInShop.Add(spell);
            SpellBox box = Instantiate(spellBoxPrefab, parent).GetComponent<SpellBox>();
            box.spell = spell;
            spellBoxes.Add(box);
            box.stayWithin = stayWithin;
            box.RandomizeLocation(leftAnchor,rightAnchor);
            tempSpells.Remove(baseSpell);
        }

    }
    public void ClearSpellsFromShop()
    {
        foreach (var item in spellsInShop)
        {
            Destroy(item.gameObject);
        }
        spellsInShop.Clear();
        foreach (var item in spellBoxes)
        {
            Destroy(item.gameObject);
        }
        spellBoxes.Clear();

    }

    void FillWithSpells(List<Spell> toFill)
    {

        toFill.Add(GetSpellsOfType(Spell.Type.Attack).RandomValue());
        toFill.Add(GetSpellsOfType(Spell.Type.Attack).RandomValue());
        toFill.Add(GetSpellsOfType(Spell.Type.Utility).RandomValue());
        toFill.Add(GetSpellsOfType(Spell.Type.Defense).RandomValue());
    }

    List<Spell> GetSpellsOfType(Spell.Type type)
    {
        var whichSpells = gc.AllSpells.Where(s => s.BaseGoldCost < .5f * gc.Gold);

        if (whichSpells.Count() < 3)
        {
            whichSpells = gc.AllSpells.ToList();
        }

        var list = whichSpells.Where(s => s.type == type);
        if (list.Count() < 1)
        {
            list = gc.AllSpells.ToList();
        }
       
        if (list.Count() == 0)
            list = whichSpells;
        return list.ToList();
    }
}
