using UnityEngine;

public class BatMovement : MonoBehaviour
{

    private Rigidbody2D parentRB;

    private float horizontal;
    private float vertical;
    private float speed = 8f;
    private bool isFacingRight = true;
    //  [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform target;


    // Start is called before the first frame update

    private void Start()
    {
        parentRB = transform.parent.GetComponent<Rigidbody2D>();

        //Debug.Log(parentRB);
    }
    // Update is called once per frame
    void Update()
    {
        //movement
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Flip();



    }
    private void FixedUpdate()
    {
        parentRB.linearVelocity = new Vector2(horizontal * speed, parentRB.linearVelocity.y);
        // �f
        parentRB.linearVelocity = new Vector2(parentRB.linearVelocity.x, vertical * speed);

    }

    private void Flip()
    {
        //flips the player if not facing right
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }


    }
}