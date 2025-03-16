using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyController : MonoBehaviour
{
    public string Name;
    public string Description;

    public Animator animator;
    GameController gc;

    Character character;

    PlayerMovement player;

    public float chaseDelay;
    public float speed;

    public Vector2 velocity;
    public bool setVelocityTowardsPlayer;

    public bool wallBounce;

    private void Awake()
    {
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
        gc = FindObjectOfType<GameController>();
        player = FindAnyObjectByType<PlayerMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
        System.Action action = 
            () => velocity = (player.transform.position - transform.position).normalized * velocity.magnitude;
        if (setVelocityTowardsPlayer)
        {
            action.Delay(chaseDelay);
        }
    }

    Vector2 lastPos;
    // Update is called once per frame
    void Update()
    {
        if (character.dead)
        {
            animator.SetTrigger("Die");
            return;
        }

       

        animator.SetBool("Walk", lastPos != (Vector2)transform.position);
        lastPos = transform.position;

        if (gc.RoundRunning)
        {
            transform.position += (Vector3)(velocity * Time.deltaTime * character.TotalSpeed);
            //Do something IDK
            if (gc.RoundTimer > chaseDelay)
            {
                Chase();
            }
        }
        bumpCd -= Time.deltaTime;
        bounceCD -= Time.deltaTime;
    }

    static float bumpCd;
    float bounceCD = 0f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Wall>(out _) && wallBounce)
        {
            if (bounceCD <= 0f) {
                if (setVelocityTowardsPlayer)
                {
                    velocity = (player.transform.position - transform.position).normalized * velocity.magnitude;
                }
                else
                {
                    velocity *= -1;
                }
                
                bounceCD = .75f;
            }
        }

        if (bumpCd > 0f || character.dead)
            return;
        Character c = collision.gameObject.GetComponent<Character>();
        if (c != null && c.player)
        {
            c.TakeDamage(1);
            bumpCd = .5f;
        }

       
    }

    public void Chase()
    {
        if (!character.dead)
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * character.TotalSpeed * Time.deltaTime);
    }

}
