using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Linq;

public class MapManager : MonoBehaviour
{
    public enum BiomeType { Forest, Snow, Desert, Corruption, Sacred }

    [Header("Quest Checker")]
    public GameObject questCheckerPrefab;

    [Header("Tilemaps")]
    public Tilemap groundTilemap;

    [Header("Biome Tiles")]
    public Tile forestTile;
    public Tile snowTile;
    public Tile desertTile;
    public Tile corruptionTile;
    public Tile sacredTile;

    [Header("Map Settings")]
    public int mapWidth = 500;
    public int mapHeight = 500;
    public int chunkSize = 50;

    [Header("References")]
    public Transform mousseyTransform;

    [Header("Resource Prefabs")]
    public GameObject forestTreePrefab;
    public GameObject snowTreePrefab;
    public GameObject corruptionTreePrefab;
    public GameObject sacredTreePrefab;
    public GameObject desertTreePrefab;
    public GameObject firePlantPrefab;
    public GameObject airPlantPrefab;
    public GameObject soulPlantPrefab;
    public GameObject snowCrystalPrefab;
    public GameObject voidCrystalPrefab;
    public GameObject stonePrefab;
    public GameObject snowStonePrefab;
    public GameObject soulShardPrefab;
    public GameObject elementalShardPrefab;
    public GameObject lifeShardPrefab;
    public GameObject airCrystalPrefab;
    public GameObject fireCrystalPrefab;

    [Header("Singularity")]
    public GameObject singularityPrefab;


    private BiomeType[,] mapBiomes;


    void GenerateBiomeMap()
    {
        int chunkCountX = Mathf.CeilToInt((float)mapWidth / chunkSize);
        int chunkCountY = Mathf.CeilToInt((float)mapHeight / chunkSize);
        mapBiomes = new BiomeType[mapWidth, mapHeight];

        List<Vector2Int> allChunks = new List<Vector2Int>();
        for (int x = 0; x < chunkCountX; x++)
            for (int y = 0; y < chunkCountY; y++)
                allChunks.Add(new Vector2Int(x, y));

        Vector2Int centerChunk = new Vector2Int(chunkCountX / 2, chunkCountY / 2);
        BiomeType[] biomePool = new BiomeType[] {
        BiomeType.Forest, BiomeType.Snow, BiomeType.Desert, BiomeType.Corruption, BiomeType.Sacred
    };

        List<Vector2Int> remainingChunks = new List<Vector2Int>(allChunks);
        remainingChunks.Remove(centerChunk);
        remainingChunks = remainingChunks.OrderBy(c => Random.value).ToList();

        List<BiomeType> requiredBiomes = new List<BiomeType> {
        BiomeType.Snow, BiomeType.Desert, BiomeType.Corruption, BiomeType.Sacred
    }.OrderBy(b => Random.value).ToList();

        AssignBiomeToChunk(centerChunk, BiomeType.Forest);

        for (int i = 0; i < requiredBiomes.Count; i++)
        {
            AssignBiomeToChunk(remainingChunks[i], requiredBiomes[i]);
        }


        for (int i = requiredBiomes.Count; i < remainingChunks.Count; i++)
        {
            BiomeType biome = biomePool[Random.Range(0, biomePool.Length)];
            AssignBiomeToChunk(remainingChunks[i], biome);
        }
    }

    void GenerateTerrain()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                TileBase tile = GetTileFromBiome(mapBiomes[x, y]);
                if (tile != null)
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    TileBase GetTileFromBiome(BiomeType biome)
    {
        switch (biome)
        {
            case BiomeType.Forest: return forestTile;
            case BiomeType.Snow: return snowTile;
            case BiomeType.Desert: return desertTile;
            case BiomeType.Corruption: return corruptionTile;
            case BiomeType.Sacred: return sacredTile;
            default: return null;
        }
    }

    private void SpawnQuestChecker()
    {
        if (questCheckerPrefab == null)
        {
           // Debug.LogWarning("QuestCheckerPrefab not assigned in MapManager.");
            return;
        }

        Vector3 centerPos = new Vector3(mapWidth / 2f, mapHeight / 2f, 0f);
        Instantiate(questCheckerPrefab, centerPos, Quaternion.identity);
    }


    void SpawnResources()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3 worldPos = new Vector3(x + 0.5f, y + 0.5f, 0f);
                BiomeType biome = mapBiomes[x, y];

