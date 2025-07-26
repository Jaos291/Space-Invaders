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
    [Header("Bullet Pooling")]
    [SerializeField] private EnemyBulletPooling enemyBulletPool;

    private float speed;
    private float health;
    [SerializeField] private float shootInterval = 2f;
    private float shootIntervalBase = 2f;
    private float shootTimer;

    [Header("Enemy Configuration File")]
    public EnemyConfig config;

    private void Awake()
    {
        shootIntervalBase = shootInterval;
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
    public void ApplyLevelMultipliers(float speedMultiplier, float shootMultiplier)
    {
        shootInterval = shootIntervalBase / shootMultiplier;
        speed = config.speed * speedMultiplier;
    }

    private void OnEnable()
    {
        Initialize();
        shootTimer = UnityEngine.Random.Range(0f, shootInterval); // Para que no disparen todos sincronizados
    }

    private void Initialize()
    {
        ResetValues();
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            Shoot();
            shootTimer = 0f;
        }
    }
    private void Shoot()
    {
        if (enemyBulletPool == null) return;
        var bullet = enemyBulletPool.GetBullet();
        if (bullet != null)
        {
            bullet.transform.position = enemyShootingPoint != null ? enemyShootingPoint.position : transform.position;
            bullet.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TakeDamage(10);
        Debug.Log("Logged damage!");
    }

    private void Die()
    {
        boxCollider.enabled = false;
        ResetValues();
        if (GameManager.Instance != null && GameManager.Instance.EnemySpawner != null)
        {
            GameManager.Instance.EnemySpawner.NotifyEnemyDefeated();
            var player = GameManager.Instance.player.GetComponent<PlayerController>();
            if (player != null)
                player.AddScore(100);
        }
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
