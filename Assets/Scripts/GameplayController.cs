using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour
{
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject LosePanel;
    bool bIsGamePaused = false;

    static GameplayController instance;

    public static GameplayController Get()
    {
        return instance;
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            ChangePauseState(!bIsGamePaused);
        }
    }

    public void OnWin()
    {
        WinPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void OnLose()
    {
        LosePanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void ChangePauseState(bool newState)
    {
        bIsGamePaused = newState;
        Time.timeScale = bIsGamePaused ? 0.0f : 1.0f;
        PausePanel.SetActive(bIsGamePaused);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
