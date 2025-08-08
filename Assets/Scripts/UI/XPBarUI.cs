using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBarUI : MonoBehaviour
{
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI levelText;

    void Start()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnXPChanged += UpdateXPUI;
            UpdateXPUI(PlayerManager.Instance.currentXP, PlayerManager.Instance.xpToNextLevel);
        }
    }

    void UpdateXPUI(int currentXP, int xpToNextLevel)
    {
        xpSlider.maxValue = xpToNextLevel;
        xpSlider.value = currentXP;

        if (levelText != null)
            levelText.text = "Lv " + PlayerManager.Instance.level;
    }
}