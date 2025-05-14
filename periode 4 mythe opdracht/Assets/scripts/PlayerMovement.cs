using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    //dash
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 12f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 2f;
    //double jump
    private bool doubleJump;
    private Vector2 lastPosition;

    [SerializeField]private int jumpsLeft = 1;


  //  public Vector2 velocity;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Start()
    {
        rb.gravityScale = 1.0f;

    }
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        //makes sure double jump only activates in the air
        /*   if (IsGrounded() && !Input.GetButton("Jump"))
           {
               doubleJump = false;
           }*/



      
                

        horizontal = Input.GetAxisRaw("Horizontal");


       

        



        Jump();
        if (IsGrounded()) jumpsLeft = 2;

        //allows you to jump higher the longer you press

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rb.gravityScale = 0.2f;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            rb.gravityScale = 1f;
        }

        Flip();

        //end of update to save info on last frame
        lastPosition = transform.position;

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground")) {
            if (transform.position.y < lastPosition.y)
            {
                //i dropped some pixels
                jumpsLeft = 1;

            }
        }
    }

    private void Jump()
    {
  
        if (Input.GetButtonDown("Jump"))
        {

           
            if (jumpsLeft > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
                //you dont lose a jump first time you jump
                jumpsLeft-=2;
                // doubleJump = !doubleJump;

              
            }
            if (rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
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
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }
    private bool IsGrounded()
    {
        
        //checks if player is on the ground
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        
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
        canDash = false;
        isDashing = true;
        float orignalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = orignalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
