using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class Player : MonoBehaviour {

    //Config
    [SerializeField]
    float runSpeed = 5f;
    [SerializeField]
    float jumpForce = 5f;
    [SerializeField]
    float climbSpeed = 5f;
   

    //State
    bool isAlive = true;

    //Cached Component references 
    Rigidbody2D myRigidBody;
    Animator animator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;


	//Message then methods
	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();

        //set gravity on player to rigidbody to the rigidbody current gravity
        gravityScaleAtStart = myRigidBody.gravityScale;

    }
	
	// Update is called once per frame
	void Update () {

        if(!isAlive) { return; }

        Run();
        Jump();
        ClimbLadder();
        FlipSprite();
        Die();

    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); //-1 to +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
      

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        animator.SetBool("Running", playerHasHorizontalSpeed);
    }

    private void Jump()
    {
        //Check if touching ground.
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            return;


        if(CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpForce);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            animator.SetBool("Climbing", false);
            myRigidBody.gravityScale = gravityScaleAtStart;
            return;
        }


        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        animator.SetBool("Climbing", playerHasVerticalSpeed);

    }

    void FlipSprite()
    {
        //if the player is moving horizontally
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
        {
            //reverse the current scaling of x axis
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }


    private void Die ()
    {
       if( myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            animator.SetTrigger("Dying");
            isAlive = false;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
            
        }
    }

}
