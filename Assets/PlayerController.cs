using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float jumpSpeed;
    public float runSpeed;
    private Rigidbody2D myRigidBody;
    private Animator myAnimator;
    private BoxCollider2D myFeet;
    private bool onGround;

    // changing variables demo code:
    public InputField jumpSpeedI;
    public InputField massI;
    public InputField gravityI;
    public InputField linearDragI;



    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>(); // get the player's feet. duh.
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        Run();
        Jump();
        CheckGrounded();

        // demo code
        updateJumpSpeed();
        updateGravity();
        updateLinearDrag();
        updateMass();
    }

    void Run()
    {
        float moveDir = Input.GetAxis("Horizontal");
        Vector2 playerVel = new Vector2(moveDir * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVel;
        bool hasXVel = Mathf.Abs(myRigidBody.velocity.x) > 5;
        myAnimator.SetBool("Run", hasXVel);
    }

    void Flip()
    {
        bool hasXVel = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (hasXVel)
        {
            if (myRigidBody.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            } 

            if (myRigidBody.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    void Jump()
    {
        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 jumpVel = new Vector2(0.0f, jumpSpeed);
            myRigidBody.velocity = Vector2.up * jumpVel;
        }
    }

    void CheckGrounded()
    {
        onGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }


    // demo code:
    public void updateJumpSpeed()
    {
        jumpSpeed = (jumpSpeedI.text == "" ? 1 : float.Parse(jumpSpeedI.text, CultureInfo.InvariantCulture.NumberFormat));
    }

    public void updateMass()
    {
        myRigidBody.mass = (massI.text == "" ? 1 : float.Parse(massI.text, CultureInfo.InvariantCulture.NumberFormat));
    }

    public void updateGravity()
    {
        myRigidBody.gravityScale = (gravityI.text == "" ? 1 : float.Parse(gravityI.text, CultureInfo.InvariantCulture.NumberFormat));
    }

    public void updateLinearDrag()
    {
        myRigidBody.drag = (linearDragI.text == "" ? 0 : float.Parse(linearDragI.text, CultureInfo.InvariantCulture.NumberFormat));
    }
}
