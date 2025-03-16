using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingProjectiles : MonoBehaviour
{
    [System.Serializable]
    public class Attack
    {
        public Projectile projectile;
        public float cadence;
        public float attackTime;
        public float attackDelay;
        public string animationTrigger;
        public float spread;
    }

    Character character;
    public List<Attack> attacks = new();

    PlayerMovement player;
    EnemyController ec;

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
        if (attacks.Count == 0)
            return;

        timer += Time.deltaTime;
        if (timer > attacks[0].cadence)
        {
            ec.animator.SetTrigger(attacks[0].animationTrigger);

            Attack attack = attacks[0];
            System.Action delay = () => LaunchProjectile(attack);
            delay.Delay(attacks[0].attackDelay);
            timer -= attacks[0].cadence;
            character.speedMults["PROJ"] = 0f;

            System.Action delay2 = () =>
            {
                character.speedMults.Remove("PROJ");
            };

            delay2.Delay(attacks[0].attackTime);
            attacks.RemoveAt(0);

        }

    }

    void LaunchProjectile(Attack attack)
    {
        if (character.dead)
            return;
        Vector2 angleTowardsPlayer = ((Vector2)(pc.transform.position - transform.position)).normalized;

        Projectile p
         = Instantiate(attack.projectile, character.center.position, Quaternion.LookRotation(transform.forward, angleTowardsPlayer));
        p.transform.eulerAngles += new Vector3(0f, 0f, Random.Range(-attack.spread, attack.spread));
    }
}
