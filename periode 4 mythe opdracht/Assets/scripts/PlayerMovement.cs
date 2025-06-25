using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D parentRB;
    private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    private bool isFacingRight = true;
    //glide
    public bool isFalling = false;
    //dash
    public bool canDash = true;
    private bool isDashing;
    private float dashingPower = 12f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 2f;
    //double jump
    private Vector2 lastPosition;
    //wallslide
    private bool isWallSliding;
    private float wallSlidingSpeed = 200f;
    //wallJump
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.5f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(3f, 5f);
    private float wallJumpCooldown = 0.45f;
    private float wallJumpCooldownTimer = 0f;


    [SerializeField] private int jumpsLeft = 0;
   // [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float jumpGravity = 0.5f;
    [SerializeField] private float fallGravity = 2f;

    private Animator anim;

    // Start is called before the first frame update


    // Update is called once per frame
    void Start()
    {


        //Debug.Log("!!");
       
        parentRB = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        parentRB.gravityScale = fallGravity;
        Debug.Log(parentRB);

    }
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        Debug.Log(parentRB.linearVelocityY);

        horizontal = Input.GetAxisRaw("Horizontal");

        anim.SetFloat("AnimHorizontal", horizontal);
        if (IsGrounded())
        {
            anim.SetBool("AnimGroundedCheck", true);
        }
        else
        {
            anim.SetBool("AnimGroundedCheck", false);
        }
        if (isFalling)
        {
            parentRB.gravityScale = fallGravity;
        }
      
        Jump();
        if (IsGrounded())
        {
            jumpsLeft = 2;
            parentRB.gravityScale = 1f;
        }
        //allows you to jump higher the longer you press

        if (Input.GetButtonDown("Dash") && canDash)
        {
            StartCoroutine(Dash());
            
        }
        if (Input.GetButton("Glide") && isFalling)
        {
            parentRB.gravityScale = 0.2f;
        }
        if (Input.GetButtonUp("Glide"))
        {
            parentRB.gravityScale = 1f;
        }
        if (transform.position.y < lastPosition.y)
        {
            isFalling = true;
        }

            Flip();

        //end of update to save info on last frame
        lastPosition = transform.position;
        //gravityscale = parentRB.gravityScale;

        WallSlide();
        WallJump();

    }
    private void Jump()
    {

        if (Input.GetButtonDown("Jump"))
        {

            // check if the int has more than 0 if yes than you can jump if no than you cant
            if (jumpsLeft > 0)
            {
                parentRB.gravityScale = jumpGravity;
                parentRB.linearVelocity = new Vector2(parentRB.linearVelocity.x, jumpingPower);
                // BUG!! you dont lose a jump first time you jump
                //should remove 1 but its 2 rn because of the bug
                jumpsLeft -= 2;
                isFalling = false;
                anim.SetTrigger("JumpingTrigger");
            }
            //makes it so you can jump higher if you hold space
            if (parentRB.linearVelocity.y > 0f)
            {
                parentRB.linearVelocity = new Vector2(parentRB.linearVelocity.x, parentRB.linearVelocity.y * 0.5f);
            }
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {

        //Check if you walk from a platform there should be one jump left
        if (collision.collider.CompareTag("Ground")) {
            if (transform.position.y < lastPosition.y)
            {
                //i dropped some pixels
                //if you drop your jumps gets set to 1 so you can jump once while falling
                jumpsLeft = 1;

            }
        }
    } 

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return; 
        }

        //more movement

      
        parentRB.linearVelocity = new Vector2(horizontal * speed, parentRB.linearVelocity.y);
        //Debug.Log(parentRB.linearVelocity);
    }
    private bool IsGrounded()
    {
       //isFalling = false;
        //checks if player is on the ground
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        
    }
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            parentRB.linearVelocity = new Vector2(parentRB.linearVelocity.x, Mathf.Clamp(parentRB.linearVelocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (wallJumpCooldownTimer > 0f)
        {
            wallJumpCooldownTimer -= Time.deltaTime;
        }

        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        // Wall jump only if cooldown is over
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f && wallJumpCooldownTimer <= 0f)
        {
            isWallJumping = true;
            parentRB.linearVelocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
            wallJumpCooldownTimer = wallJumpCooldown; // Reset cooldown
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
   

    private void Flip()
    {
        //flips the player if not facing right
        if (isFacingRight && horizontal <0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }
    }
    private IEnumerator Dash()
    {
        anim.SetTrigger("DashTrigger");
        canDash = false;
        isDashing = true;
        float orignalGravity = parentRB.gravityScale;
        parentRB.gravityScale = 0f;
        parentRB.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        parentRB.gravityScale = orignalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
