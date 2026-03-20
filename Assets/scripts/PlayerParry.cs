using UnityEngine;
using System.Collections;

public class PlayerParry : MonoBehaviour
{
    public Transform parryBox;
    public float parryDuration = 0.15f;
    public float invulnerableDuration = 0.2f;

    private bool isParrying;
    private string activeParry;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        parryBox.localScale = Vector3.zero;

        if (!PlayerPrefs.HasKey("SelectedParry"))
        {
            PlayerPrefs.SetString("SelectedParry", "Cling");
            PlayerPrefs.Save();
        }

        activeParry = PlayerPrefs.GetString("SelectedParry");
        Debug.Log("Parry activo: " + activeParry);
    }

    void Update()
    {
        // 🔥 CLICK DERECHO
        if (Input.GetMouseButtonDown(1) && !isParrying)
        {
            StartCoroutine(Parry());
        }
    }

    IEnumerator Parry()
    {
        isParrying = true;

        if (playerHealth != null)
            playerHealth.SetInvulnerable(true);

        // Activar hitbox
        parryBox.localScale = new Vector3(5f, 50f, 50f);

        Collider2D[] hits = Physics2D.OverlapBoxAll(parryBox.position, parryBox.localScale, 0f);
        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Stun(1f);
            }
        }

        yield return new WaitForSeconds(parryDuration);

        // Desactivar hitbox
        parryBox.localScale = Vector3.zero;

        yield return new WaitForSeconds(invulnerableDuration - parryDuration);

        if (playerHealth != null)
            playerHealth.SetInvulnerable(false);

        isParrying = false;
    }

    public bool IsParrying()
    {
        return isParrying;
    }

    public string GetActiveParry()
    {
        return activeParry;
    }

    public void SetActiveParry(string newParry)
    {
        activeParry = newParry;
        PlayerPrefs.SetString("SelectedParry", newParry);
        PlayerPrefs.Save();
        Debug.Log("Parry cambiado a: " + activeParry);
    }
}