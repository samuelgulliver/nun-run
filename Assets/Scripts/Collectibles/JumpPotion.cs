using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPotion : MonoBehaviour
{
    [SerializeField] private float newJumpVelocity = 30;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            playerMovement.NewJumpVelocity(newJumpVelocity);
            gameObject.SetActive(false);
        }
    }
}
