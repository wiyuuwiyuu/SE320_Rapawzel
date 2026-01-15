using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb; 
    private float _xInput;
    public TrailRenderer trail;
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
    public jumpBoost jumpBoost;
    public bool isWallSliding = false;
    public ParticleSystem slidingDust;
    
    public bool gotJumpBoots = false;
    public bool gotClaws = false;
    public bool gotDashBoots = false;
    public bool gotMagicHair = false;
    

    public bool _canDash = true;
    public bool _isDashing;
    private float _dashSpeed = 10f;
    private float _dashTime = 0.3f;
    private float _dashCooldown = 2f;
    private float originalGravity;
    
    private Hook hook;
    private float hookRelaseLockTime;
    
    //heart
    public HeartBar heartBar;
    private bool canTakeDamage = true;
    private Coroutine damageCoroutine;
    public SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isKnockedBack;

    
    
    

    public void Awake()
    {   
        rb = GetComponent<Rigidbody2D>();
        checkGrounded = true;
        nextToWall = false;
        rb.gravityScale *= 1.5f;

        hook = GetComponent<Hook>();
        
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        animator.SetFloat("xSpeed", rb.linearVelocity.magnitude);
        
        if (_isDashing)
        {
            return;
        }
        HandleCollisions();
        SetTraps();
        SetDoubleJump();
        
        _xInput = Input.GetAxis("Horizontal");
        if ((_xInput > 0 && transform.localScale.x < 0) || (_xInput < 0 && transform.localScale.x > 0))
        {
            facingDirection *= -1;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        if(gotClaws){SetIsWallSliding();}
        DustMaker();
        if (checkGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump(); 
            }
        } 
        else if (Input.GetKeyDown(KeyCode.Space) && _canWallJump && gotClaws) //WallJump
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && doubleJump && gotJumpBoots) //DoubleJump
        {
            Jump();
            Instantiate(jumpBoost, transform.position, Quaternion.identity);
            doubleJump = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.7f);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash && gotDashBoots)
        {
            StartCoroutine("Dash");
        }

        if (!_isDashing&& !isKnockedBack)
        {   
            if(rb.linearVelocity.y < -13f) {rb.linearVelocity = new Vector2(rb.linearVelocity.x, -13f);}
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
           Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance, LayerMask.GetMask("Ground")) ||
           Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance, LayerMask.GetMask("Traps")))
        {checkGrounded= true; } else {checkGrounded = false;}
        if(Physics2D.Raycast(transform.position, Vector2.right, _wallCheckDistance, LayerMask.GetMask("Wall")) ||
           Physics2D.Raycast(transform.position, Vector2.left, _wallCheckDistance, LayerMask.GetMask("Wall"))||
           Physics2D.Raycast(transform.position, Vector2.right, _wallCheckDistance, LayerMask.GetMask("Ground"))||
           Physics2D.Raycast(transform.position, Vector2.left, _wallCheckDistance, LayerMask.GetMask("Ground")))
        {nextToWall = true; } else {nextToWall = false;}
        SetWallWay();
        SetCanWallJump();
    }

    private void SetTraps()
    {
        if(Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance, LayerMask.GetMask("Traps")))
        {
            if (!canTakeDamage) return;

            canTakeDamage = false;

            heartBar.TakeDamage(1);
            
            ApplyKnockback();
            
            checkGrounded = true;
            doubleJump = true;
            
            StartCoroutine(DamageFlash());

            if (heartBar.IsDead())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                if (damageCoroutine != null)
                    StopCoroutine(damageCoroutine);

                damageCoroutine = StartCoroutine(DamageCooldown());
            }
        
        }
        
    }
    
    private void ApplyKnockback()
    {
        isKnockedBack = true;
    
        float horizontalDir = transform.localScale.x > 0 ? -1f : 1f;
    
        // Instead of AddForce, we set the velocity directly
        // This forces the player to move at a specific speed instantly
        rb.linearVelocity = new Vector2(horizontalDir * 3f, 3f); 
    
        StartCoroutine(EndKnockback());
    }

    private IEnumerator EndKnockback()
    {
        yield return new WaitForSeconds(0.3f); // How long the player is "stunned"
        isKnockedBack = false;
    }
    
    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(2f); 
        canTakeDamage = true;
    }
    IEnumerator DamageFlash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = originalColor;
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

    private void SetIsWallSliding()
    {
        if (nextToWall && (_xInput / wallWay) > 0)
        {   if (rb.linearVelocity.y <= -2)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -2);
            }
            isWallSliding = true;
            animator.speed = 0;
        }
        else
        {
            isWallSliding = false;
            animator.speed = 1;
        }
    }

    void DustMaker()
    {
        if (isWallSliding)
        {
            slidingDust.Play();
        }
    }
    
    private void SetCanWallJump()
    {
        if (nextToWall && (_xInput / wallWay < 0) ) {_canWallJump = true;}
        else{_canWallJump = false;}
    }

    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;
        originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(facingDirection * _dashSpeed, 0f);
        trail.emitting = true;
        yield return new WaitForSeconds(_dashTime);
        _isDashing = false;
        trail.emitting = false;
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
