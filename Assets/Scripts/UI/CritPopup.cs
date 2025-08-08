using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritPopup : MonoBehaviour
{
    public float lifetime = 1.2f;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        StartCoroutine(FadeAndDestroy());
    }

    private System.Collections.IEnumerator FadeAndDestroy()
    {
        float t = 0f;

        while (t < lifetime)
        {
            t += Time.unscaledDeltaTime;
            if (canvasGroup != null)
                canvasGroup.alpha = 1f - (t / lifetime);
            yield return null;
        }

        Destroy(gameObject);
    }
}
