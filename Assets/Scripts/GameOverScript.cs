using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    private Button[] buttons;

    void Start()
    {
        // Get the buttons
        buttons = GetComponentsInChildren<Button>(true); // Get even inactive ones!

        // Disable them
        HideButtons();
    }

    public void HideButtons()
    {
        foreach (var b in buttons)
        {
            b.gameObject.SetActive(false);
        }
    }

    public void ShowButtons()
    {
        Debug.Log("Showing buttons: " + buttons);
        foreach (var b in buttons)
        {
            b.gameObject.SetActive(true);
            Debug.Log("Showing button " + b);
        }
    }

    public void ExitToMenu()
    {
        // Reload the level
        SceneManager.LoadScene("Menu");
    }

    public void RestartGame()
    {
        // Reload the level
        SceneManager.LoadScene("Stage1");
    }
}
