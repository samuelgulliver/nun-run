using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    private float direction;
    private bool hit;
    private float lifetime;


    private BoxCollider2D boxCollider;
    private Animator anim;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    // if it hits then stop moving
    // if it doesn't hit then move the projectile by movement speed using transform.Translate
    private void Update()
    {
        if (hit) return; // if the projetile hits something, return and do not execute the rest of the code

        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 3) gameObject.SetActive(false);
    }

    // If the fireball hits another object, this is what we're going to do
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has the PopeBoss script
        PopeBoss pope = collision.gameObject.GetComponent<PopeBoss>();
        if (pope != null) // If it does...
        {
            pope.TakeDamage(5); // Call the TakeDamage method (assuming 5 is the damage amount)
        }

        // Check if the collided object has the Angel script
        GuardianAngel angel = collision.gameObject.GetComponent<GuardianAngel>();
        if (angel != null) // If it does...
        {
            angel.TakeDamage(1); // Call the TakeDamage method (assuming 5 is the damage amount)
        }

        // code to destroy the projectile
        lifetime = 0;
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode");
    }

    // used everytime we shoot, used to reset the state of the fireball as well. 
    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        // if the fireball is facing the wrong way, flip it
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != direction)
        {
            localScaleX = -localScaleX;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}


