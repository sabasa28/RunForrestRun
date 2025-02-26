using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIMainMenu : MonoBehaviour
{
    [SerializeField] GameObject InstructionsPanel;

    public void ShowInstructions(bool bShouldShow)
    {
        InstructionsPanel.SetActive(bShouldShow);
    }

    public void GoToGameplayScene()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void CloseGame()
    {
        Application.Quit();
        Debug.Log("El juego se hubiese cerrado si no estuvieses en editor");
    }
}
