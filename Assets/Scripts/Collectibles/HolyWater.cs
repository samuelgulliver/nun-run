using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyWater : MonoBehaviour
{
    [SerializeField] private float healingAmount = 30;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            playerHealth.Heal(healingAmount);
            gameObject.SetActive(false);
        }
    }
}
