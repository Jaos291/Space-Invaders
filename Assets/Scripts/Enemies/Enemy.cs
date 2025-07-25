using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider;

    private float speed;
    private float health;

    [Header("Enemy Configuration File")]
    public EnemyConfig config;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        ResetValues();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TakeDamage(10f);
        Debug.Log("Logged damage!");
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
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
}
