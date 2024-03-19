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
    Rigidbody2D rb; // Reference to the Rigidbody2D component, which allows the GameObject to be affected by physics
    SpriteRenderer sr; // Reference to the SpriteRenderer component, which renders the 2D graphics of the GameObject
    Animator anim; // Reference to the Animator component, which controls animations for the GameObject
    AudioSource audioSource;// Reference to the AudioSource component, which plays audio clips

    //Movemement Var
    [SerializeField] private float speed = 7.0f; // Speed of the player character
    [SerializeField] private float jumpForce = 300.0f; // Force applied when the player jumps

    //Groundcheck stuff
    [SerializeField] private bool isGrounded; // Flag to check if the player is touching the ground
    [SerializeField] private Transform GroundCheck; // Transform used to check if the player is grounded
    [SerializeField] private LayerMask isGroundLayer; // Layer mask used to determine what is considered ground
    [SerializeField] private float groundCheckRadius = 0.02f; // Radius of the circle used for ground checking

    //Audio Clips
    [SerializeField] AudioClip jumpSound; // Sound clip played when the player jumps
    [SerializeField] AudioClip stompSound; // Sound clip played when the player stomps

    //Coroutine Variable
    Coroutine jumpForceChange; // Coroutine used to change the player's jump force

    // Start is called before the first frame update
    void Start()
    {
        //Component references grabbed through script
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the GameObject
        sr = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component attached to the GameObject
        anim = GetComponent<Animator>(); // Get the Animator component attached to the GameObject
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to the GameObject

        // Set default values if parameters are not set
        if (speed <= 0)
        {
            speed = 7.0f; // Default speed value if not set
            if (TestMode) Debug.Log("Speed has been set to a default value of 7.0f " + gameObject.name);
        }

        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.02f;
            if (TestMode) Debug.Log("Hey our ground check radius was defauted to 0.2f " + gameObject.name);
        }


        if (GroundCheck == null)
        {
            // Create a GroundCheck object if not set
            GameObject obj = new GameObject(); // Create a new GameObject
            obj.transform.SetParent(gameObject.transform);// Set its parent to the current GameObject
            obj.transform.localPosition = Vector3.zero;// Reset its local position
            obj.name = "GroundCheck";// Set its name
            GroundCheck = obj.transform;// Assign it to the GroundCheck variable
            if (TestMode) Debug.Log("GroundCheck Object is Created " + gameObject.name);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;

        float xInput = Input.GetAxisRaw("Horizontal");
        //float yInput = Input.GetAxisRaw("Vertical");

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
            audioSource.PlayOneShot(jumpSound);
        }

        if (Input.GetButtonDown("Jump") && !isGrounded)
        {
            anim.SetTrigger("JumpAtk");
            audioSource.PlayOneShot(stompSound);
        }

        anim.SetBool("IsGrounded", isGrounded);
        anim.SetFloat("Speed", Mathf.Abs(xInput));

        //Sprite Flipping
        if (xInput != 0) sr.flipX = (xInput < 0);
    }

    public void IncreaseGravity()
    {
        rb.gravityScale = 5;
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

    //trigger functions are called most other times - but would still require at least one object to be physics enabled
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyProjectile"))
        {
            GameManager.Instance.lives--;
            //SceneManager.LoadScene("GameOver");
        }

        if (collision.CompareTag("Squish"))
        {
            collision.transform.parent.gameObject.GetComponent<Enemy>().TakeDamage(9999);

            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //audioSource.PlayOneShot(stompSound);
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
