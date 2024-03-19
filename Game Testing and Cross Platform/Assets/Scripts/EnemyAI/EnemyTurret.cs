  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Enemy
{
    public float projectileFireRate;
    public new int maxHealth = 2; // Maximum health of the turret
    private int currentHealth; // Current health of the turret

    float timeSinceLastFire = 0;
    float distThreshold = 10.0f;

    protected override void Start()
    {
        base.Start();

        if (projectileFireRate <= 0)
            projectileFireRate = 2.0f;

        currentHealth = maxHealth; // Set current health to maximum when the turret starts
    }

    private void Update()
    {
        if (!GameManager.Instance.PlayerInstance) return;

        sr.flipX = (GameManager.Instance.PlayerInstance.transform.position.x < transform.position.x) ? true : false;

        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        float distance = Vector3.Distance(GameManager.Instance.PlayerInstance.transform.position, transform.position);

        if (distance <= distThreshold)
        {
            sr.color = Color.red;
            if (curPlayingClips[0].clip.name != "Fire")
            {
                if (Time.time >= timeSinceLastFire + projectileFireRate)
                {
                    anim.SetTrigger("Fire");
                    timeSinceLastFire = Time.time;
                }
            }
        }
        else
        {
            sr.color = Color.white;
        }
    }

    // Collision detection with player balls
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            // Reduce turret health
            currentHealth--;

            // Check if health reaches zero
            if (currentHealth <= 0)
            {
                // Destroy turret
                Destroy(gameObject);
            }
        }
    }
}


