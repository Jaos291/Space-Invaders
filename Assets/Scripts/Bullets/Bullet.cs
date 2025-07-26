using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletConfig bulletConfig;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (spriteRenderer.Equals(null))
        {
            Debug.LogWarning("SpriteRenderer component is not assigned. Attempting to get it from the GameObject.");
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
    private void Start()
    {
        if (!bulletConfig.isPlayerBullet)
        {
            spriteRenderer.flipY = true; // Flip the sprite for enemy bullets
        }
    }
}
