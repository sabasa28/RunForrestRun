using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIMainMenu : MonoBehaviour
{
    [SerializeField] GameObject InstructionsPanel;
    [SerializeField] GameObject CreditsPanel;
    [SerializeField] GameObject PlayPanel;

    public void ShowInstructions(bool bShouldShow)
    {
        if (bShouldShow)
        {
            AudioManager.Get().PlayUISelect();
        }
        else 
        {
            AudioManager.Get().PlayUIBack();
        }
        InstructionsPanel.SetActive(bShouldShow);
    }

    public void ShowCredits(bool bShouldShow)
    {
        if (bShouldShow)
        {
            AudioManager.Get().PlayUISelect();
        }
        else
        {
            AudioManager.Get().PlayUIBack();
        }
        CreditsPanel.SetActive(bShouldShow);
    }

    public void ShowPlayOptions(bool bShouldShow)
    {
        if (bShouldShow)
        {
            AudioManager.Get().PlayUISelect();
        }
        else
        {
            AudioManager.Get().PlayUIBack();
        }
        PlayPanel.SetActive(bShouldShow);
    }

    public void GoToGameplayEasyScene()
    {
        AudioManager.Get().PlayUISelect();
        SceneManager.LoadScene("Tutorial");
    }

    public void GoToGameplayMediumScene()
    {
        AudioManager.Get().PlayUISelect();
        SceneManager.LoadScene("Facil");
    }
    public void GoToGameplayHardScene()
    {
        AudioManager.Get().PlayUISelect();
        SceneManager.LoadScene("Dificil");
    }

    public void CloseGame()
    {
        AudioManager.Get().PlayUIBack();
        Application.Quit();
        Debug.Log("El juego se hubiese cerrado si no estuvieses en editor");
    }
}
