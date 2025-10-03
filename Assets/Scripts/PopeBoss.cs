using System.Collections;
using UnityEngine;

public class PopeBoss : MonoBehaviour
{
    [SerializeField] private float meleeRange = 2f;
    [SerializeField] private float meleeDamage = 10f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject guardianAngelPrefab;
    [SerializeField] private float health = 100f;

    [SerializeField] private float rangedAttackCooldown = 3f;
    [SerializeField] private float projectileRange = 7f;
    [SerializeField] private float fireAngle = 15f;
    [SerializeField] private float projectileGravity = 0.25f;

    private float lastRangedAttackTime;
    private int projectilesThrown = 0;
    private bool hasSummonedFirstAngel = false;
    private bool hasSummonedSecondAngel = false;
    SceneLogic sceneLogic;

    private PlayerHealth playerHealth;
    private Rigidbody2D rb;
    private bool isDead = false;

    private void Start()
    {
        sceneLogic = GetComponent<SceneLogic>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
        lastRangedAttackTime = -rangedAttackCooldown;
    }

    private void Update()
    {
        if (isDead)
        {
            sceneLogic.EndGame();
            Destroy(gameObject);
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerHealth.transform.position);

        // Make Pope face the player
        if (playerHealth.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(7, 7, 7);
        else
            transform.localScale = new Vector3(-7, 7, 7);

        if (Time.time - lastRangedAttackTime >= rangedAttackCooldown)
        {
            if (distanceToPlayer <= projectileRange)
            {
                RangedAttack();
                lastRangedAttackTime = Time.time;

                projectilesThrown++;
                if (projectilesThrown >= 3)
                {
                    JumpTowardsPlayer();
                    projectilesThrown = 0;
                }
            }
        }

        // Guardian angel summons based on health
        if (health < 70 && !hasSummonedFirstAngel)
        {
            SummonGuardianAngel(1f);
            hasSummonedFirstAngel = true;
        }
        else if (health < 30 && !hasSummonedSecondAngel)
        {
            GameObject angel = SummonGuardianAngel(1.5f);
            GuardianAngel angelScript = angel.GetComponent<GuardianAngel>();
            angelScript.setHealth(3);
            angelScript.setDamage(20);
            angelScript.setScale(6, 6);
            hasSummonedSecondAngel = true;
        }

        // code to make sure the pope stays upright
        CorrectRotation();
    }

    private void RangedAttack()
    {
        transform.Rotate(new Vector3(0, 0, 10f));

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.gravityScale = projectileGravity;

        Vector2 direction = (Quaternion.Euler(0, 0, fireAngle) * (playerHealth.transform.position - firePoint.position)).normalized;
        projectileRb.velocity = direction * projectileSpeed;

        Destroy(projectile, 3f);

        transform.Rotate(new Vector3(0, 0, -10f));
    }

    private void JumpTowardsPlayer()
    {
        Vector2 direction = (playerHealth.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * projectileSpeed / 2, 5f);
        rb.gravityScale = 0;
        StartCoroutine(RestoreGravity());
    }

    private IEnumerator RestoreGravity()
    {
        yield return new WaitForSeconds(1.5f);
        rb.gravityScale = 1;
    }

    private GameObject SummonGuardianAngel(float scaleMultiplier)
    {
        Debug.Log("Attempting to instantiate Guardian Angel");

        Vector3 spawnPosition = new Vector3(transform.position.x, -6, transform.position.z);
        GameObject guardianAngel = Instantiate(guardianAngelPrefab, spawnPosition, Quaternion.identity);

        guardianAngel.transform.localScale *= scaleMultiplier;
        return guardianAngel;
    }


    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            isDead = true;
           
        }
    }

    private void CorrectRotation()
    {
        if (transform.rotation != Quaternion.identity)
        {
            float step = 5f * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, step);
        }
    }

    public void setHealth(int health)
    {
        this.health = health;
    }
}
