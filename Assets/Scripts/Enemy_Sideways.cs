using UnityEngine;

public class Enemy_Sideways : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float movementDistance;
    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;

    private void Awake()
    {
        // set the edges to be starting position +- movement distance (/2?)
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance; 
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (transform.position.x > leftEdge)        // if it has not hit the left edge
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else                                        // if it hits the left edge go back to moving right
                movingLeft = false;
        }
        else
        {
            if (transform.position.x < rightEdge)       // if it has not hit the right edge
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else                                        // if it hits the right edge go back to moving left
                movingLeft = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
