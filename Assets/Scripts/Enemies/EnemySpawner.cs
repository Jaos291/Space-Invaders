using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private PlayerBulletPooling playerBulletPooling;
    [SerializeField] private EnemyBulletPooling enemyBulletPooling;
    [SerializeField] private Transform initialPosition;
    [SerializeField] private EnemyGroupController enemyGroupController;
    [SerializeField] private int currentLevelIndex = 0;
    [SerializeField] private int rowDivisor = 2;
    [SerializeField] private int rowOffset = 1;

    private int rows;
    private int columns;
    private float levelSpeed;
    private List<Enemy> spawnedEnemies = new List<Enemy>();
    private int defeatedEnemies = 0;
    private GameManager gameManager;

    public static event System.Action<int> OnLevelChanged;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void Start()
    {
        SpawnEnemyFromPool();
    }

    public void NotifyEnemyDefeated()
    {
        defeatedEnemies++;
        if (defeatedEnemies >= gameManager.LevelConfig[currentLevelIndex].maxEnemies)
        {
            currentLevelIndex++;
            OnLevelChanged?.Invoke(currentLevelIndex);
            defeatedEnemies = 0;
            spawnedEnemies.Clear();
            playerBulletPooling?.ReturnAllBulletsToPool();
            enemyBulletPooling?.ReturnAllBulletsToPool();
            enemyGroupController.enabled = true;
            SpawnEnemyFromPool();
        }
    }

    private void SpawnEnemyFromPool()
    {
        var levelConfig = gameManager.LevelConfig[currentLevelIndex];
        rows = levelConfig.maxEnemies / levelConfig.rows;
        columns = levelConfig.maxEnemiesPerRow;
        levelSpeed = levelConfig.levelSpeed;

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

    private Vector3 GetInitialPositionFromManager(int row, int col, int columns, int rows)
    {
        return new Vector3(columns + col, rows - row, 0);
    }
}
