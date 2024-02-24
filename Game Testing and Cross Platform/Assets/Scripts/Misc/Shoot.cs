using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public bool TestMode = false;

    SpriteRenderer sr;

    public float projectileSpeed = 7.0f;
    public Transform spawnPointLeft;
    public Transform spawnPointRight;

    public Projectile projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (projectileSpeed <= 0)
        {
            projectileSpeed = 7.0f;
        }

        if (!spawnPointLeft || !spawnPointRight || !projectilePrefab)
        {
            if (TestMode) Debug.Log("Set default values Shoot Script. On object " + gameObject.name);
        }

    }

    public void Fire()
    {
        if (!sr.flipX)
        {
            Projectile curProjectile = Instantiate(projectilePrefab, spawnPointRight.position, spawnPointRight.rotation);
            curProjectile.speed = projectileSpeed;
        }
        else
        {
            Projectile curProjectile = Instantiate(projectilePrefab, spawnPointLeft.position, spawnPointLeft.rotation);
            curProjectile.speed = projectileSpeed;
        }
    }
}