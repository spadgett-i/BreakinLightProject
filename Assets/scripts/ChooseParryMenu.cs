using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseParryMenu : MonoBehaviour
{
    public void SelectParry(string archetypeName)
    {
        PlayerPrefs.SetString("SelectedParry", archetypeName);
        Debug.Log("Parry seleccionado: " + archetypeName);
        SceneManager.LoadScene("GameScene");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}



