using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Manager References")]
    public MapManager mapManager;
    public GiverToleranceManager GiverToleranceManager;
    public UIManager uiManager;
    public EnemyManager enemyManager;
    public PlayerManager playerManager;
    public ResourceManager resourceManager;
    public UpgradeManager upgradeManager;
    public QuestManager questManager;
    public ResourceRegrowManager resourceRegrowManager;


    void Start()
    {
        if (uiManager == null)
            uiManager = FindObjectOfType<UIManager>();
    }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void FindUIManager()
    {
        uiManager = FindObjectOfType<UIManager>();
      //  if (uiManager == null)
        //    Debug.LogError("[GameManager] UIManager not found in scene!");
      //  else
         //   Debug.Log("[GameManager] UIManager successfully found.");
    }

    public void FindAllManagers()
    {
      //  Debug.Log("[GameManager] Finding all managers...");

        uiManager = FindObjectOfType<UIManager>();
      //  if (uiManager != null)
          //  Debug.Log("[GameManager] UIManager successfully found.");
     //   else
         //   Debug.LogWarning("[GameManager] UIManager not found.");

        questManager = FindObjectOfType<QuestManager>();
      //  if (questManager != null)
           // Debug.Log("[GameManager] QuestManager successfully found.");

        mapManager = FindObjectOfType<MapManager>();
        if (mapManager != null)
        {
          //  Debug.Log("[GameManager] MapManager successfully found.");

            if (mapManager.groundTilemap == null)
            {
                Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();
                foreach (var t in tilemaps)
                {
                    if (t.name.ToLower().Contains("ground"))
                    {
                        mapManager.groundTilemap = t;
                      //  Debug.Log("[GameManager] GroundTilemap reassigned.");
                        break;
                    }
                }

                if (mapManager.groundTilemap == null)
                    Debug.LogWarning("[GameManager] GroundTilemap not found.");
            }
        }

        playerManager = FindObjectOfType<PlayerManager>();
        if (playerManager != null)
        {
           // Debug.Log("[GameManager] PlayerManager successfully found.");
            playerManager.FindUpgradeUI(); // ← burada çağırıyoruz
        }

        resourceManager = FindObjectOfType<ResourceManager>();
      //  if (resourceManager != null)
         //   Debug.Log("[GameManager] ResourceManager successfully found.");

        upgradeManager = FindObjectOfType<UpgradeManager>();
      //  if (upgradeManager != null)
         //   Debug.Log("[GameManager] UpgradeManager successfully found.");

        GiverToleranceManager = FindObjectOfType<GiverToleranceManager>();
      //  if (GiverToleranceManager != null)
          //  Debug.Log("[GameManager] GiverToleranceManager successfully found.");

        enemyManager = FindObjectOfType<EnemyManager>();
      //  if (enemyManager != null)
          //  Debug.Log("[GameManager] EnemyManager successfully found.");

        resourceRegrowManager = FindObjectOfType<ResourceRegrowManager>();
      //  if (resourceRegrowManager != null)
          //  Debug.Log("[GameManager] ResourceRegrowManager successfully found.");

          //  Debug.Log("[GameManager] All manager references updated.");
    }
}
