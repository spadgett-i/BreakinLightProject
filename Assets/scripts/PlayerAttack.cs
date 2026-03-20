using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackBox;       // Reutiliza el parryBox como hitbox
    public float attackDuration = 0.15f;
    public int attackDamage = 25;

    private bool isAttacking = false;

    // 🔵 evita golpear al mismo enemigo múltiples veces
    private HashSet<Enemy> enemiesHit = new HashSet<Enemy>();

    // 🔵 agregado: referencia al collider de la hitbox
    private Collider2D attackCollider;

    public bool IsAttacking()
    {
        return isAttacking;
    }

    void Start()
    {
        attackBox.localScale = Vector3.zero;

        // 🔵 obtener collider del attackBox
        attackCollider = attackBox.GetComponent<Collider2D>();

        // 🔵 desactivar collider al inicio
        if (attackCollider != null)
            attackCollider.enabled = false;
    }

    void Update()
    {
        // 🗡 Click izquierdo para atacar
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        enemiesHit.Clear(); // 🔵 limpiar lista de enemigos golpeados

        // Activar hitbox
        attackBox.localScale = new Vector3(12f, 5f, 5f);

        // 🔵 activar collider de ataque
        if (attackCollider != null)
            attackCollider.enabled = true;

        // Detectar enemigos dentro del hitbox
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackBox.position, attackBox.localScale, 0f);
        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null && !enemiesHit.Contains(enemy))
            {
                enemy.TakeDamage(attackDamage);
                enemiesHit.Add(enemy); // 🔵 evitar daño duplicado
                Debug.Log("Enemigo golpeado por " + attackDamage);
            }
        }

        // Esperar duración del ataque
        yield return new WaitForSeconds(attackDuration);

        // Desactivar hitbox
        attackBox.localScale = Vector3.zero;

        // 🔵 desactivar collider de ataque
        if (attackCollider != null)
            attackCollider.enabled = false;

        isAttacking = false;
    }
}