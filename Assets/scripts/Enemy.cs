using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Sistema de golpes")]
    public int maxHits = 3;
    private int hitsTaken = 0;

    private bool canTakeDamage = true;
    public float damageCooldown = 0.1f;

    // 🔵 compatibilidad con PlayerParry (no hace nada)
    public void Stun(float duration)
    {
        // stun desactivado
    }

    // -------------------- DESTRUIR --------------------
    public void DestroyEnemy()
    {
        Debug.Log(name + " destruido!");
        Destroy(gameObject);
    }

    IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    // -------------------- DAÑO / HIT --------------------
    public void TakeDamage(int amount = 1)
    {
        if (!canTakeDamage) return;

        hitsTaken += 1;

        Debug.Log(name + " recibió un golpe (" + hitsTaken + "/" + maxHits + ")");

        StartCoroutine(DamageCooldown());

        if (hitsTaken >= maxHits)
        {
            DestroyEnemy();
        }
    }
}