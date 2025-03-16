using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    public TextMeshProUGUI text;
    GameController gc;
    // Start is called before the first frame update
    void Start()
    {
        gc = FindAnyObjectByType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Final Score: " +gc.Gold +" NEW HIGH SCORE (Probably)";
    }
}
