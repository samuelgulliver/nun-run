using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5;
    [SerializeField] private float jumpVelocity = 5;
    private Rigidbody2D body;       // this is a reference to the rigid body from the player component. 
    private Animator anim;
    private bool grounded;
    SceneLogic sceneLogic;
    private float lastDamageTime = 0f; // The last time the player took damage
    private float damageCooldown = 0.5f; // Time between possible damage events


    private void Awake()
    {
        // grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();     // get component searches the player object for a rigid body
        anim = GetComponent<Animator>();
        sceneLogic = GetComponent<SceneLogic>();    // grabbing the SceneLogic script
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");        // Input.GetAxis("Horizontal") moves the player left and right by one when the arrows or a,d is pressed.

        body.velocity = new Vector2(horizontalInput * playerSpeed, body.velocity.y);      // use body.velocity to move the player 

        // Flip player when moving left-right
        if (horizontalInput > 0.01f)                        // this means the player is moving right
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)                  // this means the player is moving left
            transform.localScale = new Vector3(-1, 1, 1);


        // we only allow the player to jump if they are on the ground and hit space
        if (Input.GetKey(KeyCode.UpArrow) && grounded)
        {
            Jump();
        }

        // set animator parameters
        anim.SetBool("run", horizontalInput != 0);      // if there is no arrow key being pressed then horizontalInput = 0. false if no key is pressed (0), true if key is pressed (1) 
        // anim.SetTrigger("jump");
        anim.SetBool("grounded", grounded);             // if grounded
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpVelocity);
        anim.SetTrigger("jump");
        grounded = false;   // the player is not grounded once it jumps
    }

    // called when this collider2d rigidbody has begun touching another rigidbody2dcollider
    // we use this for jump so the player stops jumping and goes back to idle whenever they hit another rigid body
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            grounded = true;
        if (collision.gameObject.tag == "EndMerchant")
        {
            Debug.Log("Merchant Hit");
            sceneLogic.EndGame();
        }

        if (collision.gameObject.tag == "PopeProjectile")
        {
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            Debug.Log("Hit by pope's projectile");
            playerHealth.TakeDamage(10); // Use the playerHealth instance to call TakeDamage
            lastDamageTime = Time.time; // Update the last damage time
        }

        if (collision.gameObject.tag == "Pope")
        {
            Debug.Log("Pope melee attack");
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(25); // Use the playerHealth instance to call TakeDamage
            lastDamageTime = Time.time; // Update the last damage time
        }
    }

    public bool CanAttack()
    {
        if (grounded == true) return true;
        else return false;
    }

    public void NewJumpVelocity(float jumpVelocity)
    {
        this.jumpVelocity = jumpVelocity;
    }
}