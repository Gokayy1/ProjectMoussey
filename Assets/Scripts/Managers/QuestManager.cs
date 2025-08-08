using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [Header("Quest Settings")]
    public List<Quest> allQuests;
    private int currentQuestIndex = 0;
    private Quest activeQuest;
    private float questTimer;

    private Dictionary<string, ResourceType> giverToResourceType = new()
    {
        { "The Living Flames", ResourceType.Fire },
        { "The Winter Itself", ResourceType.Cold },
        { "The Mother Nature", ResourceType.Earth },
        { "The Unity", ResourceType.Hope },
        { "The Void Empress", ResourceType.Void },
        { "The Everflow", ResourceType.Air }
    };

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        if (activeQuest == null) return;
        if (GameManager.Instance == null || GameManager.Instance.uiManager == null) return;

        questTimer -= Time.deltaTime;
        if (questTimer <= 0)
        {
            if (!giverToResourceType.TryGetValue(activeQuest.questTitle, out ResourceType giver))
            {
               // Debug.LogWarning("Unknown quest giver, skipping failure logic.");
                return;
            }

          //  Debug.Log("Quest failed!");

            GameManager.Instance.GiverToleranceManager.RegisterFailure(giver);
            GameManager.Instance.uiManager.HideQuestUI();

            int remaining = GameManager.Instance.GiverToleranceManager.GetRemainingTolerance(giver);
            bool gameOver = remaining <= 0;

            if (!gameOver)
            {
                currentQuestIndex++;
                if (currentQuestIndex < allQuests.Count)
                {
                    StartQuest(currentQuestIndex);
                }
                else
                {
                  //  Debug.Log("All quests failed or completed.");
                    activeQuest = null;
                }
            }
            else
            {
                string message = GetGameOverMessage(giver);
                GameManager.Instance.uiManager.ShowGameOverUI(message);
                activeQuest = null;
            }

            return;
        }

        GameManager.Instance.uiManager.UpdateQuestTimer(questTimer);
        if (GameManager.Instance.uiManager.gameObject.activeInHierarchy)
        {
            GameManager.Instance.uiManager.UpdateQuestResources(activeQuest);
        }

    }

    public void StartQuest(int index)
    {
        if (index >= allQuests.Count) return;

        activeQuest = allQuests[index];
        questTimer = activeQuest.timeLimit;
        GameManager.Instance.uiManager.ShowQuestUI(activeQuest);
    }

    public bool IsQuestComplete()
    {
        foreach (var req in activeQuest.requiredResources)
        {
            int currentAmount = ResourceManager.Instance.GetAmount(req.resourceType);
            if (currentAmount < req.requiredAmount)
                return false;
        }
        return true;
    }

    public void TryCompleteQuest()
    {
        if (!IsQuestComplete()) return;

        if (activeQuest.giver != ResourceType.Hope)
        {
            foreach (var req in activeQuest.requiredResources)
            {
                ResourceManager.Instance.SpendResource(req.resourceType, req.requiredAmount);
            }
        }
        else
        {
          //  Debug.Log("The Unity is generous. No resources spent.");

            Vector3 popupPos = Camera.main.transform.position + Vector3.forward * 10f;
            GameManager.Instance.uiManager.ShowSpecialQuestPopup(popupPos);
        }

        GameManager.Instance.uiManager.HideQuestUI();
        AudioManager.Instance.PlaySpecialQuestSFX();

        currentQuestIndex++;
        if (currentQuestIndex < allQuests.Count)
        {
            StartQuest(currentQuestIndex);
        }
        else
        {
           // Debug.Log("All quests completed!");
            activeQuest = null; 
            GameManager.Instance.uiManager.HideQuestUI();

            GameManager.Instance.uiManager.ShowWinUI();
        }
    }

    public Quest GetActiveQuest() => activeQuest;
    public float GetRemainingTime() => questTimer;

    public string GetGameOverMessage(ResourceType giver)
    {
        return giver switch
        {
            ResourceType.Fire => "The Living Flames turned Moussey into ashes.",
            ResourceType.Cold => "The Winter Itself froze Moussey's soul.",
            ResourceType.Earth => "The Mother Nature exiled Moussey from the world.",
            ResourceType.Hope => "The Unity lost its hope in Moussey.",
            ResourceType.Void => "The Void Empress consumed Moussey into nothingness.",
            ResourceType.Air => "The Everflow scattered Moussey to the winds.",
            _ => "An unknown force has ended Moussey's journey."
        };
    }

    public void ResetManager()
    {
        currentQuestIndex = 0;
        activeQuest = null;

        if (allQuests != null && allQuests.Count > 0)
        {
            StartQuest(currentQuestIndex);
        }

      //  Debug.Log("[QUEST MANAGER] Reset completed.");
    }
}
