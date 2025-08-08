using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrbSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI amountText;

    public void SetOrb(ResourceData data, int amount, int max)
    {
        if (icon != null && data.icon != null)
            icon.sprite = data.icon;

        if (amountText != null)
            amountText.text = amount.ToString();
    }
}