using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

//ensures that these components are attached to the gameobject
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public bool TestMode = false;

    //Components
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    //Movemement Var
    [SerializeField] private float speed = 7.0f;
    [SerializeField] private float jumpForce = 300.0f;

    //Groundcheck stuff
    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private LayerMask isGroundLayer;
    [SerializeField] private float groundCheckRadius = 0.02f;

    [SerializeField] private int maxLives = 5;

    //PointMagaer
    public PointManager pm;

    //Fields and Properties
    private int _lives = 3;
    public int lives
    {
        get => _lives;
        set
        {
            //if (_lives > value)
            //we lost a life = respawn

            _lives = value;

            //if (_lives > maxLives)
            //we've increased past our life maximum - so we should be set to our maxium
            //_lives = maxLives

            //if (_lives <= 0)
            //GameOver!!!

            if (TestMode) Debug.Log("Lives has been set to: " + _lives.ToString());
        }
    }

    private int _score = 0;
    public int score
    {
        get => _score;
        set
        {
            _score = value;

            if (TestMode) Debug.Log("Score has been set to: " + _score.ToString());
        }
    }

    //Coroutine Variable
    Coroutine jumpForceChange;



    // Start is called before the first frame update
    void Start()
    {
        //Component references grabbed through script
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (speed <= 0)
        {
            speed = 7.0f;
            if (TestMode) Debug.Log("Speed has been set to a default value of 7.0f " + gameObject.name);
        }

        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.02f;
            if (TestMode) Debug.Log("Hey our ground check radius was defauted to 0.2f " + gameObject.name);
        }

        if (GroundCheck == null)
        {
            GameObject obj = new GameObject();
            obj.transform.SetParent(gameObject.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.name = "GroundCheck";
            GroundCheck = obj.transform;
            if (TestMode) Debug.Log("GroundCheck Object is Created " + gameObject.name);
        }

    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        if (isGrounded)
        {
            rb.gravityScale = 1;
            anim.ResetTrigger("JumpAtk");
        }

        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, isGroundLayer);

        if (clipInfo[0].clip.name == "Throw")
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.velocity = new Vector2(xInput * speed, rb.velocity.y);
            if (Input.GetButtonDown("Fire1"))
            {
                anim.SetTrigger("Throw");
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetButtonDown("Jump") && !isGrounded)
        {
            anim.SetTrigger("JumpAtk");
        }

        anim.SetBool("IsGrounded", isGrounded);
        anim.SetFloat("Speed", Mathf.Abs(xInput)); 

        //Sprite Flipping
        if (xInput != 0) sr.flipX = (xInput < 0);
    }

    public void IncreaseGravity()
    {
        rb.gravityScale = 15;
    }

    public void StartJumpForceChange()
    {
        if (jumpForceChange == null)
            jumpForceChange = StartCoroutine(JumpForceChange());
        else
        {
            StopCoroutine(jumpForceChange);
            jumpForceChange = null;
            jumpForce /= 2;
            jumpForceChange = StartCoroutine(JumpForceChange());
        }

    }


    IEnumerator JumpForceChange()
    {
        jumpForce *= 2;
        yield return new WaitForSeconds(5.0f);
        jumpForce /= 2;
        jumpForceChange = null;
    }

    //Point collectibles
   /* void OnTriggerEnter2D(Collider2D other)
    { 
       
        
        
        
        if (other.gameObject.CompareTag("Point"))
        {
            Destroy(other.gameObject);
            pm.poinCount++;
        }
    } */

    //trigger functions are called most other times - but would still require at least one object to be physics enabled
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyProjectile"))
        {
            GameManager.Instance.lives--;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }

    //Collision functions are only called - when one of the two objects is a dynamic rigidbody
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}
