using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 0, -10);
    public Vector2 minCameraPos;
    public Vector2 maxCameraPos; 

    private bool isLocked = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isLocked = !isLocked;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (!isLocked)
        {
            Vector3 desiredPosition = target.position + offset;

            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minCameraPos.x, maxCameraPos.x);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minCameraPos.y, maxCameraPos.y);

            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
