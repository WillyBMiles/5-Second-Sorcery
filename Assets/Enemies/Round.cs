using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Round : MonoBehaviour
{
    [Header("Prior to round, except first")]
    public int GoldAmount;
    [HideInInspector]
    public List<Character> enemies;

    public AudioClip prepMusic;
    public AudioClip battleMusic;



    private void Awake()
    {
        enemies.AddRange(GetComponentsInChildren<Character>());
    }

    public bool AllEnemiesKilled()
    {
        return !enemies.Any(c => c != null && !c.dead);
    }
}
