using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeData", menuName = "Moussey/Biome")]
public class BiomeData : ScriptableObject
{
    public string biomeName;
    public Sprite background;
    public GameObject[] resourcePrefabs;
    public GameObject[] enemyPrefabs;
    public float temperature; 
    public bool isHostile;
}
