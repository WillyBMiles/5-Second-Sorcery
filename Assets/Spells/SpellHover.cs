using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellHover : MonoBehaviour
{
    public GameObject parent;
    bool side = true;

    [SerializeField]
    Image icon;

    [SerializeField]
    TextMeshProUGUI nameText;
    [SerializeField]
    TextMeshProUGUI descriptionText;
    [SerializeField]
    TextMeshProUGUI goldText;
    [SerializeField]
    TextMeshProUGUI timeText;

    [SerializeField]
    int leftX;
    [SerializeField]
    int rightX;
    [SerializeField]
    int middle;

    public Image outline;
    public Sprite attackImage;
    public Sprite defenseImage;
    public Sprite utilityImage;



    // Start is called before the first frame update
    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
    {
        (transform as RectTransform).anchoredPosition = new Vector3(side ? leftX : rightX, (transform as RectTransform).anchoredPosition.y);

        side = Input.mousePosition.x > middle + Screen.width / 2f;
        parent.SetActive(SpellBox.hover != null);
        if (SpellBox.hover != null)
        {
            Spell spell = SpellBox.hover.spell;
            icon.sprite = spell.icon;
            nameText.text = spell.Title;
            descriptionText.text = spell.Description;
            goldText.text = $"{spell.CurrentCost}G";
            timeText.text = $"{spell.castTime} sec";
            outline.sprite = spell.type switch
            {
                Spell.Type.Attack => attackImage,
                Spell.Type.Defense => defenseImage,
                Spell.Type.Utility => utilityImage,
                _ => null,
            };

        }
        
    }

}
