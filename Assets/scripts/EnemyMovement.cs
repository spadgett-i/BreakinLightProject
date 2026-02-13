using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float stopDistance = 0.5f;

    private Transform player;
    private Rigidbody2D rb;
    private EnemyStun stunScript;

    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stunScript = GetComponent<EnemyStun>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        originalScale = transform.localScale;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // 🔥 SI ESTÁ STUNNEADO → NO HACER NADA
        if (stunScript != null && stunScript.IsStunned())
        {
            return; // ← IMPORTANTÍSIMO: no tocar el rigidbody
        }

        float distanceX = player.position.x - transform.position.x;

        if (Mathf.Abs(distanceX) < stopDistance)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        float direction = Mathf.Sign(distanceX);

        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        // Girar sprite
        if (direction > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (direction < 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    // 🔹 CLASE ENEMY INTEGRADA
    public void Stun(float duration)
    {
        StartCoroutine(StunRoutine(duration));
    }

    private bool isStunned = false;
    private IEnumerator StunRoutine(float duration)
    {
        isStunned = true;
        Debug.Log(name + " está aturdido!");
        // Opcional: desactivar animaciones o movimiento aquí
        yield return new WaitForSeconds(duration);
        isStunned = false;
        Debug.Log(name + " ya no está aturdido!");
    }

    public bool IsStunned()
    {
        return isStunned;
    }

    public void DestroyEnemy()
    {
        Debug.Log(name + " destruido por Swish!");
        Destroy(gameObject);
    }
}




