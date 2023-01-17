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

    public PhysicsMaterial2D frictionlessBoy;

    public float hangTime;
    private float hangCounter;

    public float jumpBufferLength;
    private float jumpBufferCount;

    public ParticleSystem footDust;
    public ParticleSystem.EmissionModule footDustFrequency;

    public float shortJumpMultiplier;

    public float runDeadband;

    public ParticleSystem impactDust;
    private bool wasOnGround;
    
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>(); // get the player's feet. duh.
        frictionlessBoy = new PhysicsMaterial2D();
        footDustFrequency = footDust.emission;

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

        CreateImpactDust(); // i feel bad calling this every frame...

        wasOnGround = onGround; // records previous frame's 
    }

    void Run()
    {
        float moveDir = (Mathf.Abs(Input.GetAxis("Horizontal")) > runDeadband ? Input.GetAxis("Horizontal") : 0);
        Vector2 playerVel = new Vector2(moveDir * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVel;
        bool hasXVel = Mathf.Abs(myRigidBody.velocity.x) > 5;
        myAnimator.SetBool("Run", hasXVel);

        CreateDust();
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

        

        if (onGround) {
            hangCounter = hangTime;
        } else {
            hangCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
             jumpBufferCount = jumpBufferLength;
        } else {
            jumpBufferCount -= Time.deltaTime;
        }

        if (hangCounter > 0 && jumpBufferCount >= 0)
        {
            jumpBufferCount = 0;
            myAnimator.SetBool("Jump", true);
            Vector2 jumpVel = new Vector2(0.0f, jumpSpeed);
            myRigidBody.velocity = Vector2.up * jumpVel;
            //myRigidBody.sharedMaterial = frictionlessBoy;
            //myRigidBody.sharedMaterial.friction = 0;
            //Debug.Log("material friction is " + myRigidBody.sharedMaterial.friction);
        } 

        if (Input.GetKeyUp(KeyCode.Space) && myRigidBody.velocity.y > 0.3) {
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, myRigidBody.velocity.y * shortJumpMultiplier);
        }
        
       
    }

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

    void CreateDust() {
        if (Mathf.Abs(myRigidBody.velocity.x) > runDeadband && !myAnimator.GetBool("Jump") && !myAnimator.GetBool("Fall")) {
            footDustFrequency.rateOverTime = 20.0f;
        } else {
            footDustFrequency.rateOverTime = 0.0f;
        }
    }

    void CreateImpactDust() {
        if (!wasOnGround && onGround) {
            impactDust.gameObject.SetActive(true);
            impactDust.Stop();
            impactDust.gameObject.transform.position = footDust.gameObject.transform.position;
            impactDust.Play();
        }   
    }
}
