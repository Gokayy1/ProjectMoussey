using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRadius = 15f;
    public float attackDistance = 1f;
    public int damagePerHit = 10;
    public float attackCooldown = 2f;

    public float targetRefreshInterval = 2f; 
    private float targetRefreshTimer = 0f;

    private float attackTimer = 0f;
    private GameObject currentTarget;

    void Update()
    {
        attackTimer += Time.deltaTime;
        targetRefreshTimer += Time.deltaTime;

        if ((currentTarget == null || !currentTarget.activeInHierarchy) && targetRefreshTimer >= targetRefreshInterval)
        {
            FindNearestResource();
            targetRefreshTimer = 0f;
        }

        if (currentTarget != null)
        {
            if (IsTargetDestroyed(currentTarget))
            {
                currentTarget = null;
                return; 
            }

            MoveToTarget();

            float dist = Vector3.Distance(transform.position, currentTarget.transform.position);
            if (dist <= attackDistance && attackTimer >= attackCooldown)
            {
                AttackTarget();
                attackTimer = 0f;
            }
        }
    }

    void FindNearestResource()
    {
        GameObject[] allResources = GameObject.FindGameObjectsWithTag("Resource");
        float closestDist = detectionRadius;
        GameObject closest = null;

        foreach (var obj in allResources)
        {
            if (obj == null) continue;

            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < closestDist)
            {
                closest = obj;
                closestDist = dist;
            }
        }

        currentTarget = closest;
    }

    void MoveToTarget()
    {
        if (currentTarget == null) return;

        Vector3 dir = (currentTarget.transform.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    void AttackTarget()
    {
        if (currentTarget == null) return;

        var harvest = currentTarget.GetComponent<ResourceOnHarvest>();
        if (harvest != null)
        {
            harvest.TakeDamage(damagePerHit, false); 
        }
        else
        {
           /* var identity = currentTarget.GetComponent<ResourceIdentity>();
            if (identity != null && identity.sourcePrefab != null)
            {
                GameManager.Instance.resourceRegrowManager.NotifyDestroyed(identity.sourcePrefab);
            } */

            Destroy(currentTarget);
        }
    }
    private bool IsTargetDestroyed(GameObject target)
    {
        return target == null || target.Equals(null);
    }
}
