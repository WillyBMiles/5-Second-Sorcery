using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AimingUI : MonoBehaviour
{
    static AimingUI instance;
    public TextMeshProUGUI text;
    public RectTransform aimer;
    public Spell spell;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spell == null)
        {
            instance.gameObject.SetActive(false);
            return;
        }
        text.text = "Aim " + spell.Title + "\nLeft click to confirm angle.";
        Vector2 direction = Input.mousePosition - aimer.transform.position;
        spell.angle = direction.normalized;
        aimer.localEulerAngles = new Vector3(0f, 0f, Vector2.SignedAngle(transform.right, direction));

        if (Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
        }
    }

    public static void ShowAimer(Spell spell)
    {
        if (!spell.needsToBeAimed)
        {
            return;
        }
        instance.spell = spell;
        instance.gameObject.SetActive(true);
    }
}
