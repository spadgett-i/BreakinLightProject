using UnityEngine;
using System.Collections;

public class PlayerParry : MonoBehaviour
{
    public Transform parryBox;
    public float parryDuration = 0.15f;
    public float invulnerableDuration = 0.2f; // Duración total de invulnerabilidad

    private bool isParrying;
    private string activeParry; // Cling, Clang, Swish
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        // Ajustar escala inicial del parryBox
        parryBox.localScale = new Vector3(0f, 0.6f, 1f);

        // Asegurarse de que Cling sea el parry por defecto
        if (!PlayerPrefs.HasKey("SelectedParry"))
        {
            PlayerPrefs.SetString("SelectedParry", "Cling");
            PlayerPrefs.Save();
        }

        // Cargar parry seleccionado
        activeParry = PlayerPrefs.GetString("SelectedParry");
        Debug.Log("Parry activo: " + activeParry);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isParrying)
        {
            StartCoroutine(Parry());
        }
    }

    IEnumerator Parry()
    {
        isParrying = true;

        // 🔹 Activar invulnerabilidad mientras dure el parry
        if (playerHealth != null)
            playerHealth.SetInvulnerable(true);

        // Mostrar hitbox
        parryBox.localScale = new Vector3(1f, 0.6f, 1f);

        // Detectar enemigos en el parryBox
        Collider2D[] hits = Physics2D.OverlapBoxAll(parryBox.position, parryBox.localScale, 0f);
        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Stun(1f); // Parry físico siempre stunea
            }
        }

        yield return new WaitForSeconds(parryDuration);

        // Ocultar hitbox
        parryBox.localScale = new Vector3(0f, 0.6f, 1f);

        // Mantener invulnerable un poco más si quieres
        yield return new WaitForSeconds(invulnerableDuration - parryDuration);

        // 🔹 Desactivar invulnerabilidad
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









