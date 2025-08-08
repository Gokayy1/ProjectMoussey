using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIController : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsMenu;
    public GameObject InfoPanel;

    public void OpenSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsMenu != null) settingsMenu.SetActive(true);
    }

    public void BackToMain()
    {
        if (InfoPanel != null) InfoPanel.SetActive(false);
        if (settingsMenu != null) settingsMenu.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    public void OpenInfo()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (InfoPanel != null) InfoPanel.SetActive(true);
    }
}
