using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform playerPos;

    public Rigidbody2D playerBody;
    public float smoothing;

    public GameObject camera;
    private static Vector3 shift = new Vector3(0, 0, -1);

    public Transform lowerLeftLimit;
    public Transform upperRightLimit;

    public float aheadValue, aheadSpeed;

    public float smoothingDeadband;
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
            if (transform.position != playerPos.position) // transform.position is the position of the game object's transform that this script is attached to --- the camera
            {
                //float deadband = 2.5f;
                Vector3 targetPos = playerPos.position + new Vector3((Mathf.Abs(playerBody.velocity.x) > smoothingDeadband ? Input.GetAxisRaw("Horizontal") : 0 )  * aheadValue, (Mathf.Abs(playerBody.velocity.y) > smoothingDeadband ? Input.GetAxisRaw("Vertical") : 0 )  * aheadValue, 0);
                //Debug.Log(Input.GetAxisRaw("Horizontal") + " this is the horizontal axis val");
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
