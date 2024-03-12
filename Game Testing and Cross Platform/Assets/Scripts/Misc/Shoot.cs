using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public bool TestMode = false;

    SpriteRenderer sr;

    [SerializeField] float initialXVelocity = 7.0f;
    [SerializeField] float initialYVelocity = 7.0f;
    public Transform spawnPointLeft;
    public Transform spawnPointRight;

    public Projectile projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
       sr = GetComponent<SpriteRenderer>();

        /*

        if (initialXVelocity <= 0)
        {
            initialXVelocity = 7.0f;
        }

        if (initialYVelocity <= 0)
        {
            initialYVelocity = 7.0f; 
        } 
       */

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
            curProjectile.xVelocityVar = initialXVelocity;
            curProjectile.yVelocityVar = initialYVelocity;
        }
        else
        {
            Projectile curProjectile = Instantiate(projectilePrefab, spawnPointLeft.position, spawnPointLeft.rotation);
            curProjectile.xVelocityVar = -initialXVelocity;
            curProjectile.yVelocityVar = initialYVelocity;
        }
    }
}