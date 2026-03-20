using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    public float knockbackForce = 2.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        PlayerParry playerParry = collision.GetComponent<PlayerParry>();
        ParryEnergy energySystem = collision.GetComponent<ParryEnergy>();
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

        EnemyStun stunScript = GetComponentInParent<EnemyStun>();
        Rigidbody2D enemyRb = GetComponentInParent<Rigidbody2D>();
        EnemyMeleeAttack attack = GetComponentInParent<EnemyMeleeAttack>();

        // 🔵 agregado: solo hacer daño si el enemigo está atacando
        if (attack != null && !attack.IsAttacking())
            return;

        // 🔴 ATAQUE ROJO → SIEMPRE hace daño (ignora parry e invulnerabilidad)
        if (attack != null && attack.IsRedAttack())
        {
            if (playerHealth != null)
                playerHealth.TakeTrueDamage();

            return;
        }

        // ⚪ ATAQUE NORMAL → SE PUEDE PARREAR
        if (playerParry != null && playerParry.IsParrying())
        {
            string activeParry = playerParry.GetActiveParry();
            Debug.Log("PARRY PERFECTO ⚡ Tipo: " + activeParry);

            // ⚡ Cling → cargar energía
            if (activeParry == "Cling")
            {
                if (energySystem != null)
                    energySystem.GainEnergy();
            }

            // ⚡ Clang → stun + knockback
            if (activeParry == "Clang")
            {
                if (stunScript != null && enemyRb != null)
                {
                    Vector2 direction = (enemyRb.transform.position - collision.transform.position).normalized;
                    Vector2 force = direction * knockbackForce;
                    stunScript.Stun(force);
                }
            }

            // ⚡ Swish → destruir enemigo
            if (activeParry == "Swish")
            {
                Enemy enemy = GetComponentInParent<Enemy>();
                if (enemy != null)
                    enemy.DestroyEnemy();
            }

            return;
        }

        // 🩸 Si NO está parrying → daño normal
        if (playerHealth != null)
            playerHealth.TakeDamage();
    }
}