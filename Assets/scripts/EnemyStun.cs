using UnityEngine;
using System.Collections;

public class EnemyStun : MonoBehaviour
{
    public float stunDuration = 1f;

    private Rigidbody2D rb;
    private bool isStunned;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Stun(Vector2 knockbackForce)
    {
        if (!isStunned)
        {
            StartCoroutine(StunRoutine(knockbackForce));
        }
    }

    IEnumerator StunRoutine(Vector2 knockbackForce)
    {
        isStunned = true;

        rb.linearVelocity = Vector2.zero;

        // 💥 Aplicar knockback
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);

        // Si tienes lógica de IA, aquí puedes desactivarla, pero sin EnemyAI esto se elimina

        yield return new WaitForSeconds(stunDuration);

        rb.linearVelocity = Vector2.zero;
        isStunned = false;

        // Reactivar IA si la hubieras desactivado
    }

    public bool IsStunned()
    {
        return isStunned;
    }
}






