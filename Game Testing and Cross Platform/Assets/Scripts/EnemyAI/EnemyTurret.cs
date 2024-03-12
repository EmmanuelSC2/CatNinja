  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Enemy
{
    public float projectileFireRate;

    float timeSinceLastFire = 0;
    float distThreshold = 3.0f;

    protected override void Start()
    {
        base.Start();


        if (projectileFireRate <= 0)
            projectileFireRate = 2.0f;

        //do any addional stuff that is need for my subclass.
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
}

