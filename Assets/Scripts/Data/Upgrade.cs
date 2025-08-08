using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary,
    Mystic
}

[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrades/New Upgrade")]
public class Upgrade : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string description;
    public Sprite icon;
    public Rarity rarity = Rarity.Common;
    public Upgrade[] unlocksOnPick; 

    public Action applyEffect;
}
