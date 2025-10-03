using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NunMovement : MonoBehaviour
{
    public int damage = 20;
    public int nunSpeed = 3;      // Speed of enemy nuns
    public int nunHealth = 1;       // Number of hits it takes to kill a nun
    private Rigidbody2D body;               // this is a reference to the rigid body from the nun component 
    public Animator anim;
    private BoxCollider2D boxCollider;      // box collider that we can deactivate once nun has been killed
    // PlayerHealth playerHealth;
    public bool attackPlayer = false;       // boolean to tell if the player has been attacked
    public bool hit = false;


    [SerializeField] private float movementDistance;
    private bool movingLeft = true;
    private float leftEdge;
    private float rightEdge;

    private void Awake()
    {
        // grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();     // get component searches the nun object for a rigid body
        anim = GetComponent<Animator>();

        // set the edges to be starting position +- movement distance (/2?)
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }

    private void Update()
    {
        // we don't want the nun to keep moving so return (don't run the rest of the update code)
        if (nunHealth <= 0)
        {
            return;
        }


        if (anim.GetCurrentAnimatorStateInfo(0).IsName("freak"))
        {
            Debug.Log("blow animation is playing");
            Destroy(gameObject);
            // blow = true;
        }

        if (movingLeft)
        {
            if (transform.position.x > leftEdge)        // if it has not hit the left edge
            {
                transform.position = new Vector3(transform.position.x - nunSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else                                        // if it hits the left edge go back to moving right
            {
                movingLeft = false;
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            if (transform.position.x < rightEdge)       // if it has not hit the right edge
            {
                transform.position = new Vector3(transform.position.x + nunSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else                                        // if it hits the right edge go back to moving left
            {
                movingLeft = true;
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }


    private void Start()
    {
        // playerHealth = GetComponent<PlayerHealth>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
            if (movingLeft)
            {
                movingLeft = false;
                transform.localScale = new Vector3(-0.7f, 0.7f, 0.7f);
            }
            else
            {
                movingLeft = true;
                transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }
        }

        if (collision.gameObject.GetComponent<Projectile>() != null)
        {
            Debug.Log("nun collided with fireball");
            nunHealth--;
            if (nunHealth <= 0)
            {
                BoxCollider2D boxCollider = gameObject.GetComponent<BoxCollider2D>();
                boxCollider.enabled = false;
                anim.SetTrigger("blow");
                // boxCollider.enabled = false;
            }
        }
    }

    // method used by the animation "blow" to deactive gameobject once animation has run
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

