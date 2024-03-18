using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed; // Karakterin ileri hızı
    public float jumpForce; // Zıplama kuvveti
    private Rigidbody2D rb;
    private bool isFacingRight = true; // Karakterin sağa mı sola mı baktığını kontrol etmek için
    bool isGrounded = false;

    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Karakterin yatay hareketi
        float moveInput = Input.GetAxisRaw("Horizontal");
        Vector3 move = new Vector3(moveInput, 0, 0);
        transform.position += move * moveSpeed * Time.deltaTime;
        animator.SetFloat("xVelocity", Mathf.Abs(moveInput));
        animator.SetFloat("yVelocity", rb.velocity.y);

        // Karakterin yüzünü döndürme
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }

        // Zıplama kontrolü
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rb.velocity.y) < 0.001f && isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        animator.SetBool("isJumping", !isGrounded);
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
        animator.SetBool("isJumping", true);
    }
}