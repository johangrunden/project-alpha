using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float speed;
    public float attackRange = 0.6f;
    public float attackCooldown = 2;
    private float attackCooldownTimer;
    public float playerDetectRange = 5;

    public Transform dectectionPoint;
    public LayerMask playerLayer;
    private EnemyState enemyState;
    private Rigidbody2D rb;
    private Transform player;
    private Animator animator;

    private const string horizontal = "horizontal";
    private const string vertical = "vertical";
    private const string lastHorizontal = "lastHorizontal";
    private const string lastVertical = "lastVertical";

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyState != EnemyState.Knockback)
        {
            CheckForPlayer();
            if(attackCooldownTimer > 0)
            {
                attackCooldownTimer -= Time.deltaTime;
            }

            if (enemyState == EnemyState.Chasing)
            {
                Chase();
            }
            else if (enemyState == EnemyState.Attacking)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    void Chase()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        animator.SetFloat(horizontal, direction.x);
        animator.SetFloat(vertical, direction.y);

        if(direction != Vector2.zero)
        {
            animator.SetFloat(lastHorizontal, direction.x);
            animator.SetFloat(lastVertical, direction.y);
        }        
    }

    private void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(dectectionPoint.position, playerDetectRange, playerLayer);

        if (hits.Length > 0)
        {
            player = hits[0].transform;

            // if the player is in attack range AND cooldown is ready
            if(Vector2.Distance(transform.position, player.position) <= attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;
                ChangeState(EnemyState.Attacking);
            }
            // Enemy can see player but not close enough for attack
            else if (Vector2.Distance(transform.position, player.position) > attackRange && enemyState != EnemyState.Attacking)
            {
                ChangeState(EnemyState.Chasing);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
        }
    }

    public void ChangeState(EnemyState newState)
    {
        //Exit current animation
        if (enemyState == EnemyState.Idle)
        {
            animator.SetBool("isIdle", false);
        }
        else if (enemyState == EnemyState.Chasing)
        {
            animator.SetBool("isChasing", false);
        }
        else if (enemyState == EnemyState.Attacking)
        {
            animator.SetBool("isAttacking", false);
        }

        // Update current state
        enemyState = newState;

        // Update the new animation
        if (enemyState == EnemyState.Idle)
        {
            animator.SetBool("isIdle", true);
        }
        else if (enemyState == EnemyState.Chasing)
        {
            animator.SetBool("isChasing", true);
        }
        else if (enemyState == EnemyState.Attacking)
        {
            animator.SetBool("isAttacking", true);
        }
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
    Knockback,
}
