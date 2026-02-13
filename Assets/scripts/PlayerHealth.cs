using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Image healthFillImage;
    private int damagePerHit = 34;
    private int hitsTaken = 0;

    public float invulnerabilityTime = 0.8f;
    public float flashInterval = 0.1f;
    private bool isInvulnerable = false;
    private SpriteRenderer spriteRenderer;

    public ParryEnergy parryEnergy;
    public float parryEffectRadius = 2f; 
    private PlayerParry playerParry;

    public float healFlashDuration = 0.3f; 
    public int healBarFlashes = 4; 
    private Color originalColor;

    public ParticleSystem healParticles2D;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        playerParry = GetComponent<PlayerParry>();
        UpdateHealthUI();
    }

    void Update()
    {
        HandleParryHealInput();
    }

    public void SetInvulnerable(bool value)
    {
        isInvulnerable = value;
        if (spriteRenderer != null && value)
            spriteRenderer.enabled = true;
    }

    public bool GetInvulnerable()
    {
        return isInvulnerable;
    }

    public void TakeDamage()
    {
        if (hitsTaken >= 3) return;
        if (isInvulnerable) return;

        hitsTaken++;
        currentHealth -= damagePerHit;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthUI();
        StartCoroutine(InvulnerabilityRoutine());

        if (hitsTaken >= 3)
            Die();
    }

    IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        float timer = 0f;

        while (timer < invulnerabilityTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
            timer += flashInterval;
        }

        spriteRenderer.enabled = true;
        isInvulnerable = false;
    }

    void UpdateHealthUI()
    {
        if (healthFillImage != null)
            healthFillImage.fillAmount = (float)currentHealth / maxHealth;
    }

    void Die()
    {
        Debug.Log("Jugador muerto");
        gameObject.SetActive(false);
    }

    void HandleParryHealInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (parryEnergy != null && parryEnergy.currentEnergy >= parryEnergy.maxEnergy - 0.01f)
            {
                string activeParry = playerParry.GetActiveParry();
                switch (activeParry)
                {
                    case "Cling":
                        StartCoroutine(HealWithVisuals2D());
                        break;
                    case "Clang":
                        ApplyParryEffectToEnemies(stun: true, destroy: false);
                        StartCoroutine(ParryEnergyFlash());
                        break;
                    case "Swish":
                        ApplyParryEffectToEnemies(stun: false, destroy: true);
                        StartCoroutine(ParryEnergyFlash());
                        break;
                }
            }
        }
    }

    void ApplyParryEffectToEnemies(bool stun, bool destroy)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, parryEffectRadius);
        foreach (Collider2D hit in hits)
        {
            EnemyMeleeAttack enemyAttack = hit.GetComponent<EnemyMeleeAttack>();
            if (enemyAttack != null && !enemyAttack.isParryable)
                continue; // NO parreable

            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                EnemyStun stunScript = enemy.GetComponent<EnemyStun>();
                if (stun && stunScript != null)
                {
                    Vector2 knockbackDir = (enemy.transform.position - transform.position).normalized * 5f;
                    stunScript.Stun(knockbackDir);
                }

                if (destroy)
                    enemy.DestroyEnemy();
            }
        }
    }

    IEnumerator ParryEnergyFlash()
    {
        for (int i = 0; i < healBarFlashes; i++)
        {
            parryEnergy.energyFillImage.enabled = false;
            yield return new WaitForSeconds(healFlashDuration / (healBarFlashes * 2));
            parryEnergy.energyFillImage.enabled = true;
            yield return new WaitForSeconds(healFlashDuration / (healBarFlashes * 2));
        }
        parryEnergy.currentEnergy = 0f;
        parryEnergy.UpdateEnergyUI();
    }

    IEnumerator HealWithVisuals2D()
    {
        yield return ParryEnergyFlash();

        if (healParticles2D != null)
        {
            healParticles2D.Play();
        }

        if (hitsTaken > 0)
        {
            hitsTaken--;
            currentHealth += damagePerHit;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            UpdateHealthUI();
            Debug.Log("Curado 1 hit ⚡ Vida actual: " + currentHealth);
        }

        yield return new WaitForSeconds(healFlashDuration);
    }
}













