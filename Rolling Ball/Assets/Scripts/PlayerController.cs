using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed; //public for intial value in game Engine
    public float jumpForce;
    private float moveInput; //private isn't acessable for Game Engine

    private Rigidbody2D ribo;

    private bool facingRight = true; // Intializing the to face Right side

    private bool isGrounded;
    public Transform groundCheck;
    public float CheckRadius;
    public LayerMask WhatIsGround;

    private int extraJumps;
    public int extrajumpsValue;

    //climbing variables
    public float distance;
    public LayerMask WhatIsLadder;
    private bool isCliming;
    private float InputVertical;
    public float VerticalSpeed;

    //long press jump
    private float JumpTimeCounter;
    public float jumpTime;
    private bool isJumping;


    void Start()
    {
        extraJumps = extrajumpsValue;
        ribo = GetComponent<Rigidbody2D>();  
    }

    void FixedUpdate()
    {
        //taking input of movement key
        moveInput = Input.GetAxis("Horizontal");
        ribo.velocity = new Vector2(moveInput * speed, ribo.velocity.y);

        //checking for the player movement key for flip
        if (facingRight == true && moveInput < 0)
            Flip();
        else if (facingRight == false && moveInput > 0)
            Flip();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, CheckRadius, WhatIsGround);

        //climbing code
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, WhatIsLadder);

        if(hitInfo.collider != null)
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                isCliming = true;
            }
            else
            {
                if(Input.GetKeyDown(KeyCode.LeftArrow)||Input.GetKeyDown(KeyCode.RightArrow))
                {
                    isCliming = true;
                }
            }
        }
        if (isCliming == true && hitInfo.collider != null)
        {
            InputVertical = Input.GetAxisRaw("Vertical");
            ribo.velocity = new Vector2(ribo.velocity.x, InputVertical * VerticalSpeed);
            ribo.gravityScale = 0;
        }
        else
        {
            ribo.gravityScale = 1;
        }
        

        
    }

    //Here all Jumping 
    private void Update()
    {
        if (isGrounded == true)
            extraJumps = extrajumpsValue;
        if (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps > 0)
        {
            //long press long jump
            isJumping = true;
            JumpTimeCounter = jumpTime;
            //here end
            ribo.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow) && extraJumps == 0 && isGrounded == true)
        {
            //isJumping = true;
           // JumpTimeCounter = jumpTime;
            ribo.velocity = Vector2.up * jumpForce;
        }
        //long press long jump code
        if(Input.GetKey(KeyCode.UpArrow) && isJumping==true)
        {
            if(JumpTimeCounter>0)
            {
                ribo.velocity = Vector2.up * jumpForce;
                JumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            isJumping = false;
        }

    }

    //fliper code for player
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
