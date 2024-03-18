using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMove : MonoBehaviour
{
    public float moveSpeed = 2f; // Düşmanın hareket hızı
    public float patrolDistance = 4f; // Patrolling yapılacak maksimum mesafe
    public float attackRange = 1.5f; // Saldırı mesafesi
    public float jumpForce = 8f; // Zıplama kuvveti
    public Transform playerTransform; // Oyuncu referansı
    private Rigidbody2D enemyRB;
    private Vector3 leftPatrolPos; // Sol patrolling sınırı
    private Vector3 rightPatrolPos; // Sağ patrolling sınırı
    private bool movingRight = true; // Düşmanın hangi yöne hareket ettiğini belirleyen bool değişken
    private bool isGrounded = true; // Düşmanın yerde olup olmadığını kontrol eden bool değişken

    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemyRB.constraints = RigidbodyConstraints2D.FreezeRotation; // Yalnızca yatay hareket etmesini sağla
        leftPatrolPos = transform.position - new Vector3(patrolDistance / 2f, 0f, 0f);
        rightPatrolPos = transform.position + new Vector3(patrolDistance / 2f, 0f, 0f);
    }

    void Update()
    {
        Patrolling();
        Attack();
    }

    void Patrolling()
    {
        // Eğer düşman sağa doğru hareket ediyorsa ve sağ sınırı geçtiyse
        if (movingRight && transform.position.x >= rightPatrolPos.x)
        {
            movingRight = false; // Yönü sola çevir
        }
        // Eğer düşman sola doğru hareket ediyorsa ve sol sınırı geçtiyse
        else if (!movingRight && transform.position.x <= leftPatrolPos.x)
        {
            movingRight = true; // Yönü sağa çevir
        }

        // Düşmanın hareket yönüne göre hızını ayarla
        float moveDirection = movingRight ? 1f : -1f;
        enemyRB.velocity = new Vector2(moveSpeed * moveDirection, enemyRB.velocity.y);
    }

    void Attack()
    {
        if (playerTransform == null)
            return;

        // Player belirli bir mesafeye yaklaştığında saldır
        if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            if (isGrounded) // Düşman yerdeyken zıplamasını sağla
            {
                enemyRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Düşman zemine temas ettiğinde yerde olduğunu belirt
        }
    }
}
