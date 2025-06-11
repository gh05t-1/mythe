using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D parentRB;
    private float horizontal;
    [SerializeField]private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    //glide
    public bool isFalling = false;
    //dash
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 12f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 2f;
    //double jump
    private Vector2 lastPosition;

    
    [SerializeField] private int jumpsLeft = 0;
   // [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Start()
    {


        Debug.Log("!!");
       
        parentRB = transform.parent.GetComponent<Rigidbody2D>();
        parentRB.gravityScale = 1.0f;
        Debug.Log(parentRB);

    }
    void Update()
    {
        if (isDashing)
        {
            return;
        }
 

        horizontal = Input.GetAxisRaw("Horizontal");


        Jump();
        if (IsGrounded()) jumpsLeft = 2;

        //allows you to jump higher the longer you press

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        if (Input.GetKey(KeyCode.Q) && isFalling)
        {
            parentRB.gravityScale = 0.2f;
        }
        if (Input.GetKeyUp(KeyCode.Q))
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

    }
    private void Jump()
    {

        if (Input.GetButtonDown("Jump"))
        { 
            // check if the int has more than 0 if yes than you can jump if no than you cant
            if (jumpsLeft > 0)
            {
                parentRB.linearVelocity = new Vector2(parentRB.linearVelocity.x, jumpingPower);
                // BUG!! you dont lose a jump first time you jump
                //should remove 1 but its 2 rn because of the bug
                jumpsLeft -= 2;
                isFalling = false;
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
        Debug.Log(parentRB.linearVelocity);
    }
    private bool IsGrounded()
    {
       //isFalling = false;
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
