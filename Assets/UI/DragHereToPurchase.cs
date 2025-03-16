using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DragHereToPurcase : MonoBehaviour
{
    public static DragHereToPurcase instance;
    public TextMeshProUGUI text;
    public GameObject speechBubble;
    public Animator animator;
    public AudioSource chaching;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RandomIdleChat());
    }

    // Update is called once per frame
    void Update()
    {
        
        SpellBox.InPurchaseArea = HoverDetector.IsHoveringAll(gameObject);

    }
    IEnumerator RandomIdleChat()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(4f, 15f));
            if (!speechBubble.activeInHierarchy)
            {
                speechBubble.SetActive(true);
                string textAdd = idles.OrderBy(_ => Random.value).FirstOrDefault();
                text.text = textAdd;
                yield return new WaitForSeconds(7f);
                if (text.text == textAdd)
                    speechBubble.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        SpellBox.InPurchaseArea = false;
    }

    List<string> idles = new List<string>() {
        "Your money is welcome!",
        "How do you like my wares?",
        "That one comes with a free coozie.",
        "Buy one get one full price!",
        "We charge window shoppers a fee.",
        "No loitering.",
        "Come here often?",
        "Nice hat!",
        "New spells restocked after every win!",
        "I made these all myself.",
        "Are you gonna kill someone with that one?",
        "I could never quite get that one right.",
        "Don't die, or I'll have to charge you extra!"
    };

    List<string> thanks = new List<string>()
    {
        "Thanks!",
        "Great choice!",
        "No refunds!",
        "You'll love it!",
        "Enjoy!",
        "Another sale!",
        "Yay!"
    };

    List<string> cantAfford = new List<string>() {
        "You can't afford that!",
        "Oops no money.",
        "Not enough? I don't give credit.",
        "Smells like poor.",
        "Your wallet is a bit light.",
        "We don't take pennies. Looks like you're out.",
        "Get a job if you want that one."
    };

    public void Purchase()
    {
        speechBubble.SetActive(true);
        text.text = thanks.OrderBy(_ => Random.value).FirstOrDefault();
        System.Action disable = () => speechBubble.SetActive(false);
        disable.Delay(4f);
        animator.SetTrigger("Dance");
        chaching.Play();
    }
    public void FailPurchase()
    {
        speechBubble.SetActive(true);
        text.text = cantAfford.OrderBy(_ => Random.value).FirstOrDefault();
        System.Action disable = () => speechBubble.SetActive(false);
        disable.Delay(4f);
    }
}
