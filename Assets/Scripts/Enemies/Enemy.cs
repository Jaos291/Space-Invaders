using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private Transform enemyShootingPoint;

    private float speed;
    private float health;

    [Header("Enemy Configuration File")]
    public EnemyConfig config;

    private void Awake()
    {
        if(spriteRenderer == null)
        {
            Debug.LogWarning("Sprite Renderer is not assigned for the enemy, getting component which is not optimal.");
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if(boxCollider == null)
        {
            Debug.LogWarning("Box Collider 2D is not assigned for the enemy, getting component which is not optimal.");
            boxCollider = GetComponent<BoxCollider2D>();
        }

        if (rigidbody2D == null)
        {
            Debug.LogWarning("Rigidbody2D is not assigned for the enemy, getting component which is not optimal.");
            rigidbody2D = GetComponent<Rigidbody2D>();
        }
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        ResetValues();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TakeDamage(10);
        Debug.Log("Logged damage!");
    }

    private void Die()
    {
        boxCollider.enabled = false; // Disable collider to prevent further interactions
        ResetValues();
        GameManager.Instance.EnemyPool.AddToPool(gameObject);
    }

    public void ResetValues()
    {
        //reseting values to ensure playability 
        speed = config.speed;
        health = config.health;

        if (spriteRenderer.Equals(null))
        {
            Debug.LogWarning("Sprite Renderer is not assigned for the enemy, getting component which is not optimal.");
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = config.sprite;
        }
        else
        {
            spriteRenderer.sprite = config.sprite;
        }

        if (boxCollider.Equals(null))
        {
            Debug.LogWarning("Box Collider 2D is not assigned for the enemy, getting component which is not optimal.");
            boxCollider = GetComponent<BoxCollider2D>();
        }
        else
        {
            boxCollider.enabled = true; // Ensure the collider is enabled
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }
}
