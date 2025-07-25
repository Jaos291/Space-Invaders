using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyConfig config;

    private float speed;
    private float health;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
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
        //Destroy enemy here or use object pooling
    }
}
