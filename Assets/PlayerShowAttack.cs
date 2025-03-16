using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerShowAttack : MonoBehaviour
{

    public GameObject arrow;
    public GameObject indicator;
    public SpriteRenderer sprite;
    public TextMeshProUGUI text;
    GameController gc;
    Spellbook spellbook;
    private void Awake()
    {
        spellbook = FindObjectOfType<Spellbook>();
        arrow.SetActive(false);
        indicator.SetActive(false);
        gc = FindObjectOfType<GameController>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    Spell spell;
    // Update is called once per frame
    void Update()
    {
        spell = spellbook.GetShownSpell();
        if (spell == null)
        {
            arrow.SetActive(false);
            indicator.SetActive(false);
            sprite.enabled = false;
            text.text = "";
            return;
        }

        float timerText = gc.RoundTimer > 0f ? gc.RoundTimer : 0f;
        text.text = $"{Mathf.Round((spellbook.timeToCast - timerText) * 10) / 10f}";
        sprite.enabled = true;
        sprite.sprite = spell.icon;
        if (spell.needsToBeAimed)
        {
            arrow.transform.eulerAngles = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up, spell.angle));
            arrow.SetActive(true);
            indicator.SetActive(false);
        }
        else
        {
            indicator.SetActive(true);
            arrow.SetActive(false);
        }
        
    }
}
