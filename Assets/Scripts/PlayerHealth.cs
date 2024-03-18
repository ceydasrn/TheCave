using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    private Vector3 startingPosition;
    private Quaternion startingRotation;

    private float timeSinceLastDamage = 0f;
    private float timeBetweenDamage = 1.5f; // 0.5 saniye aralıklarla hasar alacak
    private int damagePerInterval = 1; // Her hasar alınma aralığında alınacak hasar miktarı

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        // Başlangıç konumunu ve rotasyonunu kaydet
        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }

    void Update()
    {
        timeSinceLastDamage += Time.deltaTime;

        if (timeSinceLastDamage >= timeBetweenDamage)
        {
            TakeDamage(damagePerInterval);
            timeSinceLastDamage = 0f;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Oyuncu öldüğünde başlangıç konumuna ve rotasyonuna geri dön
        transform.position = startingPosition;
        transform.rotation = startingRotation;

        // Canı yeniden doldur
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
    }
}
