using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float speed;
    private bool isChasing;

    private Rigidbody2D rb;
    private Transform player;
    private Animator animator;

    private const string horizontal = "horizontal";
    private const string vertical = "vertical";
    private const string lastHorizontal = "lastHorizontal";
    private const string lastVertical = "lastVertical";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChasing)
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
        else
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetFloat(horizontal, 0);
            animator.SetFloat(vertical, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(player == null)
            {
                player = collision.transform;
            }
            isChasing = true;
        }
    }

        private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isChasing = false;
        }
    }
}
