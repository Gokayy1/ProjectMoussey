using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MouseClickHandler : MonoBehaviour
{
    [Header("Layer Mask")]
    public LayerMask interactableLayer = 1 << 6;

    [Header("Crit Effect")]
    [SerializeField] private GameObject critPopupPrefab;
    [SerializeField] private Canvas mainCanvas;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (PauseManager.Instance.IsPaused()) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    void HandleClick()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 clickPos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        RaycastHit2D hit = Physics2D.Raycast(clickPos2D, Vector2.zero, 1f, interactableLayer);

        if (hit.collider != null)
        {
            ResourceOnHarvest resource = hit.collider.GetComponent<ResourceOnHarvest>();
            if (resource != null)
            {
                int baseDamage = PlayerManager.Instance.GetBaseDamage();
                int bonus = PlayerManager.Instance.GetBonusDamageFor(resource);
                int totalDamage = baseDamage + bonus;

                bool isCrit = PlayerManager.Instance.TryCrit();
                if (isCrit)
                {
                    totalDamage = Mathf.RoundToInt(totalDamage * PlayerManager.Instance.GetCritMultiplier());
                    ShowCritEffect(hit.point);
                }

                resource.TakeDamage(totalDamage, true);
            }
        }
    }

    private void ShowCritEffect(Vector3 worldPosition)
{
    if (critPopupPrefab == null || mainCanvas == null) return;

    Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
    GameObject popup = Instantiate(critPopupPrefab, mainCanvas.transform);

    RectTransform rect = popup.GetComponent<RectTransform>();
    RectTransform canvasRect = mainCanvas.GetComponent<RectTransform>();

    if (rect != null && canvasRect != null)
    {
        Vector2 localPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out localPos))
        {
            rect.anchoredPosition = localPos;
        }
    }
}

}