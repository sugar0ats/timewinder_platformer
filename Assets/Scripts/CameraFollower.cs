using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform playerPos;
    public float smoothing;

    public GameObject camera;
    private static Vector3 shift = new Vector3(0, 0, -1);

    public Transform lowerLeftLimit;
    public Transform upperRightLimit;
    // Start is called before the first frame update
    void Start()
    {
        //Vector3 shift = new Vector3(0, 0, -1);
        //playerPos = player.GetComponent<Transform>().position + shift;
        //cameraPos = transform.position + shift;
    }

    void LateUpdate()
    {
        if (playerPos != null)
        {
            if (transform.position != playerPos.position)
            {
                Vector3 targetPos = playerPos.position;
                targetPos.x = Mathf.Clamp(targetPos.x, lowerLeftLimit.position.x, upperRightLimit.position.x);
                targetPos.y = Mathf.Clamp(targetPos.y, lowerLeftLimit.position.y, upperRightLimit.position.y);
                transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
                camera.GetComponent<Transform>().position = transform.position + shift;
            }
        }
    }

    public void setCameraLimit(Transform minPos, Transform maxPos)
    {
        lowerLeftLimit = minPos;
        upperRightLimit = maxPos;
    }

    // Update is called once per frame
}
