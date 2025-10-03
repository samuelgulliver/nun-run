using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;       // The position that the bullets are fired from
    [SerializeField] private GameObject[] fireballs;    // A array of the fireballs

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = 100;

    private void Awake()
    {
       anim = GetComponent<Animator>();                         // grabbing the animator component from the player
       playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && cooldownTimer > attackCooldown /*&&playerMovement.canAttack()*/)
        {
            Attack();
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        fireballs[FindFireball()].transform.position = firePoint.position; // step one: take one of the fireballs and set it's position to the firepoint
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));   // two: get the fireball and face it the firection that the player is facing
    }

    private int FindFireball()
    {
        // This for loop loops through the array
        for(int i = 0; i < fireballs.Length; i++)
        {
            // e.g. if fireball three is not active then you can useit - return it 
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }





    
}

/*
if (Input.GetKey(KeyCode.G) && cooldownTimer > attackCooldown)
{
    AttackGiganticFlame();
}
private void AttackGiganticFlame()
{

}*/