using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceRegrowManager : MonoBehaviour
{
    [System.Serializable]
    public class TrackedResource
    {
        public GameObject prefab;
        public int originalCount;
        public int currentCount;
        public int currentThreshold;
    }

    public float checkInterval = 10f;
    private float timer = 0f;

    public List<TrackedResource> trackedResources = new();

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            timer = 0f;
            CheckAndRegrow();
        }
    }

    public void RegisterInitialResources()
    {
        trackedResources.Clear();

        GameObject[] all = GameObject.FindGameObjectsWithTag("Resource");
        Dictionary<string, (GameObject prefab, int count)> tempCounts = new();

        foreach (var obj in all)
        {
            var identity = obj.GetComponent<ResourceIdentity>();
            if (identity == null || identity.sourcePrefab == null)
                continue;

            string key = identity.sourcePrefab.name;

            if (!tempCounts.ContainsKey(key))
                tempCounts[key] = (identity.sourcePrefab, 0);

            tempCounts[key] = (identity.sourcePrefab, tempCounts[key].count + 1);
        }

        int totalResources = 0;
        foreach (var kvp in tempCounts)
        {
            trackedResources.Add(new TrackedResource
            {
                prefab = kvp.Value.prefab,
                originalCount = kvp.Value.count,
                currentCount = kvp.Value.count,
                currentThreshold = Mathf.FloorToInt(kvp.Value.count * 0.5f)
            });

            totalResources += kvp.Value.count;
        }

      //  Debug.Log($"[REGROW] Registered {trackedResources.Count} resource types, total {totalResources} resource objects.");
    }

   /* public void NotifyDestroyed(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("[REGROW] NotifyDestroyed called with null prefab!");
            return;
        }

        foreach (var r in trackedResources)
        {
            if (r.prefab == null)
                continue;

            if (r.prefab.name == prefab.name || r.prefab.name + "(Clone)" == prefab.name)
            {
                r.currentCount = Mathf.Max(0, r.currentCount - 1);
                Debug.Log($"[REGROW] {r.prefab.name} destroyed. Remaining: {r.currentCount} / {r.originalCount} (Threshold: {r.currentThreshold})");
                return;
            }
        }

        Debug.LogWarning($"[REGROW] NotifyDestroyed called but no matching prefab found for {prefab.name}");
    } */

    private void CheckAndRegrow()
    {
        foreach (var r in trackedResources)
        {
            if (r.currentCount <= r.currentThreshold)
            {
                int toSpawn = Mathf.CeilToInt(r.originalCount * 0.25f);
                r.currentCount += toSpawn;
                r.currentThreshold = Mathf.FloorToInt(r.currentCount * 0.5f);

                for (int i = 0; i < toSpawn; i++)
                {
                    Vector3 pos = GetRandomPositionOnMap();
                    Instantiate(r.prefab, pos, Quaternion.identity);
                }

              //  Debug.Log($"[REGROW] {r.prefab.name} regenerated: +{toSpawn}");
            }
        }
    }

    private Vector3 GetRandomPositionOnMap()
    {
        float x = Random.Range(0f, GameManager.Instance.mapManager.mapWidth);
        float y = Random.Range(0f, GameManager.Instance.mapManager.mapHeight);
        return new Vector3(x + 0.5f, y + 0.5f, 0f);
    }
}
