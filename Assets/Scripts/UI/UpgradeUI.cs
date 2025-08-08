using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public Transform slotParent;
    public GameObject upgradeSlotPrefab;

    private void OnEnable()
    {
        ShowUpgradeOptions();
    }

    public void ShowUpgradeOptions()
    {
        ClearChildren(slotParent);

        List<Upgrade> upgrades = UpgradeManager.Instance.GetRandomUpgrades(3);

        foreach (var up in upgrades)
        {
            GameObject go = Instantiate(upgradeSlotPrefab, slotParent);
            UpgradeSlot slot = go.GetComponent<UpgradeSlot>();
            slot.Setup(up, () =>
            {
                UpgradeManager.Instance.OnUpgradeChosen(up);
                gameObject.SetActive(false);
                Time.timeScale = 1f;
            });
        }
    }


    private void ClearChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
