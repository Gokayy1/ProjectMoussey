using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiverToleranceManager : MonoBehaviour
{
    public static GiverToleranceManager Instance;

    [System.Serializable]
    public class GiverTolerance
    {
        public ResourceType giverType;
        public int maxFailures = 1;
        public int currentFailures = 0;
    }

    public List<GiverTolerance> giverTolerances = new List<GiverTolerance>();

    private Dictionary<ResourceType, GiverTolerance> toleranceMap = new();

    void Awake()
    {
        foreach (var gt in giverTolerances)
        {
            toleranceMap[gt.giverType] = gt;
        }
    }

    public void RegisterFailure(ResourceType giver)
    {
        if (giver == ResourceType.Hope) return;

        if (!toleranceMap.ContainsKey(giver)) return;

        toleranceMap[giver].currentFailures++;

        // UI'yi güncelle (UIManager üzerinden)
        int remaining = Mathf.Max(0, toleranceMap[giver].maxFailures - toleranceMap[giver].currentFailures);
        GameManager.Instance.uiManager.UpdateToleranceUI(giver, remaining);

        
    /*    if (toleranceMap[giver].currentFailures >= toleranceMap[giver].maxFailures)
        {
            Debug.Log($"{giver} toleransı tükendi.");
        } */
    }

    public int GetRemainingTolerance(ResourceType giver)
    {
        if (!toleranceMap.ContainsKey(giver)) return -1;
        var t = toleranceMap[giver];
        return Mathf.Max(0, t.maxFailures - t.currentFailures);
    }

    public void ResetManager()
    {
        foreach (var gt in giverTolerances)
        {
            gt.currentFailures = 0;

            // UI güncelle
            GameManager.Instance.uiManager.UpdateToleranceUI(gt.giverType, gt.maxFailures);
        }

      //  Debug.Log("[GIVER TOLERANCE MANAGER] Reset completed.");
    }
}
