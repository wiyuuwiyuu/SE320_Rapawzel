using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb; 
    private float xInput;
    private float wallCheckDistance = 0.55f;
    private float groundCheckDistance = 0.55f;
    public bool checkGrounded;
    public bool doubleJump;
    public bool nextToWall;
    public float lastWay;
    private float moveSpeed = 3.0f;
    public Animator animator;
    public int facingDirection = 1;
    
    

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        checkGrounded = true;
        nextToWall = false;
    }

    void Update()
    {
        animator.SetFloat("xSpeed", rb.linearVelocity.magnitude);
        HandleCollisions();
        SetDoubleJump();
        SetLastWay();
        xInput = Input.GetAxis("Horizontal");
        if ((xInput > 0 && transform.localScale.x < 0) || (xInput < 0 && transform.localScale.x > 0))
        {
            facingDirection *= -1;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        
        if (checkGrounded)
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                Jump();
            }
        } 
        else if (Input.GetKeyDown(KeyCode.Space) && nextToWall && xInput / lastWay < 0)
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && doubleJump)
        {
            Jump();
            doubleJump = false;
        }
        
        //else if (nextToWall) {rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal"), rb.linearVelocity.y*4/5);}
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rb.linearVelocity = new Vector2(lastWay * moveSpeed* 15.0f, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
        }

    }
    
    private void SetDoubleJump()
    {   if(checkGrounded) { doubleJump = true; }
    }

    private void HandleCollisions()
    {
        checkGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, LayerMask.GetMask("Ground"));
        if(Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, LayerMask.GetMask("Wall")) ||
           Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, LayerMask.GetMask("Wall")))
        {nextToWall = true; } else {nextToWall = false;}
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(xInput * moveSpeed, 5.0f);
        
    }

    private void SetLastWay()
    {
        if (Input.GetAxis("Horizontal")>= 0.1f)  {lastWay = 1.0f;}

        if (Input.GetAxis("Horizontal")<=-0.1f) {lastWay = -1.0f;}
    }
    

    
    
}
