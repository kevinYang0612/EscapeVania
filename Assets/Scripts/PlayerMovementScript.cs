using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] float runSpeed = 6f;
    [SerializeField] float climbSpeed = 5;
    [SerializeField] float firstJumpSpeed = 20f;
    [SerializeField] float secondJumpSpeed = 12f;
    [SerializeField] Vector2 deathKick = new Vector2(0f, 20f);
    [SerializeField] GameObject bullet; 
    [SerializeField] Transform gun;
    int jumpCount = 0;
    Vector2 moveInput; // push right, left, up, or down. 
    Rigidbody2D rb2d;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider; 
    BoxCollider2D myFeetCollider;
    float gravityAtStart;

    bool isAlive = true;
    private CinemachineImpulseSource myImpulseSource;
    

    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityAtStart = rb2d.gravityScale;
        myImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    
    void Update()
    {
        if (!isAlive) return;
    
        Run();
        FlipSprite();
        ClimbLadder();
        
        Die();
        
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) return;
        Instantiate(bullet, gun.position, transform.rotation);
    }

    // OnMove is player input method, it is a input system attached to player
    void OnMove(InputValue value)
    {
        if (!isAlive) return;
        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (!isAlive) return;
        if (!value.isPressed) return;
        if (CheckGround() || jumpCount < 2)
        {   
            float jumpVelocity = jumpCount == 0 ? firstJumpSpeed : secondJumpSpeed;
            rb2d.velocity = new Vector2(0f, jumpVelocity);
            jumpCount++;
        }
    }
    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rb2d.velocity.y);
        rb2d.velocity = playerVelocity;
        
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        // this bool value is indicating the player running or not. 
        // Set animator's method to true to activate
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);

    }
    void FlipSprite()
    {
        // rb2d.velocity.x is a value when player presses left or right. 
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            bool moveRight = rb2d.velocity.x > 0;
            // built-in rb2d.velocity.x either positive or negative
            transform.localScale = new Vector2(moveRight ? 1f : -1f, 1f);
        }
    }
    bool CheckGround()
    {
        return myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    void ClimbLadder()
    {
        bool isTouchingLadder = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        myAnimator.SetBool("isClimbing", isTouchingLadder);

        if (isTouchingLadder)
        {
            Vector2 climbVelocity = new Vector2(rb2d.velocity.x, moveInput.y * climbSpeed);
            rb2d.velocity = climbVelocity;
            rb2d.gravityScale = 0f;
            myAnimator.speed = climbVelocity.y == 0 ? 0 : 1;
        }
        else
        {
            rb2d.gravityScale = gravityAtStart;
            myAnimator.speed = 1;
        }
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            jumpCount = 0; // Reset jump count on touching the ground
        }    
    }
    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            myImpulseSource.GenerateImpulse(1);
            rb2d.velocity = deathKick;
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
