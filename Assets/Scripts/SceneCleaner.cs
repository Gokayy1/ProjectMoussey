using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneCleaner : MonoBehaviour
{
    void Awake()
    {
        CleanupAudioListeners();
        CleanupEventSystems();
    }

    private void CleanupAudioListeners()
    {
        AudioListener[] listeners = FindObjectsOfType<AudioListener>();
        if (listeners.Length > 1)
        {
        //    Debug.LogWarning($"[SceneCleaner] {listeners.Length} AudioListeners detected. Removing extras...");
            for (int i = listeners.Length - 1; i > 0; i--)
            {
                Destroy(listeners[i]);
            }
        }
    }

    private void CleanupEventSystems()
    {
        EventSystem[] systems = FindObjectsOfType<EventSystem>();
        if (systems.Length > 1)
        {
        //    Debug.LogWarning($"[SceneCleaner] {systems.Length} EventSystems detected. Removing extras...");
            for (int i = 1; i < systems.Length; i++)
            {
                Destroy(systems[i].gameObject);
            }
        }
    }
}
