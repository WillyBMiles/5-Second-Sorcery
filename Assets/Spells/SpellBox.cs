using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellBox : MonoBehaviour
{
    public Spell spell;
    public Image icon;
    public Image outline;
    public TextMeshProUGUI text;
    public TextMeshProUGUI textForCost;
    public bool moveable;

    public static SpellBox hover;
    public static SpellBox hold;
    public bool InShop;
    public static bool InPurchaseArea;
    public static bool InTimelineArea;
    public RectTransform lengthIndicator;
    Image lengthImage;

    public RectTransform stayWithin;
    public Vector2? driftTarget;

    Vector2 currentDriftVelocity;
    public float maxDriftSpeed;
    Vector2 offset;
    Vector2 startPos;

    public Sprite utilitySprite;
    public Sprite defenseSprite;
    public Sprite attackSprite;

    public Image tag;

    public Image aimingArrow;

    List<SpellBox> allboxes = new();

    GameController gc;
    private void Awake()
    {
        gc = FindObjectOfType<GameController>();
        lengthImage = lengthIndicator.GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        allboxes.Add(this);
    }
    void OnDestroy()
    {
        allboxes.Remove(this);
    }

    Vector2 lastpos;
    // Update is called once per frame
    void Update()
    {
        


        if (HoverDetector.IsHovering(gameObject))
        {
            hover = this;
            
        }
        else
        {
            if (hover == this)
            {
                hover = null;
            }
        }

        aimingArrow.gameObject.SetActive(false);
        if (spell != null)
        {
            outline.sprite = spell.type switch
            {
                Spell.Type.Attack => attackSprite,
                Spell.Type.Defense => defenseSprite,
                Spell.Type.Utility => utilitySprite,
                _ => null
            };
            tag.color = Spell.GetColorByType(spell.type) - new Color(0.1f, .1f, .1f, 0f);
            lengthImage.color = Spell.GetColorByType(spell.type);

            icon.sprite = spell.icon;
            text.text = spell.Title;
            if (InShop)
            {
                textForCost.text = spell.CurrentCost + "G";
                tag.gameObject.SetActive(true);
            }

            else
            {
                textForCost.text = "";
                tag.gameObject.SetActive(false);
            }
            aimingArrow.gameObject.SetActive(lengthIndicator.sizeDelta.x > .01f && spell.needsToBeAimed);
            
            aimingArrow.transform.eulerAngles = new Vector3(aimingArrow.transform.eulerAngles.x, aimingArrow.transform.eulerAngles.y,
                Vector2.SignedAngle(transform.up, spell.angle));
                
        }

        if (Input.GetMouseButtonDown(0) && hover == this)
        {
            offset = transform.position - Input.mousePosition;
            hold = this;
            //Debug.Log("Holding");
            transform.SetSiblingIndex(1000);
        }
        if (Input.GetMouseButton(0) && hold == this)
        {
            transform.position = offset + (Vector2) Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (hold == this)
            {
                if (InPurchaseArea && InShop)
                    FindObjectOfType<ShopController>().PurchaseItem(spell);
                if (InTimelineArea && !InShop)
                {
                    FindObjectOfType<TimelineController>().AddToTimeline(this);
                }
                else if (!InShop)
                {
                    FindObjectOfType<TimelineController>().RemoveFromTimeline(this);
                }
                hold = null;
            }
                
        }

        if (gc.shopShowing != InShop)
        {
            if (hold == this)
                hold = null;
            if (hover == this)
                hover = null;
        }

        if (driftTarget.HasValue)
        {
            //else
            //{
            //    this.rectTransform().anchoredPosition = Vector2.SmoothDamp(this.rectTransform().anchoredPosition,
            //        this.rectTransform().anchoredPosition + currentDriftVelocity, ref currentDriftVelocity, .01f, maxDriftSpeed);
            //}
        }

        if (hold != this)
        {
            if (!stayWithin.rect.Overlaps(this.rectTransform().rect))
            {
                currentDriftVelocity = Vector2.Lerp(currentDriftVelocity, (stayWithin.rect.center - this.rectTransform().anchoredPosition).normalized * maxDriftSpeed, Time.deltaTime);
            }
            else
            {
                currentDriftVelocity = Vector2.Lerp(currentDriftVelocity, new Vector2(), Time.deltaTime);
            }
            this.rectTransform().anchoredPosition += currentDriftVelocity * Time.deltaTime;
        }
        else
        {
            currentDriftVelocity = (( this.rectTransform().anchoredPosition - lastpos) / Time.deltaTime);
            if (currentDriftVelocity.magnitude > maxDriftSpeed)
            {
                currentDriftVelocity = currentDriftVelocity.normalized * maxDriftSpeed;
            }
        }
        lastpos = this.rectTransform().anchoredPosition;

        //if (driftTarget == null && )
        //{
        //    allboxes.Where(box => box != this).Where()
        //}
    }

    public void RandomizeLocation(RectTransform left, RectTransform right)
    {
        transform.position = Vector2.Lerp(left.transform.position, right.transform.position, Random.value) + new Vector2(0f, Random.Range(-100, 100));
    }


}
