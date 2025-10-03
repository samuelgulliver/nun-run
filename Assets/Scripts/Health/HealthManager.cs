using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;     // reference to player health script
    [SerializeField] private Image totalHealth;             // red
    [SerializeField] private Image currentHealth;           // green (fill)

    private void Start()
    {
        totalHealth.fillAmount = playerHealth.currentHealth / 100;   // players health at the start of the game is max
    }

    private void Update()
    {
        currentHealth.fillAmount = playerHealth.currentHealth / 100;
    }
}
