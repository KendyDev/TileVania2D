using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    float moveSpeed = 1f;

    Rigidbody2D myRigidBody;


    bool isFacingRight;

	// Use this for initialization
	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        if (IsFacingRight())
        {
            myRigidBody.velocity = new Vector2(moveSpeed, 0f);

        }
        else
        {
            myRigidBody.velocity = new Vector2(-moveSpeed, 0f);
        }
       
	}

    void FlipSprite()
    {
        //if the player is moving horizontally
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            //reverse the current scaling of x axis
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);
    }

    bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }
}
