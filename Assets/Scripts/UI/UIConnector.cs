using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIConnector : MonoBehaviour
{
    public void OnTryAgainButton()
    {
        Time.timeScale = 1f;
        if(PauseManager.Instance != null)
            PauseManager.Instance.LockPauseInput(false);
            
        SceneManagerCustom.Instance.LoadSceneWithLoading("Game");
    }

    public void OnBackToTitle()
    {
        Time.timeScale = 1f;
        PauseManager.Instance.LockPauseInput(false);
        SceneManager.LoadScene("MainMenu");
    }
    public void OnQuitButton()
    {
        if (SceneManagerCustom.Instance != null)
            SceneManagerCustom.Instance.ExitGame();
        else
            Debug.LogError("SceneManagerCustom.Instance is null.");
    }
}

