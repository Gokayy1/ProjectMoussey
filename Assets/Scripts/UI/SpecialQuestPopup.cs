using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpecialQuestPopup : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public float moveSpeed = 40f;
    public float fadeDuration = 1.2f;

    private float timer;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Setup(string message)
    {
        popupText.text = message;
        timer = fadeDuration;
        canvasGroup.alpha = 1f;
    }

    void Update()
    {
        float delta = Time.unscaledDeltaTime;

        rectTransform.anchoredPosition += Vector2.up * moveSpeed * delta;
        timer -= delta;

        if (timer < fadeDuration)
            canvasGroup.alpha = timer / fadeDuration;

        if (timer <= 0f)
            Destroy(gameObject);
    }
}
