using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private Dictionary<ResourceType, int> resourceAmounts = new Dictionary<ResourceType, int>();
    private Dictionary<ResourceType, float> resourceMultipliers = new Dictionary<ResourceType, float>();
    public List<ResourceData> resourceDataList;
    private Dictionary<ResourceType, ResourceData> resourceDataDict = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
        {
            resourceAmounts[type] = 0;
            resourceMultipliers[type] = 1f;
        }
        foreach (var data in resourceDataList)
        {
            resourceDataDict[data.type] = data;
        }
    }

    public void AddResource(ResourceType type, int amount, Vector3? worldPosition = null)
    {
        if (!resourceAmounts.ContainsKey(type))
            resourceAmounts[type] = 0;

        int finalAmount = GetEffectiveAmount(type, amount);
        resourceAmounts[type] += finalAmount;

        GameManager.Instance.uiManager.UpdateResourceUI(type, resourceAmounts[type]);

        if (worldPosition.HasValue)
            GameManager.Instance.uiManager.ShowResourcePopup(type.ToString(), finalAmount, worldPosition.Value);
    }

    public int GetAmount(ResourceType type)
    {
        return resourceAmounts.ContainsKey(type) ? resourceAmounts[type] : 0;
    }

    public void SetAmount(ResourceType type, int value)
    {
        resourceAmounts[type] = value;
        GameManager.Instance.uiManager.UpdateResourceUI(type, value);
    }

    public void AddMultiplier(ResourceType type, float multiplier)
    {
        if (!resourceMultipliers.ContainsKey(type))
            resourceMultipliers[type] = 1f;

        resourceMultipliers[type] *= multiplier;
    }

    public int GetEffectiveAmount(ResourceType type, int baseAmount)
    {
        if (!resourceMultipliers.ContainsKey(type))
            return baseAmount;

        return Mathf.RoundToInt(baseAmount * resourceMultipliers[type]);
    }

    public void SpendResource(ResourceType type, int amount)
    {
        if (!resourceAmounts.ContainsKey(type)) return;
        resourceAmounts[type] = Mathf.Max(0, resourceAmounts[type] - amount);
        GameManager.Instance.uiManager.UpdateResourceUI(type, resourceAmounts[type]);
    }

    public ResourceData GetResourceData(ResourceType type)
    {
        return resourceDataDict[type];
    }

    public void ResetManager()
    {
        foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
        {
            resourceAmounts[type] = 0;
            resourceMultipliers[type] = 1f;

            GameManager.Instance.uiManager.UpdateResourceUI(type, 0);
        }

      //  Debug.Log("[RESOURCE MANAGER] Reset completed.");
    }
}