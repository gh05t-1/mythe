using System.Collections;
using UnityEngine;

public class simpleAnimalMovement : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private Transform vampireForm;
    [SerializeField] private Transform batForm;
    private Transform playerTransform;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float verticalDetectionTolerance = 1f;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Patrol Settings")]
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    private Transform currentPatrolPoint;

    [Header("Movement Settings")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float fleeSpeed = 6f;
    private Rigidbody2D rb;
    private Animator animator;
    private Transform animalTransform;

    private bool isFleeing = false;
    private bool hasStartedFleeing = false;

    [Header("Flee Settings")]
    [SerializeField] private float fleeDuration = 3f;

    private void Start()
    {
        animalTransform = transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentPatrolPoint = pointB.transform;
        animator.SetBool("isRunning", true);
    }

    private void Update()
    {
        UpdatePlayerReference();

        if (playerTransform == null)
        {
            Patrol();
            return;
        }

        float distanceToPlayer = Vector2.Distance(animalTransform.position, playerTransform.position);
        float verticalDifference = Mathf.Abs(animalTransform.position.y - playerTransform.position.y);
        bool canSeePlayer = !Physics2D.Linecast(animalTransform.position, playerTransform.position, obstacleMask);

        isFleeing = (distanceToPlayer <= detectionRange &&
                     verticalDifference <= verticalDetectionTolerance &&
                     canSeePlayer);

        if (isFleeing)
        {
            FleeFromPlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void UpdatePlayerReference()
    {
        if (vampireForm != null && vampireForm.gameObject.activeInHierarchy)
            playerTransform = vampireForm;
        else if (batForm != null && batForm.gameObject.activeInHierarchy)
            playerTransform = batForm;
        else
            playerTransform = null;
    }

    private void Patrol()
    {
        if (hasStartedFleeing) return;

        Vector2 direction = (currentPatrolPoint.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * patrolSpeed, rb.linearVelocity.y);

        animator.SetBool("isRunning", true);

        if ((direction.x > 0 && transform.localScale.x < 0) || (direction.x < 0 && transform.localScale.x > 0))
            Flip();

        if (Vector2.Distance(transform.position, currentPatrolPoint.position) < 0.5f)
            currentPatrolPoint = (currentPatrolPoint == pointA.transform) ? pointB.transform : pointA.transform;
    }

    private void FleeFromPlayer()
    {
        if (!hasStartedFleeing)
        {
            hasStartedFleeing = true;
            StartCoroutine(DisappearAfterDelay());
        }

        Vector2 direction = (transform.position - playerTransform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * fleeSpeed, rb.linearVelocity.y);

        animator.SetBool("isRunning", true);

        if ((direction.x > 0 && transform.localScale.x < 0) || (direction.x < 0 && transform.localScale.x > 0))
            Flip();
    }

    private IEnumerator DisappearAfterDelay()
    {
        yield return new WaitForSeconds(fleeDuration);
        gameObject.SetActive(false); // Or use Destroy(gameObject);
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}

