using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Vector2 maxVelocity;
    Rigidbody2D rb;
    Animator animator;

    Vector2 velocity;

    Character c;


    private void Awake()
    {
        c = GetComponent<Character>();
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    float animSpeed = 1f;
    // Update is called once per frame


    void Update()
    {
        if (c.dead)
        {
            animator.SetBool("Dead", true);
            rb.velocity = new Vector2();
            return;
        }
        animator.SetBool("Dead", false);

        Vector2 modifiedMaxVelocity = maxVelocity * c.TotalSpeed;

        if (!GameController.Instance.RoundRunning)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("Walking", false);
            return;
        }
        rb.velocity = (velocity.sqrMagnitude > 1f ? velocity.normalized : velocity) * modifiedMaxVelocity;
        animSpeed = Mathf.Max(1, rb.velocity.magnitude / modifiedMaxVelocity.x);

        animator.speed = animSpeed;
        animator.SetBool("Walking", rb.velocity.sqrMagnitude > .01f);
    }

    public void Move(InputAction.CallbackContext context)
    {
        velocity = context.ReadValue<Vector2>();
    }
}
