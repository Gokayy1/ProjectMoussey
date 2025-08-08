using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest/Create New Quest")]
public class Quest : ScriptableObject
{
    [Header("General Info")]
    public string questTitle;
    public Sprite giverIcon;
    public float timeLimit; // Seconds
    public ResourceType giver;

    [Header("Resource Requirements")]
    public List<ResourceRequirement> requiredResources = new List<ResourceRequirement>();
}

[Serializable]
public class ResourceRequirement
{
    public ResourceType resourceType;
    public int requiredAmount;
}