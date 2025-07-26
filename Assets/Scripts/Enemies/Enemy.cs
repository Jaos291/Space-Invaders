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

    private GameManager gameManager;

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
        gameManager = GameManager.Instance;
        shootTimer = UnityEngine.Random.Range(0f, shootInterval); // Para que no disparen todos sincronizados
    }

    private void Initialize()
    {
        ResetValues();
    }

    private void Update()
    {
        if (gameManager == null || !gameManager.canPlay) return;
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            Shoot();
            shootTimer = 0f;
        }
    }
    private void Shoot()
    {
        if (gameManager == null || !gameManager.canPlay) return;
        if (enemyBulletPool == null) return;
        var bullet = enemyBulletPool.GetBullet();
        if (bullet != null)
        {
            bullet.transform.position = enemyShootingPoint != null ? enemyShootingPoint.position : transform.position;
            bullet.SetActive(true);
            // Play enemy shoot sound
            AudioManager.Instance?.PlaySFX("EnemyShoot");
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
        // Play enemy destroyed sound
        AudioManager.Instance?.PlaySFX("EnemyDestroyed");
        if (GameManager.Instance != null && GameManager.Instance.EnemySpawner != null)
        {
            GameManager.Instance.EnemySpawner.NotifyEnemyDefeated();
            var player = GameManager.Instance.player.GetComponent<PlayerController>();
            if (player != null)
                player.AddScore(gameManager.scorePerEnemy);
        }
        GameManager.Instance.EnemyPool.AddToPool(gameObject);
    }

    // Restored: TakeDamage implementation for IDamageable
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        // Play damage sound (optional, if you want a hit sound)
        // AudioManager.Instance?.PlaySFX("EnemyHit");
        if (health <= 0)
        {
            Die();
        }
    }

    // Restored: ResetValues method
    public void ResetValues()
    {
        // Resetting values to ensure playability 
        speed = config.speed;
        health = config.health;

        if (spriteRenderer == null)
        {
            Debug.LogWarning("Sprite Renderer is not assigned for the enemy, getting component which is not optimal.");
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.sprite = config.sprite;

        if (boxCollider == null)
        {
            Debug.LogWarning("Box Collider 2D is not assigned for the enemy, getting component which is not optimal.");
            boxCollider = GetComponent<BoxCollider2D>();
        }
        boxCollider.enabled = true; // Ensure the collider is enabled
    }
}
