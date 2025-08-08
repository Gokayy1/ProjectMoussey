using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestChecker : MonoBehaviour
{
    private void OnMouseDown()
    {
        QuestManager.Instance.TryCompleteQuest();
    }
}
