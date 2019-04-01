using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(SpriteRenderer))]
public class player : MonoBehaviour
{

    

    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float jumpHeight = 3f;

    private Animator am;

    int animState;
    [SerializeField] private Transform groundCheck;
    // [SerializeField] private LayerMask collisionMask;

    private bool grounded;
    private Vector2 inputDirection;
    private Vector3 velocity;

    private bool dead = false;

    private Rigidbody2D playerRB;
    private SpriteRenderer sr;

    [SerializeField] float rayCastLength = 0.2f;

    void Start()
    {
        am = GetComponent <Animator> ();
        playerRB = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        animState = am.GetInteger("animState");
        grounded = IsRayCastHittingGround();
        if(dead==false)
        movePlayer();

        if (!grounded) am.SetInteger("animState",2);
       
        //Debug.DrawRay(groundCheck.position, Vector2.down, Color.red);
    }
    void FixedUpdate()
    {
    
            if (inputDirection.y > 0)
              if (IsRayCastHittingGround())
                playerRB.velocity = new Vector3(playerRB.velocity.x, GetJumpVelocity(), 0f);

        playerRB.velocity = new Vector3(velocity.x, playerRB.velocity.y, 0f);
    }

    void movePlayer()
    {
        inputDirection.x = Input.GetAxisRaw("Horizontal");
        inputDirection.y = Input.GetAxisRaw("Jump");

        velocity = inputDirection.normalized * movementSpeed;
        if (inputDirection.x != 0) am.SetInteger("animState", 1);
        else if (inputDirection.x == 0) am.SetInteger("animState",0);

        if(inputDirection.x>0f)
        {
            sr.flipX = false;
        }
        else { sr.flipX = true; }
        
    }
    float GetJumpVelocity()
    {

        return Mathf.Sqrt(2 * Physics.gravity.magnitude * jumpHeight);
    }

   

    bool IsRayCastHittingGround()
    {
        // Debug.Log("In Raycast");
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, rayCastLength, LayerMask.GetMask("Grounded"));
        if (hit.collider != null)
        {
            return true;
        }
        

        /*if (Physics.Raycast(groundCheck.position, Vector2.down, rayCastLength,LayerMask.GetMask("Grounded")))
        {
            return true;
        }*/
        return false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="enemies")
        {
            velocity = Vector3.zero;
            dead = true;
            //am.SetInteger("animState", 3);
            am.SetBool("isDead", dead);
        }
    }
}
