using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public bool isPlayer;
    public int damage = 1;
    public float MaxTime = 5f;
    public float reapplyDelay = 5f;

    public bool destroyOnImpactCharacters = true;
    public bool destroyOnImpactWall = true;

    public GameObject createOnDestroy;

    DisconnectParticles disconnectParticles;

    public float slowsEnemiesOnHit = 1f;
    public float slowDuration = 0f;

    public float knockBack;

    public static System.Action<Projectile> CreateProjectile;
    public bool rightMyself;


    string key;
    private void Awake()
    {
        key = UnityEngine.Random.value + name;
        disconnectParticles = GetComponent<DisconnectParticles>();
        
    }
    private void Start()
    {
        CreateProjectile?.Invoke(this);
    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;

        MaxTime -= Time.deltaTime;
        if (MaxTime <= 0f)
        {
            DoDestroy();
        }
        if (rightMyself)
        {
            transform.up = Vector3.up;
        }

    }
    bool done = false;
    private void LateUpdate()
    {
        foreach (Collider2D collision in collisions.ToArray())
        {
            if (collision == null)
                continue;
            if (done)
                return;
            if (collision.TryGetComponent<Character>(out var c))
            {
                if (c.player != isPlayer && !hits.Contains(c))
                {
                    c.TakeDamage(damage);

                    if (slowsEnemiesOnHit != 1f && slowDuration > 0f)
                    {
                        c.speedMults.Add(key, slowsEnemiesOnHit);
                        System.Action undo = () => c.speedMults.Remove(key);
                        undo.Delay(slowDuration);
                    }

                    if (knockBack > 0f)
                    {
                        c.transform.position += Vector3.up * knockBack;
                    }
                    Strike();
                    hits.Add(c);
                    Action allowHit = () => hits.Remove(c);
                    allowHit.Delay(reapplyDelay);
                }
            }
            else
            {
                var w = collision.GetComponent<Wall>();
                if (w)
                {
                    StrikeWall();
                }
            }
        }

    }

    List<Character> hits = new();
    List<Collider2D> collisions = new();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collisions.Add(collision);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collisions.Remove(collision);
    }

    void Strike()
    {
        if (destroyOnImpactCharacters)
            DoDestroy();
    }

    void StrikeWall()
    {
        if (destroyOnImpactWall)
            DoDestroy();
    }

    public void DoDestroy()
    {
        done = true;
        if (disconnectParticles != null)
            disconnectParticles.Disconnect();
        Destroy(gameObject);
        if (createOnDestroy != null)
        {
            GameObject go = Instantiate(createOnDestroy, transform.position, transform.rotation);
        }
        
    }
}
