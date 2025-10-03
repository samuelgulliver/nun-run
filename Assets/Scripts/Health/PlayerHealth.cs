using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float startingHealth = 100;
    [SerializeField] public float currentHealth {  get; private set; }          // can get it from anywhere but can only set it from methods here (inside take damage and collect holy water etc)
    private Animator anim;
    private bool dead = false;
    SceneLogic sceneLogic;
    private void Awake()
    {
        currentHealth = startingHealth; 
        anim = GetComponent<Animator>();    // because the player object has an animator component on it, we can grab it
        sceneLogic = GetComponent<SceneLogic>();    // grabbing the SceneLogic script
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            //implement iframes
        }
        else
        {
            if(!dead)
            {
                anim.SetTrigger("die");
                GetComponent<PlayerMovement>().enabled = false;         // so the player cannot move once they die
                dead = true;
            }
        }
    }

    public void Heal(float healingAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth + healingAmount, 0, startingHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(20);
        }

        if (Input.GetKeyDown(KeyCode.H)) 
        {
            Heal(5);
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Current Health <= 0");
            sceneLogic.GameOver();
        }
    }
}
