using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
    // --- Events ---
    public static event System.Action<int> OnLivesChanged;
    public static event System.Action OnPlayerDied;
    public static event System.Action<int> OnScoreChanged;

    // --- Serialized fields ---
    [Header("Player configuration")]
    [SerializeField] private int health = 10;
    [SerializeField] private int lives = 3;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float[] sideBoundary;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private BoxCollider2D boxCollider2D;

    [Header("Bullet")]
    [SerializeField] private PlayerBulletPooling playerBulletPooling;
    [SerializeField] private Transform bulletSpawnPoint;

    [Header("Animations")]
    [SerializeField] private int blinkTimesFromDamage = 15;
    [SerializeField] private float blinkTime = 0.2f;
    [SerializeField] private PlayerInput playerInput;

    // --- Public fields ---
    public bool canPlay = true;
    public int score => Score;

    // --- Private fields ---
    private int Score = 0;
    private float timer = 0f;
    private Vector2 moveInput;
    private float moveHorizontal;
    private float posX;
    private Vector3 initialPosition;
    private GameObject bullet;
    private GameManager gameManager;

    // --- Unity event methods ---
    // Setup references and input
    private void Awake()
    {
        if (playerInput.Equals(null))
        {
            Debug.LogWarning("PlayerInput component is not assigned. Attempting to get it from the GameObject.");
            playerInput = GetComponent<PlayerInput>();
        }
        if (rigidbody2D.Equals(null))
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

    // Initialize player position and game manager
    private void Start()
    {
        initialPosition = transform.position;
        gameManager = GameManager.Instance;
    }

    // Handle movement and shooting input
    private void Update()
    {
        MovePlayer();
        if (playerInput.actions["Shoot"].triggered)
        {
            Shoot();
        }
    }

    // Shoot a bullet if allowed
    private void Shoot()
    {
        if (!gameManager.canPlay) return;
        bullet = playerBulletPooling.GetBullet();
        bullet.transform.position = bulletSpawnPoint.position;
        Debug.Log("Player is shooting.");
    }

    // Move the player horizontally
    private void MovePlayer()
    {
        if (!gameManager.canPlay) return;
        moveInput = playerInput.actions["Movement"].ReadValue<Vector2>();
        moveHorizontal = moveInput.x * moveSpeed * Time.deltaTime;
        posX = transform.position.x + moveHorizontal;
        posX = Mathf.Clamp(posX, sideBoundary[0], sideBoundary[1]);
        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
    }

    // Reset player position to initial
    private void RestartPosition()
    {
        transform.position = initialPosition;
    }

    // Handle taking damage and lives
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

    // Triggered by collisions (e.g. enemy bullets)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TakeDamage(10);
    }

    // Invulnerability after taking damage
    private IEnumerator InvulnerabilityCoroutine()
    {
        canPlay = false;
        boxCollider2D.enabled = false;
        blinkTime = 0.2f;
        timer = 0f;
        for (int i = 0; i < blinkTimesFromDamage; i++)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkTime);
            timer += blinkTime;
        }
        spriteRenderer.enabled = true;
        boxCollider2D.enabled = true;
        canPlay = true;
    }

    // Public method to trigger death externally
    public void PublicDie()
    {
        Die();
    }

    // Handle player death
    private void Die()
    {
        OnPlayerDied?.Invoke();
        Destroy(gameObject);
    }

    // Add score and notify
    public void AddScore(int amount)
    {
        Score += amount;
        OnScoreChanged?.Invoke(Score);
    }

    // Add a life and notify
    public void AddLife()
    {
        lives++;
        OnLivesChanged?.Invoke(lives);
    }
}
