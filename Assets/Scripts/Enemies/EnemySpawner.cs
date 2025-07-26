using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // --- Serialized fields ---
    [SerializeField] private LevelCountdownUI levelCountdownUI;
    [SerializeField] private PlayerBulletPooling playerBulletPooling;
    [SerializeField] private EnemyBulletPooling enemyBulletPooling;
    [SerializeField] private Transform initialPosition;
    [SerializeField] private EnemyGroupController enemyGroupController;
    [SerializeField] private int currentLevelIndex = 0;
    [SerializeField] private int rowDivisor = 2;
    [SerializeField] private int rowOffset = 1;

    // --- Private fields ---
    private int rows;
    private int columns;
    private float levelSpeed;
    private List<Enemy> spawnedEnemies = new List<Enemy>();
    private int defeatedEnemies = 0;
    private GameManager gameManager;

    // --- Events ---
    public static event System.Action<int> OnLevelChanged;

    // --- Unity event methods ---
    // Setup reference to GameManager
    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    // Start first level with countdown
    private void Start()
    {
        StartLevelWithCountdown();
    }

    // Called when an enemy is defeated
    public void NotifyEnemyDefeated()
    {
        defeatedEnemies++;
        if (defeatedEnemies >= gameManager.LevelConfig[currentLevelIndex].maxEnemies)
        {
            if (gameManager.player != null)
            {
                var playerController = gameManager.player.GetComponent<PlayerController>();
                if (playerController != null)
                    playerController.AddLife();
            }
            currentLevelIndex++;
            OnLevelChanged?.Invoke(currentLevelIndex + 1);
            defeatedEnemies = 0;
            spawnedEnemies.Clear();
            playerBulletPooling?.ReturnAllBulletsToPool();
            enemyBulletPooling?.ReturnAllBulletsToPool();
            enemyGroupController.enabled = true;
            StartLevelWithCountdown();
        }
    }

    // Start countdown or spawn enemies
    private void StartLevelWithCountdown()
    {
        if (levelCountdownUI != null)
        {
            levelCountdownUI.StartCountdown(SpawnEnemyFromPool);
        }
        else
        {
            SpawnEnemyFromPool();
        }
    }

    // Spawn enemies for the current level
    private void SpawnEnemyFromPool()
    {
        var levelConfig = gameManager.LevelConfig[currentLevelIndex];
        rows = levelConfig.maxEnemies / levelConfig.rows;
        columns = levelConfig.maxEnemiesPerRow;
        levelSpeed = levelConfig.levelSpeed;

        if (enemyGroupController != null)
            enemyGroupController.SetMoveInterval(1f / levelSpeed);

        enemyGroupController.enabled = true;
        spawnedEnemies.Clear();
        for (int j = 0; j < (rows / rowDivisor)-rowOffset; j++)
        {
            for (int i = 0; i < columns; i++)
            {
                var Enemy = GameManager.Instance.EnemyPool.GetEnemy();
                Vector3 startingPosition = GetInitialPositionFromManager(j, i, columns, rows);
                Enemy.transform.position = startingPosition;

                var enemy = Enemy.GetComponent<Enemy>();
                enemy.ApplyLevelMultipliers(levelSpeed, levelSpeed);
                enemyGroupController.RegisterEnemy(enemy);
                spawnedEnemies.Add(enemy);
            }
        }
    }

    // Calculate initial position for each enemy
    private Vector3 GetInitialPositionFromManager(int row, int col, int columns, int rows)
    {
        return new Vector3(columns + col, rows - row, 0);
    }
}
