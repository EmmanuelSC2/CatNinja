using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public bool TestMode = false;
    public float lifeTime;

    //hidden in inspector because we want to set our speed via script once we instantiate
    [HideInInspector]
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        if (lifeTime <= 0)
        {
            lifeTime = 2.0f;
            if (TestMode) Debug.Log("Lifetime defaulted to 2 on object " + gameObject.name);
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, speed);
        Destroy(gameObject, lifeTime);
    }
}
