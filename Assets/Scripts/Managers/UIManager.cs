using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class SimpleResourceSlot
{
    public ResourceType type;
    public TextMeshProUGUI amountText;
}

public class UIManager : MonoBehaviour
{
    [System.Serializable]
    public class OrbUI
    {
        public ResourceType type;
        public OrbSlot slot;
        public int maxValue = 10;
    }

    public GameObject winPanel;
    public GameObject gameOverPanel;

    [Header("Primary Resource Slots")]
    public List<OrbUI> orbUIs;

    [Header("Secondary Resource Slots (like Wood, Stone)")]
    public List<SimpleResourceSlot> otherUIs;

    [Header("Resource Data Mapping")]
    public List<ResourceData> resourceDataList;

    private Dictionary<ResourceType, ResourceData> dataMap;

    [Header("Resource Popup")]
    public GameObject resourcePopupPrefab;
    public Canvas mainCanvas;

    [Header("Special Quest Popup")]
    public GameObject specialQuestPopupPrefab;

    [Header("Quests")]
    public GameObject questUIPanel; 
    public TextMeshProUGUI questTitleText;
    public Transform questResourceListContainer;
    public GameObject questResourceSlotPrefab;
    public TextMeshProUGUI questTimerText;

    private List<GameObject> activeResourceSlots = new();



    void Awake()
    {
        dataMap = new Dictionary<ResourceType, ResourceData>();
        foreach (var data in resourceDataList)
        {
            if (!dataMap.ContainsKey(data.type))
                dataMap.Add(data.type, data);
        }
    }

    public void UpdateResourceUI(ResourceType type, int amount)
    {

        foreach (var orb in orbUIs)
        {
            if (orb.type == type && orb.slot != null)
            {
                ResourceData data = GetResourceData(type);
                orb.slot.SetOrb(data, amount, orb.maxValue);
                return;
            }
        }


        foreach (var slot in otherUIs)
        {
            if (slot.type == type && slot.amountText != null)
            {
                slot.amountText.text = amount.ToString();
                return;
            }
        }
    }

    private ResourceData GetResourceData(ResourceType type)
    {
        if (dataMap.TryGetValue(type, out var data))
            return data;
        else
            return null;
    }

    public void ShowResourcePopup(string resourceName, int amount, Vector3 worldPosition)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        GameObject popupGO = Instantiate(resourcePopupPrefab, screenPos, Quaternion.identity, mainCanvas.transform);
        ResourcePopup popup = popupGO.GetComponent<ResourcePopup>();
        popup.Setup(resourceName, amount);
    }

    public void ShowSpecialQuestPopup(Vector3 worldPosition)
    {
        string message = Random.value < 0.5f
            ? "The Unity believes in Moussey..."
            : "Your devotion has been noticed.";

        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        GameObject popupGO = Instantiate(specialQuestPopupPrefab, screenPos, Quaternion.identity, mainCanvas.transform);

        SpecialQuestPopup popup = popupGO.GetComponent<SpecialQuestPopup>();
        if (popup != null) popup.Setup(message);
    }

    public void ShowQuestUI(Quest quest)
    {
        questUIPanel.SetActive(true);

        string titlePrefix = quest.questTitle.Contains("The Unity") ? "Request from " : "Order from ";
        questTitleText.text = titlePrefix + quest.questTitle;

        foreach (GameObject go in activeResourceSlots)
            Destroy(go);
        activeResourceSlots.Clear();

        foreach (var req in quest.requiredResources)
        {
            GameObject slot = Instantiate(questResourceSlotPrefab, questResourceListContainer);
            ResourceData data = ResourceManager.Instance.GetResourceData(req.resourceType);
            slot.transform.Find("icon").GetComponent<Image>().sprite = data.icon;
            slot.transform.Find("text").GetComponent<TextMeshProUGUI>().text =
                $"{ResourceManager.Instance.GetAmount(req.resourceType)} / {req.requiredAmount}";

            activeResourceSlots.Add(slot);
        }
    }

    public void HideQuestUI()
    {
        questUIPanel.SetActive(false);
    }

    public void UpdateQuestTimer(float remainingTime)
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        questTimerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void UpdateQuestResources(Quest quest)
    {
        for (int i = 0; i < activeResourceSlots.Count; i++)
        {
            var req = quest.requiredResources[i];
            string text = $"{ResourceManager.Instance.GetAmount(req.resourceType)} / {req.requiredAmount}";

            Transform slot = activeResourceSlots[i].transform; 
            slot.Find("text").GetComponent<TextMeshProUGUI>().text = text;
        }
    }

    public void ShowGameOverUI(string message)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            PauseManager.Instance.LockPauseInput(true); 
            Time.timeScale = 0f;
            TextMeshProUGUI msg = gameOverPanel.GetComponentInChildren<TextMeshProUGUI>();
            if (msg != null)
                msg.text = message;
        }
    }

    public void ShowWinUI()
    {
        winPanel.SetActive(true);
        PauseManager.Instance.LockPauseInput(true);
        Time.timeScale = 0f;
    }

    public void UpdateToleranceUI(ResourceType giver, int remaining)
    {
        foreach (var orb in orbUIs)
        {
            if (orb.type == giver && orb.slot != null)
            {
                Transform toleranceText = orb.slot.transform.Find("ToleranceText");
                if (toleranceText != null)
                {
                    toleranceText.GetComponent<TextMeshProUGUI>().text = remaining.ToString();
                }
                break;
            }
        }
    }

    public void ResetManager()
    {
        foreach (var orb in orbUIs)
        {
            int remaining = GameManager.Instance.GiverToleranceManager.GetRemainingTolerance(orb.type);
            UpdateToleranceUI(orb.type, remaining);
        }


        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

}