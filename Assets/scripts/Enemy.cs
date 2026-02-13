using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private bool isStunned = false;

    public void Stun(float duration)
    {
        if (!isStunned)
            StartCoroutine(StunRoutine(duration));
    }

    IEnumerator StunRoutine(float duration)
    {
        isStunned = true;
        Debug.Log(name + " está aturdido!");
        // Aquí puedes desactivar movimiento o animaciones
        yield return new WaitForSeconds(duration);
        isStunned = false;
        Debug.Log(name + " ya no está aturdido!");
    }

    public void DestroyEnemy()
    {
        Debug.Log(name + " destruido por Swish!");
        Destroy(gameObject);
    }
}
