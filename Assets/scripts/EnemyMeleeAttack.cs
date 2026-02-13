using UnityEngine;
using System.Collections;

public class EnemyMeleeAttack : MonoBehaviour
{
    public Transform attackPivot;
    public float windUpTime = 0.4f;
    public float attackDuration = 0.15f;
    public float retractTime = 0.2f;
    public float cooldown = 1.5f;

    [Header("Red Attack Settings")]
    public float redAttackChance = 0.5f;     
    public float redWarningTime = 0.5f;     
    public float redSizeMultiplier = 1.6f;  
    public float enemyScaleMultiplier = 1.2f;
    public float redExtraCooldown = 0.8f;    

    [Header("Attack Type (runtime)")]
    public bool isRedAttack = false;
    public bool isParryable = true; // ✅ Ataque parreable por defecto

    private bool isAttacking;
    private Vector3 pivotStartScale;
    private Vector3 pivotEndScale;
    private SpriteRenderer sr;
    private Transform player;
    private EnemyMovement enemyMovement;
    private Vector3 originalScale; 

    void Start()
    {
        pivotStartScale = new Vector3(0f, 1f, 1f);
        pivotEndScale = new Vector3(1f, 1f, 1f);
        attackPivot.localScale = pivotStartScale;

        sr = attackPivot.GetComponent<SpriteRenderer>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        enemyMovement = GetComponent<EnemyMovement>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (!isAttacking)
            StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        isRedAttack = Random.value < redAttackChance;
        isParryable = !isRedAttack; // ❌ Rojo no parreable

        Vector3 targetScale = pivotEndScale;
        float finalCooldown = cooldown;

        if (isRedAttack)
        {
            if (sr != null)
                sr.color = Color.red;

            float elapsed = 0f;
            float growTime = redWarningTime;
            Vector3 startScale = originalScale;
            Vector3 endScale = originalScale * enemyScaleMultiplier;

            while (elapsed < growTime)
            {
                transform.localScale = Vector3.Lerp(startScale, endScale, elapsed / growTime);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.localScale = endScale;

            targetScale = pivotEndScale * redSizeMultiplier;
            finalCooldown += redExtraCooldown;
        }
        else
        {
            if (sr != null)
                sr.color = Color.white;
        }

        // ATAQUE
        yield return ScalePivot(pivotStartScale, targetScale, windUpTime);
        yield return new WaitForSeconds(attackDuration);
        yield return ScalePivot(targetScale, pivotStartScale, retractTime);

        transform.localScale = originalScale;

        if (sr != null)
            sr.color = Color.white;

        yield return new WaitForSeconds(finalCooldown);
        isAttacking = false;
    }

    IEnumerator ScalePivot(Vector3 from, Vector3 to, float time)
    {
        float elapsed = 0f;
        while (elapsed < time)
        {
            attackPivot.localScale = Vector3.Lerp(from, to, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }
        attackPivot.localScale = to;
    }

    public bool IsRedAttack() => isRedAttack;
}










