 using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyWalker : Enemy
{
    Rigidbody2D rb;
    [SerializeField] float xVelocity;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        if (xVelocity <= 0)
            xVelocity = 3;
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


    public override void TakeDamage(int damage)
    {
        //CREATE OWN DAMAGE FUNCTIONALITY THAT WILL DELETE THE PARENT GAMEOBJECT
        health -= damage;

        if (health <= 0)
            Destroy(gameObject.transform.parent.gameObject, 2);

        if (damage == 9999)
        {
            anim.SetTrigger("Squish");
            return;
        }

        base.TakeDamage(damage);

        Debug.Log("Enemy Walker took " + damage.ToString() + " damage");
    }

}
