using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [Header("Combat Stats")]
    [SerializeField] private int baseDamage = 5;
    private int initialBaseDamage;

    [Header("XP Stats")]
    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;
    [SerializeField] private UpgradeUI upgradeUI;

    [Header("Critical Stats")]
    [SerializeField] private float critChance = 0.03f;
    [SerializeField] private float critMultiplier = 2f;
    private float initialCritChance;
    private float initialCritMultiplier;

    public event Action<int, int> OnXPChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        initialBaseDamage = baseDamage;
        initialCritChance = critChance;
        initialCritMultiplier = critMultiplier;
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        OnXPChanged?.Invoke(currentXP, xpToNextLevel);
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            level++;
            xpToNextLevel = Mathf.FloorToInt(250 * Mathf.Pow(level, 1.65f));
            OnXPChanged?.Invoke(currentXP, xpToNextLevel);

            OpenUpgradePanel();
        }
    }

    private void OpenUpgradePanel()
    {
        if (upgradeUI != null)
        {
            upgradeUI.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public int GetBaseDamage() => baseDamage;
    public float GetCritChance() => critChance;
    public float GetCritMultiplier() => critMultiplier;

    public void AddCritChance(float amount) => critChance += amount;
    public void SetCritChance(float value) => critChance = value;
    public void AddCritMultiplier(float amount) => critMultiplier += amount;
    public void SetCritMultiplier(float value) => critMultiplier = value;

    public bool TryCrit()
    {
        return UnityEngine.Random.value < critChance;
    }

    public int GetBonusDamageFor(ResourceOnHarvest resource)
    {
        return 0;
    }

    public void IncreaseBaseDamage(int amount)
    {
        baseDamage += amount;
    }

    public void ResetManager()
    {
        level = 1;
        currentXP = 0;
        xpToNextLevel = 100;

        baseDamage = initialBaseDamage;
        critChance = initialCritChance;
        critMultiplier = initialCritMultiplier;

        OnXPChanged?.Invoke(currentXP, xpToNextLevel);

        if (upgradeUI != null)
        {
            upgradeUI.gameObject.SetActive(false);
        }

      //  Debug.Log("[PLAYER MANAGER] Reset completed.");
    }

    public void FindUpgradeUI()
    {
        if (upgradeUI == null)
        {
            upgradeUI = FindObjectOfType<UpgradeUI>(true);
           // if (upgradeUI != null)
             //   Debug.Log("[PlayerManager] Upgrade UI successfully found and assigned.");
          //  else
            //    Debug.LogError("[PlayerManager] Upgrade UI referansı atanamadı!");
        }
    }
}
