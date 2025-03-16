using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimelineController : MonoBehaviour
{
    public SpellBox spellBoxPrefab;
    public Transform parent;

    public List<SpellBox> spellBoxes = new();
    public List<SpellBox> spellBoxesInTimeline = new();

    public Spellbook spellbook;
    public RectTransform leftTimeline;
    public RectTransform rightTimeline;
    public RectTransform stayWithin;
    public RectTransform leftAnchor;
    public RectTransform rightAnchor;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Setup()
    {
        foreach (var box in spellBoxes.ToArray())
        {
            if (!spellbook.allSpells.Contains(box.spell))
            {
                Destroy(box);
                spellBoxes.Remove(box);
            }
        }
        foreach (var spell in spellbook.allSpells.Where(spell => !spellBoxes.Select(box => box.spell).Contains(spell)))
        {
            var box = Instantiate(spellBoxPrefab, parent);
           
            box.stayWithin = stayWithin;
            box.RandomizeLocation(leftAnchor,rightAnchor);
            box.spell = spell;
            spellBoxes.Add(box);
        }
    }

    const float maxTime = 5f;
    float time = 0;
    const float lengthOffset = 5f;
    // Update is called once per frame
    void Update()
    {
        time = 0;
        float sizeOf1Second = (rightTimeline.anchoredPosition.x - leftTimeline.anchoredPosition.x) / maxTime;
        //Arrange boxes
        foreach (var box in spellBoxesInTimeline)
        {
            if (!Input.GetMouseButtonUp(0))
                (box.transform as RectTransform).anchoredPosition = Vector2.Lerp(leftTimeline.anchoredPosition, rightTimeline.anchoredPosition, time / maxTime);
            time += box.spell.castTime;
            box.lengthIndicator.sizeDelta = new Vector2(sizeOf1Second * box.spell.castTime - lengthOffset, 10);
            if (box.spell.castTime < .5f)
            {
                (box.transform as RectTransform).localScale = new Vector3(.5f, .5f, 1f);
                box.lengthIndicator.sizeDelta = box.lengthIndicator.sizeDelta * 2f;
            }
        }
        foreach (var box in spellBoxes.Where(box => !spellBoxesInTimeline.Contains(box)))
        {
            box.lengthIndicator.sizeDelta = new Vector2(0f, box.lengthIndicator.sizeDelta.y);
            (box.transform as RectTransform).localScale = new Vector3(1f, 1f, 1f);
        }

        spellbook.spellsInTimeline = spellBoxesInTimeline.Select(box => box.spell).ToList();
    }

    public void AddToTimeline(SpellBox spellbox)
    {
        if (spellbox.spell.castTime + time > maxTime)
        {
            Debug.Log("Can't add to timeline, too long cast time");
            return;
        }
        if (!spellBoxesInTimeline.Contains(spellbox))
        {
            AimingUI.ShowAimer(spellbox.spell);
        }
        spellBoxesInTimeline.Remove(spellbox);
        int index = spellBoxesInTimeline.IndexOf(
            spellBoxesInTimeline.LastOrDefault(box =>
                (spellbox.transform as RectTransform).anchoredPosition.x > (box.transform as RectTransform).anchoredPosition.x
            )) + 1;

        spellBoxesInTimeline.Insert(index, spellbox);
    }
    public void RemoveFromTimeline(SpellBox spellBox)
    {
        spellBoxesInTimeline.Remove(spellBox);
    }

    public void ClearTimeline()
    {
        spellBoxes.RemoveAll(box => spellBoxesInTimeline.Contains(box));

        spellbook.allSpells.Where(spell => spellBoxesInTimeline.Select(box => box.spell).Contains(spell)).ToList().ForEach(spell =>
        {
            spellbook.allSpells.Remove(spell);
            Destroy(spell.gameObject);
        });
        spellBoxesInTimeline.ForEach(go => Destroy(go.gameObject)); 

        spellBoxesInTimeline.Clear();
        spellbook.spellsInTimeline.Clear();
    }
}
