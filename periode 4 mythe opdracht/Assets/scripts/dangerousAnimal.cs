using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dangerousAnimal : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private GameObject playerRoot;
    [SerializeField] private Transform vampirePlayer;
    [SerializeField] private Transform batPlayer;
    private Transform activeForm;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float verticalDetectionTolerance = 1f;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Patrol Settings")]
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    private Transform currentPoint;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform enemyTransform;
    private bool isFollowing = false;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;
    private float lastAttackTime;

    private void Start()
    {
        enemyTransform = transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        anim.SetBool("isRunning", true);
    }

    private void Update()
    {
        UpdateActiveForm();
        if (activeForm == null)
        {
            Patrol();
            return;
        }

        float distanceToPlayer = Vector2.Distance(enemyTransform.position, activeForm.position);
        float verticalDifference = Mathf.Abs(enemyTransform.position.y - activeForm.position.y);
        bool canSeePlayer = !Physics2D.Linecast(enemyTransform.position, activeForm.position, obstacleMask);

        isFollowing = (distanceToPlayer <= detectionRange &&
                       verticalDifference <= verticalDetectionTolerance &&
                       canSeePlayer);

        if (isFollowing)
        {
            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
            }
            else
            {
                MoveTowardsPlayer();
            }
        }
        else
        {
            Patrol();
        }
    }

    private void UpdateActiveForm()
    {
        if (vampirePlayer != null && vampirePlayer.gameObject.activeInHierarchy)
            activeForm = vampirePlayer;
        else if (batPlayer != null && batPlayer.gameObject.activeInHierarchy)
            activeForm = batPlayer;
        else
            activeForm = null;
    }

    private void Patrol()
    {
        Vector2 direction = (currentPoint.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        anim.SetBool("isRunning", true);

        if ((direction.x > 0 && transform.localScale.x < 0) || (direction.x < 0 && transform.localScale.x > 0))
            Flip();

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
            currentPoint = (currentPoint == pointA.transform) ? pointB.transform : pointA.transform;
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (activeForm.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed * 2, rb.linearVelocity.y);

        anim.SetBool("isRunning", true);

        if ((direction.x > 0 && transform.localScale.x < 0) || (direction.x < 0 && transform.localScale.x > 0))
            Flip();
    }

    private void AttackPlayer()
    {
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("isRunning", false);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            anim.SetTrigger("Attack");
            Debug.Log("Enemy is attacking the player ");
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}