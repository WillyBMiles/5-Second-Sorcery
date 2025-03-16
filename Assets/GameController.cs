using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] CanvasGroup prepUI;
    [SerializeField] TextMeshProUGUI countdownText;

    [SerializeField] int startingGold;

    [SerializeField] int gold;
    [SerializeField] TextMeshProUGUI goldText;

    public int Gold => gold;
    [SerializeField] int currentRound = 0;
    public int CurrentRound => currentRound;
    [SerializeField]
    float maxTimer = 5f;
    [SerializeField] Transform startingPosition;

    [SerializeField] RectTransform shopUI;
    [SerializeField] float shopOffX;
    [SerializeField] RectTransform timelineUI;
    [SerializeField] float timelineOffX;
    [SerializeField] float UIScrollSpeed;
    [SerializeField] string menuScene;

    [SerializeField] List<Spell> allSpells;
    public Spell testSpell;

    public IReadOnlyList<Spell> AllSpells => allSpells;
    [SerializeField] List<Round> rounds;
    Round activeRound;

    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject dieScreen;
    public GameObject gameOverScreen;
    public GameObject winGameScreen;

    public AudioSource battleMusicSource;

    public AudioSource prepMusicSource;

    public enum EndCondition
    {
        Win,
        Lose,
        Die,
        GameOver,
        WinGame
    }


    public Spellbook spellbook;
    public ShopController shop;
    public TimelineController timeline;

    public bool shopShowing = true;

    bool inMatch = false;
    bool roundRunning = false;
    public bool RoundRunning => roundRunning;

    float roundTimer;
    public float RoundTimer => roundTimer;

    PlayerMovement player;

    public int NumberOfSpellsPerRound(int currentRound) => 7 + currentRound / 2 + (currentRound > 5 ? currentRound / 3 : 0);

    private void Awake()
    {
        Instance = this;
        timeline = FindObjectOfType<TimelineController>();
    }

    // Start is called before the first frame update
    void Start()
    {

        RestartGame();
        player = FindObjectOfType<PlayerMovement>();
    }

    public void RestartGame()
    {
        gold = startingGold;
        currentRound = 0;
        shop.ClearSpellsFromShop();
        shop.GenerateSpells(NumberOfSpellsPerRound(currentRound));
    }

    // Update is called once per frame
    void Update()
    {
        if (inMatch)
        {
            roundTimer += Time.deltaTime;
            if (roundTimer >= maxTimer)
                EndRound();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            ScreenCapture.CaptureScreenshot("Test.png", 2);
        }

        countdownText.text = ((int)(roundTimer > 0f ? 6 - roundTimer : Mathf.Abs(roundTimer))).ToString();
        if (roundTimer < 0 && countdownText.text == "0")
        {
            countdownText.text = "GO!!!";
            spellbook.StartRound();
        }

        roundRunning = inMatch && roundTimer > 0f;

        if (RoundRunning)
        {
            spellbook.UpdateSpellbook(roundTimer);
        }

        if (shopShowing)
        {
            shopUI.anchoredPosition = new Vector2(Mathf.MoveTowards(shopUI.anchoredPosition.x, 0f, UIScrollSpeed * Time.deltaTime), shopUI.anchoredPosition.y);
            timelineUI.anchoredPosition = new Vector2(Mathf.MoveTowards(timelineUI.anchoredPosition.x, timelineOffX, UIScrollSpeed * Time.deltaTime), timelineUI.anchoredPosition.y);
        }
        else
        {
            shopUI.anchoredPosition = new Vector2(Mathf.MoveTowards(shopUI.anchoredPosition.x, shopOffX, UIScrollSpeed * Time.deltaTime), shopUI.anchoredPosition.y);
            timelineUI.anchoredPosition = new Vector2(Mathf.MoveTowards(timelineUI.anchoredPosition.x, 0f, UIScrollSpeed * Time.deltaTime), timelineUI.anchoredPosition.y);
        }

        goldText.text = $"{Gold}G";
    }

    public void StartRound()
    {
        if (inMatch)
            return;
        inMatch = true;
        roundTimer = -4f;
        player.GetComponent<Rigidbody2D>().velocity = new Vector2();
        player.transform.position = startingPosition.transform.position;
        prepUI.DOFade(0f, .5f);

        activeRound = Instantiate(rounds[currentRound]);
        battleMusicSource.clip = rounds[currentRound].battleMusic;
        battleMusicSource.Play();
        prepMusicSource.DOFade(0f, .25f);

    }

    /// <summary>
    /// TODO
    /// </summary>
    EndCondition GetCurrentEndCondition() {
        Character character = player.GetComponent<Character>();
        if (character.dead)
        {
            return EndCondition.Die;
        }

        if (activeRound.AllEnemiesKilled())
        {
            if (currentRound +1 < rounds.Count)
            {
                return EndCondition.Win;
            }
            return EndCondition.WinGame;
        } else
        {
            return EndCondition.Lose;
        }

        //return EndCondition.GameOver;
            

    }

    EndCondition endCondition;
    public void EndRound()
    {
        endCondition = GetCurrentEndCondition();
        bool lost = endCondition switch
        {
            EndCondition.Win => false,
            EndCondition.WinGame => false,
            _ => true,
        };
        if (!lost)
        {
            currentRound++;
            shop.ClearSpellsFromShop();
            shop.GenerateSpells(NumberOfSpellsPerRound(currentRound));
            timeline.ClearTimeline();
            if (currentRound < rounds.Count)
                gold += rounds[currentRound].GoldAmount;
        }

        inMatch = false;
        player.GetComponent<Rigidbody2D>().velocity = new Vector2();

        SwapToShop();


        

        switch (endCondition)
        {
            case EndCondition.Lose:
                loseScreen.SetActive(true);
                break;
            case EndCondition.Die:
                dieScreen.SetActive(true);
                break;
            case EndCondition.Win:
                winScreen.SetActive(true);
                break;
            case EndCondition.GameOver:
                gameOverScreen.SetActive(true);
                break;
            case EndCondition.WinGame:
                winGameScreen.SetActive(true);
                break;
        }
    }


    public void Continue()
    {
        prepUI.DOFade(1f, .5f);
        System.Action a = (() =>
        {
            player.transform.position = startingPosition.transform.position;
            Destroy(activeRound.gameObject);
            Character c = player.GetComponent<Character>();
            c.health = 1;
            c.dead = false;

        });
        a.Delay(.5f);
        loseScreen.SetActive(false);
        dieScreen.SetActive(false);
        winScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        winGameScreen.SetActive(false);

        prepMusicSource.clip = rounds[currentRound].prepMusic;
        prepMusicSource.DOFade(1f, .25f);
        prepMusicSource.Play();

    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(menuScene);
    }

    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            return true;
        }
        return false;
    }

    public void SwapToShop()
    {
        shopShowing = true;
    }
    public void SwapToTimeline()
    {
        shopShowing = false;
        FindObjectOfType<TimelineController>().Setup();
    }
}
