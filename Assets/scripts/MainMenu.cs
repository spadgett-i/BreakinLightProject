using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Este script maneja los botones del menú principal

    public void PlayGame()
    {
        // Reemplaza "GameScene" por el nombre exacto de tu escena
        Debug.Log("Botón Play presionado!");
        SceneManager.LoadScene("Juego");
    }

    public void ChooseParryArchetype()
    {
        // Reemplaza "ChooseParryScene" por la escena de selección de parry
        Debug.Log("Botón Play presionado!");
        SceneManager.LoadScene("Parry");
    }

    public void ExitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

        // Esto solo sirve dentro del editor para probar
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
