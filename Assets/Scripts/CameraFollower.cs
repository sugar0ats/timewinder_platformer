using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform playerPos;
    public float smoothing;

    public GameObject camera;
    private static Vector3 shift = new Vector3(0, 0, -1);
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
                
                transform.position = Vector3.Lerp(transform.position, playerPos.position, smoothing);
                camera.GetComponent<Transform>().position = transform.position + shift;
            }
        }
    }

    // Update is called once per frame
}
