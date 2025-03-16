using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectiles : MonoBehaviour
{
    Character character;
    public Projectile projectile;
    public float cadence;
    public float attackTime;
    public float attackDelay;
    public float spread;
    PlayerMovement player;
    EnemyController ec;

    public string animationTrigger;

    Character pc;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Character>();
        player = FindObjectOfType<PlayerMovement>();
        pc = player.GetComponent<Character>();
        ec = FindObjectOfType<EnemyController>();
    }

    public float timer;
    // Update is called once per frame
    void Update()
    {
        if (character.dead || !GameController.Instance.RoundRunning)
            return;

        timer += Time.deltaTime;
        if (timer > cadence)
        {
            ec.animator.SetTrigger(animationTrigger);

            System.Action delay = () => LaunchProjectile();
            delay.Delay(attackDelay);
            timer -= cadence;
            character.speedMults["PROJ"] = 0f;

            System.Action delay2 = () =>
            {
                character.speedMults.Remove("PROJ");
            };

            delay2.Delay(attackTime);

        }

    }

    void LaunchProjectile()
    {
        if (character.dead)
            return;
        Vector2 angleTowardsPlayer = ((Vector2)(pc.transform.position - transform.position)).normalized;
       
        Projectile p 
         = Instantiate(projectile, character.center.position, Quaternion.LookRotation(transform.forward, angleTowardsPlayer));
        p.transform.eulerAngles += new Vector3(0f, 0f, Random.Range(-spread, spread));
    }
}
