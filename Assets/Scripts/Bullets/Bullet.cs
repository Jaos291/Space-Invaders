using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletConfig bulletConfig;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Rigidbody2D rigidbody2D;

    private float direction;

    [HideInInspector] public PlayerBulletPooling playerBulletPooling;
    [HideInInspector] public EnemyBulletPooling enemyBulletPooling;

    private void Awake()
    {
        if (spriteRenderer.Equals(null))
        {
            Debug.LogWarning("SpriteRenderer component is not assigned. Attempting to get it from the GameObject.");
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (boxCollider.Equals(null))
        {
            Debug.LogWarning("BoxCollider2D component is not assigned. Attempting to get it from the GameObject.");
            boxCollider = GetComponent<BoxCollider2D>();
        }

        if (rigidbody2D.Equals(null))
        {
            Debug.LogWarning("Rigidbody2D component is not assigned. Attempting to get it from the GameObject.");
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        spriteRenderer.sprite = bulletConfig.bulletSprite; // Set the bullet sprite from the configuration
    }

    private void OnEnable()
    {
        if (!bulletConfig.isPlayerBullet)
        {
            spriteRenderer.flipY = true; // Flip the sprite for enemy bullets
        }

        InitializeBullet();
    }

    private void OnDisable()
    {
        rigidbody2D.velocity = Vector2.zero; // Reset velocity when bullet is disabled
    }

    private void InitializeBullet()
    {

        direction = bulletConfig.isPlayerBullet ? 1f : -1f; // Set direction based on bullet type
        rigidbody2D.velocity = Vector2.up * bulletConfig.speed * direction; // Set initial velocity
        StartCoroutine(returnBulletAfterTime()); // Start coroutine to return bullet after its lifetime 
    }

    private IEnumerator returnBulletAfterTime()
    {
        yield return new WaitForSeconds(bulletConfig.lifetime);
        ReturnToPool(bulletConfig.isPlayerBullet); // Return bullet to the pool after its lifetime
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!bulletConfig.isPlayerBullet)
        {
            ReturnToPool(false); // Return enemy bullet to the pool
        }
        else
        {
            ReturnToPool(true); // Return enemy bullet to the pool
        }
    }

    private void ReturnToPool(bool isPlayerBullet)
    {
        if (!isPlayerBullet)
        {
            enemyBulletPooling.AddToPool(gameObject); // Return enemy bullet to the pool
        }
        else
        {
            playerBulletPooling.AddToPool(gameObject); // Return player bullet to the pool
        }
    }
}
