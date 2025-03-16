using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialBubble : MonoBehaviour
{
    TimelineController timeline;
    TextMeshProUGUI text;
    public GameObject child;

    bool starting = true;
    private void Awake()
    {
        timeline = FindObjectOfType<TimelineController>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IdleCoroutine());
    }

    List<string> idleThoughts = new()
    {
        "I wonder what first prize is?",
        "I wonder when my lasagne will be done",
        "Can this hat be dry cleaned?",
        "I hope this stain isn't permanent...",
        "Oh, I love this spell!",
        "Ahh, the sweet scent of decaying monster flesh...",
        "Spells spells spells spells spells spells",
        "I miss my familiar.",
        "What should I order from Portaldash after this?",
        "Oh dear, is that blood on my shoe? Again?!",
        "I'm definitely going to feel that tomorrow...",
        "Fireball...",
        "The arcane arts are truly something to behold.",
        "I'm just going to put this in a random spot.",
        "What if I aimed this one backwards?",
        "I feel like someone is reading my thoughts...",
        "I'm cold.",
        "Oops...",
        "That shopkeeper was hot.",
        "If I get knocked out in the arena I'll have to pay a fee."

    };
    IEnumerator IdleCoroutine()
    {

        while (true)
        {
            if (!child.activeInHierarchy)
            {
                Show(idleThoughts.RandomValue(), 5f);
            }

            yield return new WaitForSeconds(UnityEngine.Random.Range(12f, 20f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (starting && timeline.spellBoxesInTimeline.Count > 0)
        {
            Hide();
            starting = false;
            Action a = () => Show("Even if I lose this round I can try again, as long as I don't die.", 5f);
            a.Delay(1f);
        }
    }

    void Hide()
    {
        child.SetActive(false);
    }

    void Show(string str, float duration)
    {
        text.text = str;
        child.SetActive(true);

        Action hide = Hide;
        hide.Delay(duration);
    }
}
