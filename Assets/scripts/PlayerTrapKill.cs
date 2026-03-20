using UnityEngine;

public class PlayerTrapKill : MonoBehaviour
{
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("trap"))
        {
            KillPlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("trap"))
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        if (playerHealth != null)
        {
            Debug.Log("☠️ Player murió por trap");

            // Fuerza muerte inmediata
            playerHealth.currentHealth = 0;

            // Llama a la muerte directamente
            playerHealth.SendMessage("Die");
        }
    }
}