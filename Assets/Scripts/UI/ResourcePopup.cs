using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResourcePopup : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public float moveSpeed = 50f;
    public float fadeDuration = 1.2f;

    private float timer;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Setup(string resourceName, int amount)
    {
        popupText.text = $"+{amount} {resourceName}";
        popupText.color = GetColorForResource(resourceName);
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

    private Color GetColorForResource(string resourceName)
    {
        switch (resourceName.ToLower())
        {
            case "fire":  return new Color32(229, 142, 8, 255);
            case "cold":  return new Color32(12, 25, 224, 255);
            case "air":   return new Color32(0, 214, 231, 255);
            case "earth": return new Color32(34, 214, 11, 255);
            case "hope":  return new Color32(235, 243, 15, 255);
            case "void":  return new Color32(70, 6, 149, 255);
            default:      return Color.white;
        }
    }
}