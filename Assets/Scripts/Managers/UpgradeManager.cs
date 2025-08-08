using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [SerializeField] private List<Upgrade> initialPool;
    private List<Upgrade> currentPool = new List<Upgrade>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        currentPool.AddRange(initialPool);
    }

    public List<Upgrade> GetRandomUpgrades(int count)
    {
        List<Upgrade> selection = new List<Upgrade>();
        int attempts = 0;

        while (selection.Count < count && attempts < 50)
        {
            Rarity rolledRarity = RollRarity();
            Upgrade chosen = PickRandomFromRarity(rolledRarity);

            if (chosen != null && !selection.Contains(chosen))
                selection.Add(chosen);

            attempts++;
        }

        return selection;
    }

    public void OnUpgradeChosen(Upgrade chosen)
    {
        ApplyEffect(chosen.upgradeName);

        if (currentPool.Contains(chosen))
            currentPool.Remove(chosen);

        if (chosen.unlocksOnPick != null)
        {
            foreach (var up in chosen.unlocksOnPick)
            {
                if (!currentPool.Contains(up))
                    currentPool.Add(up);
            }
        }
    }

    private void ApplyEffect(string upgradeName)
    {
        var player = PlayerManager.Instance;

        switch (upgradeName)
        {
            case "Base Damage+":
                player.IncreaseBaseDamage(2);
                break;
            case "Base Damage+1":
                player.IncreaseBaseDamage(3);
                break;
            case "Base Damage+2":
                player.IncreaseBaseDamage(4);
                break;
            case "Base Damage+3":
                player.IncreaseBaseDamage(5);
                break;

            case "Critical Chance+":
                PlayerManager.Instance.AddCritChance(0.02f);
                break;
            case "Critical Chance+1":
                PlayerManager.Instance.AddCritChance(0.05f);
                break;
            case "Critical Chance+2":
                PlayerManager.Instance.AddCritChance(0.1f);
                break;

            case "Critical Damage+":
                PlayerManager.Instance.AddCritMultiplier(0.2f);
                break;
            case "Critical Damage+1":
                PlayerManager.Instance.AddCritMultiplier(0.3f);
                break;
            case "Critical Damage+2":
                PlayerManager.Instance.AddCritMultiplier(0.5f);
                break;

            case "Fire Gain+":
                ResourceManager.Instance.AddMultiplier(ResourceType.Fire, 1.3f);
                break;
            case "Fire Gain+1":
                ResourceManager.Instance.AddMultiplier(ResourceType.Fire, 1.5f);
                break;
            case "Fire Gain+2":
                ResourceManager.Instance.AddMultiplier(ResourceType.Fire, 1.7f);
                break;
            case "Fire Gain+3":
                ResourceManager.Instance.AddMultiplier(ResourceType.Fire, 2f);
                break;

            case "Void Gain+":
                ResourceManager.Instance.AddMultiplier(ResourceType.Void, 1.3f);
                break;
            case "Void Gain+1":
                ResourceManager.Instance.AddMultiplier(ResourceType.Void, 1.5f);
                break;
            case "Void Gain+2":
                ResourceManager.Instance.AddMultiplier(ResourceType.Void, 1.7f);
                break;
            case "Void Gain+3":
                ResourceManager.Instance.AddMultiplier(ResourceType.Void, 2f);
                break;

            case "Earth Gain+":
                ResourceManager.Instance.AddMultiplier(ResourceType.Earth, 1.3f);
                break;
            case "Earth Gain+1":
                ResourceManager.Instance.AddMultiplier(ResourceType.Earth, 1.5f);
                break;
            case "Earth Gain+2":
                ResourceManager.Instance.AddMultiplier(ResourceType.Earth, 1.7f);
                break;
            case "Earth Gain+3":
                ResourceManager.Instance.AddMultiplier(ResourceType.Earth, 2f);
                break;

            case "Hope Gain+":
                ResourceManager.Instance.AddMultiplier(ResourceType.Hope, 1.3f);
                break;
            case "Hope Gain+1":
                ResourceManager.Instance.AddMultiplier(ResourceType.Hope, 1.5f);
                break;
            case "Hope Gain+2":
                ResourceManager.Instance.AddMultiplier(ResourceType.Hope, 1.7f);
                break;
            case "Hope Gain+3":
                ResourceManager.Instance.AddMultiplier(ResourceType.Hope, 2f);
                break;

            case "Air Gain+":
                ResourceManager.Instance.AddMultiplier(ResourceType.Air, 1.3f);
                break;
            case "Air Gain+1":
                ResourceManager.Instance.AddMultiplier(ResourceType.Air, 1.5f);
                break;
            case "Air Gain+2":
                ResourceManager.Instance.AddMultiplier(ResourceType.Air, 1.7f);
                break;
            case "Air Gain+3":
                ResourceManager.Instance.AddMultiplier(ResourceType.Air, 2f);
                break;

            case "Cold Gain+":
                ResourceManager.Instance.AddMultiplier(ResourceType.Cold, 1.3f);
                break;
            case "Cold Gain+1":
                ResourceManager.Instance.AddMultiplier(ResourceType.Cold, 1.5f);
                break;
            case "Cold Gain+2":
                ResourceManager.Instance.AddMultiplier(ResourceType.Cold, 1.7f);
                break;
            case "Cold Gain+3":
                ResourceManager.Instance.AddMultiplier(ResourceType.Cold, 2f);
                break;

            case "Wood Gain+":
                ResourceManager.Instance.AddMultiplier(ResourceType.Wood, 1.3f);
                break;
            case "Wood Gain+1":
                ResourceManager.Instance.AddMultiplier(ResourceType.Wood, 1.5f);
                break;
            case "Wood Gain+2":
                ResourceManager.Instance.AddMultiplier(ResourceType.Wood, 1.7f);
                break;
            case "Wood Gain+3":
                ResourceManager.Instance.AddMultiplier(ResourceType.Wood, 2f);
                break;

            case "Stone Gain+":
                ResourceManager.Instance.AddMultiplier(ResourceType.Stone, 1.3f);
                break;
            case "Stone Gain+1":
                ResourceManager.Instance.AddMultiplier(ResourceType.Stone, 1.5f);
                break;
            case "Stone Gain+2":
                ResourceManager.Instance.AddMultiplier(ResourceType.Stone, 1.7f);
                break;
            case "Stone Gain+3":
                ResourceManager.Instance.AddMultiplier(ResourceType.Stone, 2f);
                break;

            case "Crystal Gain+":
                ResourceManager.Instance.AddMultiplier(ResourceType.Crystal, 1.3f);
                break;
            case "Crystal Gain+1":
                ResourceManager.Instance.AddMultiplier(ResourceType.Crystal, 1.5f);
                break; 
            case "Crystal Gain+2":
                ResourceManager.Instance.AddMultiplier(ResourceType.Crystal, 1.7f);
                break;
            case "Crystal Gain+3":
                ResourceManager.Instance.AddMultiplier(ResourceType.Crystal, 2f);
                break;

            case "Soul Gain+":
                ResourceManager.Instance.AddMultiplier(ResourceType.Soul, 1.3f);
                break;
            case "Soul Gain+1":
                ResourceManager.Instance.AddMultiplier(ResourceType.Soul, 1.5f);
                break; 
            case "Soul Gain+2":
                ResourceManager.Instance.AddMultiplier(ResourceType.Soul, 1.7f);
                break; 
            case "Soul Gain+3":
                ResourceManager.Instance.AddMultiplier(ResourceType.Soul, 2f);
                break;                                                                   



            default:
                Debug.LogWarning("No case found:" + upgradeName);
                break;
        }
    }

    private Rarity RollRarity()
    {
        float roll = Random.Range(0f, 100f);

        if (roll < 0.5f) return Rarity.Mystic;
        if (roll < 4f) return Rarity.Legendary;
        if (roll < 15f) return Rarity.Epic;
        if (roll < 40f) return Rarity.Rare;
        return Rarity.Common;
    }

    private Upgrade PickRandomFromRarity(Rarity rarity)
    {
        List<Upgrade> filtered = currentPool.FindAll(up => up.rarity == rarity);

        if (filtered.Count > 0)
            return filtered[Random.Range(0, filtered.Count)];

        switch (rarity)
        {
            case Rarity.Mystic: return PickRandomFromRarity(Rarity.Legendary);
            case Rarity.Legendary: return PickRandomFromRarity(Rarity.Epic);
            case Rarity.Epic: return PickRandomFromRarity(Rarity.Rare);
            case Rarity.Rare: return PickRandomFromRarity(Rarity.Common);
            case Rarity.Common: return GetAnyAvailableUpgrade();
            default: return null;
        }
    }

    private Upgrade GetAnyAvailableUpgrade()
    {
        if (currentPool.Count == 0) return null;
        return currentPool[Random.Range(0, currentPool.Count)];
    }

    public void ResetManager()
    {
        currentPool.Clear();
        currentPool.AddRange(initialPool);
      //  Debug.Log("[UPGRADE MANAGER] Reset completed.");
    }
}
