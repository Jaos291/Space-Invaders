using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // --- Serialized fields ---
    [Header("Bullet Parents")]
    public Transform playerBulletsParent;
    public Transform enemiesBulletsParent;
    [SerializeField] private LevelConfig[] levelConfig;
    [SerializeField] private EnemyPool enemyPool;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private EnemyGroupController enemyGroupController;
    [SerializeField] private BoxCollider2D gameOverAreaCollilder;

    [Header("Game Configuration")]
    public int scorePerEnemy = 100;

    // --- Public fields ---
    public GameObject player;
    public bool canPlay = false;
    public static GameManager Instance { get; private set; }
    public LevelConfig[] LevelConfig => levelConfig;
    public EnemyPool EnemyPool => enemyPool;
    public EnemySpawner EnemySpawner => enemySpawner;

    // --- Private fields ---
    private PlayerController playerController;

    // --- Unity event methods ---
    // Called when script instance is being loaded
    private void Awake()
    {
        Instance = this;
    }

    // Called before the first frame update
    private void Start()
    {
        Initialize();
    }

    // Initialize references
    private void Initialize()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    // Game over trigger when enemy collides with area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        canPlay = false;
        if (playerController != null)
        {
            playerController.PublicDie();
        }
    }

    // Reloads the current scene
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Game has been started, reseted or level restarted.");
    }
}
