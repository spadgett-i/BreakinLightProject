using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    private Enemy enemy;

    void Start()
    {
        // Buscar Enemy en los padres
        enemy = GetComponentInParent<Enemy>();
        if (enemy == null)
        {
            Debug.LogWarning("Enemy no encontrado en EnemyHitbox. Asegúrate que el Enemy esté en el padre del hitbox.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemy == null) return; // Evita NullReferenceException

        // ❌ DESACTIVADO: daño solo por tocar al jugador
        /*
        if (collision.CompareTag("Player"))
        {
            PlayerAttack attack = collision.GetComponent<PlayerAttack>();
            if (attack != null && attack.IsAttacking())
            {
                enemy.TakeDamage();
                Debug.Log("Enemy golpeado por PlayerAttack");
            }
        }
        */

        // ✅ Solo detectar la hitbox del ataque
        if (collision.CompareTag("Attack"))
        {
            enemy.TakeDamage();
            Debug.Log("Enemy golpeado por Hitbox de espada");
        }
    }
}