using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceOnHarvest : MonoBehaviour
{
    public int maxHP = 10;
    private int currentHP;
    public AudioClip harvestSound;
    private AudioSource audioSource;

    public List<ResourceDrop> drops = new List<ResourceDrop>();

    private void Start()
    {
        currentHP = maxHP;
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int damage, bool isFromPlayer = false)
    {
        if (isFromPlayer)
            PlayHarvestSound();

        currentHP -= damage;
        StartCoroutine(DamageFlash());

        if (currentHP <= 0)
        {
            Harvest(isFromPlayer);
        }
    }

    IEnumerator DamageFlash()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
        }
    }

    private void Harvest(bool isFromPlayer)
    {
        if (isFromPlayer)
        {
            UIManager uiManager = FindObjectOfType<UIManager>();
            int totalDrops = drops.Count;

            for (int i = 0; i < totalDrops; i++)
            {
                var drop = drops[i];

                int xpPerUnit = GetXPForResource(drop.resourceType);
                int finalAmount = ResourceManager.Instance.GetEffectiveAmount(drop.resourceType, drop.amount);
                int xpGained = xpPerUnit * finalAmount;
                PlayerManager.Instance.AddXP(xpGained);

                if (uiManager != null)
                {
                    Vector3 offset;

                    if (totalDrops == 1)
                        offset = new Vector3(0f, 2f, 0f);
                    else if (totalDrops == 2)
                        offset = (i == 0) ? new Vector3(-2f, 2f, 0f) : new Vector3(2f, 2f, 0f);
                    else if (totalDrops == 3)
                        offset = (i == 0) ? new Vector3(-2f, 2f, 0f) :
                                 (i == 1) ? new Vector3(0f, 2.5f, 0f) :
                                            new Vector3(2f, 2f, 0f);
                    else
                    {
                        float angle = Mathf.Lerp(-45f, 45f, i / (float)(totalDrops - 1));
                        float radius = 2f;
                        offset = Quaternion.Euler(0, 0, angle) * Vector3.up * radius;
                    }

                    Vector3 popupWorldPosition = transform.position + offset;
                    ResourceManager.Instance.AddResource(drop.resourceType, drop.amount, popupWorldPosition);
                }
            }
        }

     /*   var identity = GetComponent<ResourceIdentity>();
        if (identity != null && identity.sourcePrefab != null && GameManager.Instance.resourceRegrowManager != null)
        {
            GameManager.Instance.resourceRegrowManager.NotifyDestroyed(identity.sourcePrefab);
        }  */

        Destroy(gameObject);
    }

    private int GetXPForResource(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Fire: return 5;
            case ResourceType.Cold: return 5;
            case ResourceType.Air: return 5;
            case ResourceType.Earth: return 5;
            case ResourceType.Void: return 6;
            case ResourceType.Hope: return 6;
            default: return 3;
        }
    }
    public void PlayHarvestSound()
    {
        if (harvestSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(harvestSound);
        }
    }
    
}
