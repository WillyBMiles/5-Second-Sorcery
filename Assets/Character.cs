using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Character : MonoBehaviour
{
    public static List<Character> allCharacters = new();

    public bool player;
    public int health;
    public bool dead;

    public float deathDelay;
    public Transform center;

    public float TotalSpeed => speedMults.Select(kvp => kvp.Value).Aggregate(1f, (v1, v2) => v1 * v2);

    public Dictionary<string, float> speedMults = new();
    private void Awake()
    {
        allCharacters.Add(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDestroy()
    {
        allCharacters.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public HashSet<string> invincible = new();
    public Action<DamageInfo> OnHit;
    public void TakeDamage(int amount)
    {
        if (invincible.Count > 0)
        {
            amount = 0;
        }
        DamageInfo damageInfo = new() { amount = amount };

        OnHit?.Invoke(damageInfo);

        if (amount > 0 && !dead)
        {
            Instantiate(GetHit(), center);
        }

        health -= amount;
        if (health <= 0)
        {
            dead = true;
            if (!player)
            {
                System.Action die = () => Destroy(gameObject);
                die.Delay(deathDelay);
            }

        }
    }
    GameObject hit;
    GameObject GetHit()
    {
        if (hit == null)
            hit = (GameObject) Resources.Load("HIT");
        return hit;
    }

    public class DamageInfo
    {
        public int amount;
    }
}
