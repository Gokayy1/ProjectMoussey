using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class SceneManagerCustom : MonoBehaviour
{
   public static SceneManagerCustom Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadMainMenu()
    {
        LoadSceneWithLoading("MainMenu");
    }

    public void LoadMainMenuSafe()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGame()
    {
        LoadSceneWithLoading("Game");
    }

    public void ExitGame()
    {
        Application.Quit();
     //   Debug.Log("ExitGame called.");
    }

    public void LoadSceneWithLoading(string sceneToLoad)
    {
        StartCoroutine(LoadAsyncScene(sceneToLoad));
    }

    private IEnumerator LoadAsyncScene(string sceneToLoad)
    {
      //  Debug.Log("[LOADING] Switching to LoadingScene...");
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);

        yield return new WaitForSeconds(0.1f);

      //  Debug.Log($"[LOADING] Starting async load of scene: {sceneToLoad}");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);

        if (asyncLoad == null)
        {
          //  Debug.LogError($"[LOADING] Failed to load scene {sceneToLoad}. asyncLoad is null!");
            yield break;
        }

        asyncLoad.allowSceneActivation = false;

        GameObject barGO = null;
        Slider loadingBar = null;
        TextMeshProUGUI loadingText = null;

        float timeout = 5f;
        while ((barGO == null || loadingBar == null || loadingText == null) && timeout > 0f)
        {
            barGO = GameObject.Find("LoadingBar");
            loadingBar = barGO?.GetComponent<Slider>();

            GameObject textGO = GameObject.Find("LoadingText");
            loadingText = textGO?.GetComponent<TextMeshProUGUI>();

            timeout -= Time.deltaTime;
            yield return null;
        }

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            loadingBar?.SetValueWithoutNotify(progress);
            if (loadingText != null)
                loadingText.text = $"LOADING... {(int)(progress * 100)}%";

            if (asyncLoad.progress >= 0.9f)
            {
              //  Debug.Log("[LOADING] Scene load reached 90%, preparing to activate...");

                yield return new WaitForSeconds(0.2f);

             //   Debug.Log("[LOADING] Activating scene.");
                asyncLoad.allowSceneActivation = true;

                yield return new WaitUntil(() => asyncLoad.isDone);
                yield return new WaitForSeconds(0.2f);
                yield return null;

              //  Debug.Log("[LOADING] Resetting all managers...");
                ResetAllManagersSafe();

                yield return new WaitForSeconds(0.1f);
                yield return null;

              //  Debug.Log("[LOADING] LoadingScene is assumed to be unloaded by SceneManager.");
            }

            yield return null;
        }

       // Debug.Log("[LOADING] Scene fully loaded and activated.");
    }

    private void ResetAllManagersSafe()
    {
       // Debug.Log(">>> ResetAllManagersSafe() started");

        if (GameManager.Instance == null)
        {
          //  Debug.LogError("GameManager.Instance is NULL in ResetAllManagersSafe!");
            return;
        }

        GameManager.Instance.FindAllManagers();

        GameManager.Instance.uiManager?.ResetManager();
        GameManager.Instance.questManager?.ResetManager();
        GameManager.Instance.mapManager?.ResetManager();
        GameManager.Instance.resourceRegrowManager?.RegisterInitialResources();
        GameManager.Instance.resourceManager?.ResetManager();
        GameManager.Instance.playerManager?.ResetManager();
        GameManager.Instance.GiverToleranceManager?.ResetManager();
        GameManager.Instance.upgradeManager?.ResetManager();
        GameManager.Instance.enemyManager?.ResetManager();

       // Debug.Log("<<< ResetAllManagersSafe() finished");
    }
}
