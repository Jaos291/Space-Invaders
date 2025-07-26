using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletConfig bulletConfig;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Rigidbody2D rigidbody2D;

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
    }
    private void Start()
    {
        if (!bulletConfig.isPlayerBullet)
        {
            spriteRenderer.flipY = true; // Flip the sprite for enemy bullets
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Bullet collided with: {collision.gameObject.name}");
    }
}
