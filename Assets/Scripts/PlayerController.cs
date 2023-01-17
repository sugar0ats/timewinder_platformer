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

    public float acceleration;
    public float deceleration;
    public float velPower; 

    public float frictionAmount;

    public float gravityScale;

    public float fallGravityMultiplier;
    
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myRigidBody.gravityScale = gravityScale;
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
        
        float targetSpeed = moveDir * runSpeed;
        float speedDiff = targetSpeed - myRigidBody.velocity.x; // if negative, we want to get slower. if positive, we want to get faster.
        float accelRate = (Mathf.Abs(targetSpeed) > runDeadband ? acceleration : deceleration); // if target speed is positive or negative (left or right)
        float movementForce = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff); // faster velocities mean more acceleration
        
        myRigidBody.AddForce(movementForce * Vector2.right);

        bool hasXVel = Mathf.Abs(myRigidBody.velocity.x) > runDeadband;
        myAnimator.SetBool("Run", hasXVel);

        ApplyFriction();

        CreateDust();
    }

    void ApplyFriction() {
        if (onGround && Input.GetAxis("Horizontal") == 0) {

        }
        float frictionForce = Mathf.Min(Mathf.Abs(frictionAmount), Mathf.Abs(myRigidBody.velocity.x)); // obtain correct force, whether it be the predetermined value for friction or the velocity of the player (why the player?)
        frictionForce *= Mathf.Sign(myRigidBody.velocity.x); // get correct sign
        myRigidBody.AddForce(-frictionForce * Vector2.right, ForceMode2D.Impulse); // force is added over a period of time, impulse is instant- like if we want to simulate friction
    }   // friction force goes in the opposite direction of the player to make them stop and feel less slippery

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
        checkHangTimeJumpBuffer();
        
        if (hangCounter > 0 && jumpBufferCount >= 0)
        {
            jumpBufferCount = 0;
            myAnimator.SetBool("Jump", true);
            Vector2 jumpVel = new Vector2(0.0f, jumpSpeed);
            myRigidBody.velocity = Vector2.up * jumpVel;
        } 

       ApplyFallGravity();

        if (Input.GetKeyUp(KeyCode.Space) && myRigidBody.velocity.y > 0.3f) {
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, myRigidBody.velocity.y * shortJumpMultiplier);
        }
        
       
    }

    void ApplyFallGravity() {
        myRigidBody.gravityScale = (myRigidBody.velocity.y < 0.0f ? gravityScale * fallGravityMultiplier : gravityScale);
    }

    void checkHangTimeJumpBuffer() {
        // hangtime and jump buffer checks
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

    }

    void CheckGrounded()
    {
        onGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    void SwitchJumpFall()
    {
        myAnimator.SetBool("Idle", false);
        if(myAnimator.GetBool("Jump")) // if jump is true
        {
            if(myRigidBody.velocity.y < 0.0f) // the character is falling rn
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
