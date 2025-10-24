using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;

    private const string horizontal = "horizontal";
    private const string vertical = "vertical";
    private const string lastHorizontal = "lastHorizontal";
    private const string lastVertical = "lastVertical";

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        movement.Set(InputManager.Movement.x, InputManager.Movement.y);
        rb.linearVelocity = movement * moveSpeed;
        animator.SetFloat(horizontal, movement.x);
        animator.SetFloat(vertical, movement.y);

        if(movement != Vector2.zero)
        {
            animator.SetFloat(lastHorizontal, movement.x);
            animator.SetFloat(lastVertical, movement.y);
        }
    }
}
