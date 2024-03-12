 using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(BoxCollider2D))]
public abstract class Enemy : MonoBehaviour
{
    //private - private to the class that created it and is a property only of that class. Even child classes cannot access this variable. Outside the class, even with reference to this object, we cannot grab a private variable or function. 
    //public - everyone has access as long as they have a reference to your object.  This could be considered public properties... rigidbody.velocity is a public vector3 in rigidbody (this is risky as it could end up with lots of outside classes changing your variables)
    //protected - private but accesable to all childern classes.

    protected SpriteRenderer sr;
    protected Animator anim;

    protected int health;
    protected int maxHealth;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (maxHealth <= 0)
            maxHealth = 10;

        health = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            anim.SetTrigger("Death");
            Destroy(gameObject, 2);
        }
    }
}


