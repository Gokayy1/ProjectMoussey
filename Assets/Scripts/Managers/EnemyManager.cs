using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [Header("Enemy Prefab")]
    public GameObject corruptionEnemyPrefab;

    [Header("Enemy Settings")]
    public float corruptionSpawnChance = 0.0001f;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private MapManager.BiomeType[,] mapBiomes;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SpawnEnemies(MapManager.BiomeType[,] biomes, int mapWidth, int mapHeight)
    {
        mapBiomes = biomes;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (mapBiomes[x, y] == MapManager.BiomeType.Corruption && Random.value < corruptionSpawnChance)
                {
                    Vector3 spawnPos = new Vector3(x + 0.5f, y + 0.5f, 0f);
                    GameObject enemy = Instantiate(corruptionEnemyPrefab, spawnPos, Quaternion.identity);
                    spawnedEnemies.Add(enemy);
                }
            }
        }

      //  Debug.Log($"[ENEMY MANAGER] Spawned {spawnedEnemies.Count} enemies.");
    }

    public void ResetManager()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        spawnedEnemies.Clear();

        if (GameManager.Instance.mapManager != null)
        {
            mapBiomes = GameManager.Instance.mapManager.GetBiomeMap();
            SpawnEnemies(mapBiomes, GameManager.Instance.mapManager.mapWidth, GameManager.Instance.mapManager.mapHeight);
        }

      //  Debug.Log("[ENEMY MANAGER] Reset completed.");
    }
}