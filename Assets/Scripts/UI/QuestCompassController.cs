using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCompassController : MonoBehaviour
{
    public Transform player;
    public Canvas canvas;
    public RectTransform compassImage;

    private Transform questTarget;
    private bool questCheckerFound = false;

    void Update()
    {
        if (!player || !canvas || !compassImage)
            return;

        if (!questCheckerFound)
        {
            GameObject found = GameObject.FindGameObjectWithTag("QuestChecker");
            if (found)
            {
                questTarget = found.transform;
                questCheckerFound = true;
            }
        }

        Vector2 screenPos = Camera.main.WorldToScreenPoint(player.position);
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out localPoint
        );

        compassImage.anchoredPosition = localPoint;

        if (questTarget)
        {
            Vector3 dir = questTarget.position - player.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            compassImage.localRotation = Quaternion.Euler(0, 0, angle - 45f);
        }
    }
}