                // TREES
                if (biome == BiomeType.Forest && Random.value < 0.005f)
                {
                    Instantiate(forestTreePrefab, worldPos, Quaternion.identity);
                }
                if (biome == BiomeType.Snow && Random.value < 0.005f)
                {
                    Instantiate(snowTreePrefab, worldPos, Quaternion.identity);
                }
                if (biome == BiomeType.Corruption && Random.value < 0.005f)
                {
                    Instantiate(corruptionTreePrefab, worldPos, Quaternion.identity);
                }
                if (biome == BiomeType.Sacred && Random.value < 0.005f)
                {
                    Instantiate(sacredTreePrefab, worldPos, Quaternion.identity);
                }
                if (biome == BiomeType.Desert && Random.value < 0.003f)
                {
                    Instantiate(desertTreePrefab, worldPos, Quaternion.identity);
                }

                // PLANTS
                if (biome == BiomeType.Desert && Random.value < 0.003f)
                {
                    Instantiate(firePlantPrefab, worldPos, Quaternion.identity);
                }
                if ((biome == BiomeType.Forest || biome == BiomeType.Sacred || biome == BiomeType.Desert) && Random.value < 0.002f)
                {
                    Instantiate(airPlantPrefab, worldPos, Quaternion.identity);
                }
                if (biome == BiomeType.Sacred && Random.value < 0.003f)
                {
                    Instantiate(soulPlantPrefab, worldPos, Quaternion.identity);
                }

                // CRYSTALS
                if (biome == BiomeType.Snow && Random.value < 0.003f)
                {
                    Instantiate(snowCrystalPrefab, worldPos, Quaternion.identity);
                }
                if (biome == BiomeType.Corruption && Random.value < 0.003f)
                {
                    Instantiate(voidCrystalPrefab, worldPos, Quaternion.identity);
                }
                if (biome == BiomeType.Forest && Random.value < 0.002f)
                {
                    Instantiate(airCrystalPrefab, worldPos, Quaternion.identity);
                }
                if (biome == BiomeType.Desert && Random.value < 0.002f)
                {
                    Instantiate(fireCrystalPrefab, worldPos, Quaternion.identity);
                }

                // ROCKS
                if ((biome == BiomeType.Forest || biome == BiomeType.Desert || biome == BiomeType.Sacred) && Random.value < 0.002f)
                {
                    Instantiate(stonePrefab, worldPos, Quaternion.identity);
                }
                if (biome == BiomeType.Snow && Random.value < 0.003f)
                {
                    Instantiate(snowStonePrefab, worldPos, Quaternion.identity);
                }

                // SHARDS
                if (biome == BiomeType.Sacred && Random.value < 0.001f)
                {
                    Instantiate(soulShardPrefab, worldPos, Quaternion.identity);
                }
                if ((biome == BiomeType.Forest || biome == BiomeType.Desert) && Random.value < 0.0003f)
                {
                    Instantiate(elementalShardPrefab, worldPos, Quaternion.identity);
                }
                if ((biome == BiomeType.Forest || biome == BiomeType.Sacred) && Random.value < 0.0003f)
                {
                    Instantiate(lifeShardPrefab, worldPos, Quaternion.identity);
                }

                // SINGULARITY
                if (Random.value < 0.00001f)
                {
                    Instantiate(singularityPrefab, worldPos, Quaternion.identity);
                }

            }
        }
    }

    public BiomeType[,] GetBiomeMap()
    {
        return mapBiomes;
    }

    private void SetCameraBounds()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * cam.aspect;

        float minX = horzExtent;
        float maxX = mapWidth - horzExtent;
        float minY = vertExtent;
        float maxY = mapHeight - vertExtent;

        SmoothCameraFollow camFollow = cam.GetComponent<SmoothCameraFollow>();
        if (camFollow != null)
        {
            camFollow.minCameraPos = new Vector2(minX, minY);
            camFollow.maxCameraPos = new Vector2(maxX, maxY);
        }
    }

    private void AssignBiomeToChunk(Vector2Int chunk, BiomeType biome)
    {
        int startX = chunk.x * chunkSize;
        int startY = chunk.y * chunkSize;

        for (int x = 0; x < chunkSize && startX + x < mapWidth; x++)
        {
            for (int y = 0; y < chunkSize && startY + y < mapHeight; y++)
            {
                mapBiomes[startX + x, startY + y] = biome;
            }
        }
    }

    public void ResetManager()
    {
        groundTilemap.ClearAllTiles();

        foreach (var obj in GameObject.FindGameObjectsWithTag("Resource"))
            Destroy(obj);

        foreach (var obj in GameObject.FindGameObjectsWithTag("QuestChecker"))
            Destroy(obj);

        GenerateBiomeMap();
        GenerateTerrain();
        SpawnResources();
        SpawnQuestChecker();

        if (mousseyTransform != null)
            mousseyTransform.position = new Vector3(mapWidth / 2f + 0.5f, mapHeight / 2f + 0.5f, 0f);

        SetCameraBounds();

        // Debug.Log("[MAP MANAGER] Reset completed.");
    }
}


