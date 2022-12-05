using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpSpeed;
    public float runSpeed;
    private Rigidbody2D myRigidBody;
    private Animator myAnimator;
    private BoxCollider2D myFeet;
    private bool onGround;
    
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
        //Attack();
        CheckGrounded();
        SwitchJumpFall();
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

            myAnimator.SetBool("Jump", true);
            Vector2 jumpVel = new Vector2(0.0f, jumpSpeed);
            myRigidBody.velocity = Vector2.up * jumpVel;
        }
    }

    //void Attack()
    //{
    //   if (Input.GetButtonDown("Attack"))
    //    {
    //        myAnimator.SetTrigger("Attack");
    //    }
    //}

    void CheckGrounded()
    {
        onGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    void SwitchJumpFall()
    {
        myAnimator.SetBool("Idle", false);
        if(myAnimator.GetBool("Jump"))
        {
            if(myRigidBody.velocity.y < 0.0f)
            {
                myAnimator.SetBool("Jump", false);
                myAnimator.SetBool("Fall", true);
            }
        }
        else if (onGround)
        {
            myAnimator.SetBool("Fall", false);
            myAnimator.SetBool("Idle", true);
        }
    }
}
