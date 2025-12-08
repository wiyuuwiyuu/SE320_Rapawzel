using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb; 
    private float _xInput;
    private float _wallCheckDistance = 0.51f;
    private float _groundCheckDistance = 0.55f;
    public bool checkGrounded;
    public bool doubleJump;
    public bool nextToWall;
    private float _moveSpeed = 4.0f;
    public Animator animator;
    public int facingDirection = 1;
    public int wallWay;
    private bool _canWallJump;

    public bool _canDash = true;
    public bool _isDashing;
    private float _dashSpeed = 10f;
    private float _dashTime = 0.3f;
    private float _dashCooldown = 2f;
    private float originalGravity;

    private Hook hook;
    private float hookRelaseLockTime;
    
    
    

    public void Awake()
    {   
        rb = GetComponent<Rigidbody2D>();
        checkGrounded = true;
        nextToWall = false;
        rb.gravityScale *= 1.5f;

        hook = GetComponent<Hook>();
    }

    void Update()
    {
        
        animator.SetFloat("xSpeed", rb.linearVelocity.magnitude);
        if (_isDashing)
        {
            return;
        }
        HandleCollisions();
        SetDoubleJump();
        _xInput = Input.GetAxis("Horizontal");
        if ((_xInput > 0 && transform.localScale.x < 0) || (_xInput < 0 && transform.localScale.x > 0))
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
        else if (Input.GetKeyDown(KeyCode.Space) && _canWallJump)
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && doubleJump)
        {
            Jump();
            doubleJump = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.7f);
            }
        }
        
        //else if (nextToWall) {rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal"), rb.linearVelocity.y*4/5);}
        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash)
        {
            StartCoroutine("Dash");
        }

        if (!_isDashing)
        {
            if (hook != null && hook.IsHooked())
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y);
            }
            else if (Time.time<hookRelaseLockTime)
            {
                //hook yeni bırakılınca ->x momentumu korumak için 
                rb.linearVelocity=new Vector2(rb.linearVelocity.x, rb.linearVelocity.y);    
            }
            else
            {
                rb.linearVelocity = new Vector2(_xInput * _moveSpeed, rb.linearVelocity.y);
            }
           
        }
    }
    
    
    private void SetDoubleJump()
    {   if(checkGrounded) { doubleJump = true; }
    }

    private void HandleCollisions()
    {
        if(Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance, LayerMask.GetMask("Wall")) ||
           Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance, LayerMask.GetMask("Ground")))
        {checkGrounded= true; } else {checkGrounded = false;}
        if(Physics2D.Raycast(transform.position, Vector2.right, _wallCheckDistance, LayerMask.GetMask("Wall")) ||
           Physics2D.Raycast(transform.position, Vector2.left, _wallCheckDistance, LayerMask.GetMask("Wall")))
        {nextToWall = true; } else {nextToWall = false;}
        SetWallWay();
        SetCanWallJump();
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x,8f);
    }
    private void SetWallWay()
    {
        if (nextToWall)
        {
            if (Physics2D.Raycast(transform.position, Vector2.right, _wallCheckDistance, LayerMask.GetMask("Wall")))
            {
                wallWay = 1;
            }
            else
            { wallWay = -1; }
        }
        else
        { wallWay = 0; }
    }
    private void SetCanWallJump()
    {
        if (nextToWall && facingDirection / wallWay < 0) {_canWallJump = true;}
        else{_canWallJump = false;}
    }

    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;
        originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(facingDirection * _dashSpeed, 0f);
        yield return new WaitForSeconds(_dashTime);
        _isDashing = false;
        rb.gravityScale = originalGravity;
        rb.linearVelocity = new Vector2(_xInput, originalGravity);
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }
    public void NotifyHookReleased()
    {
        hookRelaseLockTime = Time.time + 0.45f;
    }
}
