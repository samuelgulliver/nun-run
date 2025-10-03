using UnityEngine;

public class GuardianAngel : MonoBehaviour
{
    [SerializeField] private float xSpeed = 2f;
    [SerializeField] private float ySpeed = 1f;
    [SerializeField] private float xMin = -2f;
    [SerializeField] private float xMax = 15f;
    [SerializeField] private float yMin = 1f;
    [SerializeField] private float yMax = 5f;
    [SerializeField] private float distFromPlayer = 5f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private int health = 2;
    [SerializeField] private float dashCooldownTime = 5f; // Time angel will wait before dashing again

    private PlayerHealth playerHealth;
    private bool isDashing = false;
    private float lastDashTime = -10f; // Initialize to a value that ensures the angel can dash immediately
    private float spawnTime;        // This is a variable so that the angels don't dash immediately after spawning 


    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        spawnTime = Time.time;
    }

    private void Update()
    {
        if (isDashing)
        {
            Vector2 directionToPlayer = (playerHealth.transform.position - transform.position).normalized;
            transform.Translate(directionToPlayer * dashSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            // Vertical movement
            if (transform.position.y >= yMax)
            {
                ySpeed = Mathf.Abs(ySpeed) * -1;  // Move downward
            }
            else if (transform.position.y <= yMin)
            {
                ySpeed = Mathf.Abs(ySpeed);  // Move upward
            }
            transform.Translate(0, ySpeed * Time.deltaTime, 0, Space.World);

            // Horizontal movement
            if (transform.position.x >= xMax)
            {
                xSpeed = Mathf.Abs(xSpeed) * -1;  // Move left
            }
            else if (transform.position.x <= xMin)
            {
                xSpeed = Mathf.Abs(xSpeed);  // Move right
            }
            transform.Translate(xSpeed * Time.deltaTime, 0, 0, Space.World);
        }

        // Check if close enough to start dashing and if enough time has passed since the last dash
        if (Vector2.Distance(transform.position, playerHealth.transform.position) <= distFromPlayer &&
            Time.time - lastDashTime >= dashCooldownTime && Time.time - spawnTime >= 5f)
        {
            isDashing = true;
            lastDashTime = Time.time;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerHealth>())
        {
            playerHealth.TakeDamage(damage);
            isDashing = false;
        }

        // If the angel collides with something else, change directions
        xSpeed = -xSpeed;
        ySpeed = -ySpeed;
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void setHealth(int health)
    {
        this.health = health;
    }

    public void setDamage(int damage)
    {
        this.damage = damage;
    }

    public void setScale(int scaleX, int scaleY)
    {
        transform.localScale = new Vector3(scaleX, scaleY, 1); 
    }

}
