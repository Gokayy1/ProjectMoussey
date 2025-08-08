using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewResourceData", menuName = "Moussey/ResourceData")]
public class ResourceData : ScriptableObject
{
    public ResourceType type;
    public string displayName;
    public Sprite icon;
}
