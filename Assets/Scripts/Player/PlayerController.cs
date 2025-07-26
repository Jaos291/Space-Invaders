using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
    public static event System.Action<int> OnLivesChanged;
    public static event System.Action OnPlayerDied;
    public static event System.Action<int> OnScoreChanged;
    [Header("Player configuration")]
    [SerializeField] private int health = 10;
    [SerializeField] private int lives = 3;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public bool canPlay = true;
    private int score = 0;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float[] sideBoundary;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private BoxCollider2D boxCollider2D;

    [Header("Bullet")]
    [SerializeField] private PlayerBulletPooling playerBulletPooling;
    [SerializeField] private Transform bulletSpawnPoint;

    [SerializeField]private PlayerInput playerInput;
    private Vector2 moveInput;
    private float moveHorizontal;
    private float posX;
    private Vector3 initialPosition;
    private GameObject bullet;

    private void Awake()
    {
        if (playerInput.Equals(null))
        {
            Debug.LogWarning("PlayerInput component is not assigned. Attempting to get it from the GameObject.");
            playerInput = GetComponent<PlayerInput>();
        }

        if(rigidbody2D.Equals(null))
        {
            Debug.LogWarning("Rigidbody2D component is not assigned. Attempting to get it from the GameObject.");
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        if (boxCollider2D.Equals(null))
        {
            Debug.LogWarning("BoxCollider2D component is not assigned. Attempting to get it from the GameObject.");
            boxCollider2D = GetComponent<BoxCollider2D>();
        }
    }

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        MovePlayer();
        if (playerInput.actions["Shoot"].triggered)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        bullet = playerBulletPooling.GetBullet();
        bullet.transform.position = bulletSpawnPoint.position;
        Debug.Log("Player is shooting.");
    }

    private void MovePlayer()
    {
        moveInput = playerInput.actions["Movement"].ReadValue<Vector2>();
        moveHorizontal = moveInput.x * moveSpeed * Time.deltaTime;
        posX = transform.position.x + moveHorizontal;

        posX = Mathf.Clamp(posX, sideBoundary[0], sideBoundary[1]);

        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
    }

    private void RestartPosition()
    {
        // Reset player position to the center of the screen
        transform.position = initialPosition;
    }

    public void TakeDamage(int damageAmount)
    {
        if (!canPlay) return;
        health -= damageAmount;
        if (health <= 0)
        {
            lives--;
            OnLivesChanged?.Invoke(lives);
            if (lives > 0)
            {
                StartCoroutine(InvulnerabilityCoroutine());
                health = 10;
            }
            else
            {
                Die();
            }
        }
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        canPlay = false;
        boxCollider2D.enabled = false;
        float blinkTime = 0.2f;
        float timer = 0f;
        for (int i = 0; i < 15; i++)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkTime);
            timer += blinkTime;
        }
        spriteRenderer.enabled = true;
        boxCollider2D.enabled = true;
        canPlay = true;
    }

    private void Die()
    {
        OnPlayerDied?.Invoke();
        Destroy(gameObject);
    }
    public void AddScore(int amount)
    {
        score += amount;
        OnScoreChanged?.Invoke(score);
    }
}
