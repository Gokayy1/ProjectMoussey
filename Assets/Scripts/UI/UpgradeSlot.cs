using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class UpgradeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI upgradeNameText;

    private Action onClick;

    public void Setup(Upgrade upgrade, Action callback)
    {
        if (icon != null) icon.sprite = upgrade.icon;
        if (upgradeNameText != null)
        {
            upgradeNameText.text = upgrade.upgradeName;
            upgradeNameText.color = GetColorForRarity(upgrade.rarity);
        }
        onClick = callback;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }

    private Color GetColorForRarity(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return Color.white;
            case Rarity.Rare: return new Color32(70, 165, 255, 255);        // mavi
            case Rarity.Epic: return new Color32(187, 104, 255, 255);       // mor
            case Rarity.Legendary: return new Color32(255, 190, 40, 255);   // altın
            case Rarity.Mystic: return new Color32(115, 255, 252, 255);     // turkuaz
            default: return Color.gray;
        }
    }
}