using System.Collections;
using UnityEngine;

public class Enemy_Patrol : MonoBehaviour
{
    public Vector2[] patrolPoints;
    private Vector2 target;
    public float speed = 2;
    public float pauseDuration = 1.5f;
    private bool isPaused;
    private int currentPatrolIndex;

    private Rigidbody2D rb;

     void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(SetPatrolPoint());
    }

    void Update()
    {
        if (isPaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = ((Vector3) target - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        if(Vector2.Distance(transform.position, target) < .1f)
        {
            StartCoroutine(SetPatrolPoint());
        }
    }

    IEnumerator SetPatrolPoint() 
    {
        isPaused = true;

        yield return new WaitForSeconds(pauseDuration);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        target = patrolPoints[currentPatrolIndex];
        isPaused = false;
    }
}
