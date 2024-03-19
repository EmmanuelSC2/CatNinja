using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyWalker : Enemy
{
    Rigidbody2D rb;
    [SerializeField] float xVelocity;
    public new int maxHealth = 1; // Maximum health of the enemy walker
    private int currentHealth; // Current health of the enemy walker

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        if (xVelocity <= 0)
            xVelocity = 3;

        currentHealth = maxHealth; // Set current health to maximum when the enemy walker starts
    }

    private void Update()
    {
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        if (curPlayingClips[0].clip.name == "Walk")
        {
            rb.velocity = sr.flipX ? new Vector2(-xVelocity, rb.velocity.y) : new Vector2(xVelocity, rb.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Barrier"))
        {
            sr.flipX = !sr.flipX;
        }
    }

    // Collision detection with player balls
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            // Reduce enemy walker health
            currentHealth--;

            // Check if health reaches zero
            if (currentHealth <= 0)
            {
                // Destroy enemy walker
                Destroy(transform.parent.gameObject);
            }
            /*else
            {
                // Play squish animation
                anim.SetTrigger("Squish");
            } */
        }
    }
}

