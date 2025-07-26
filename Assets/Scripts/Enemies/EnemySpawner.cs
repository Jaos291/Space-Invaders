using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform initialPosition;
    [SerializeField] private EnemyGroupController enemyGroupController;
    [SerializeField] private int currentLevelIndex = 0;
    [SerializeField] private int rowDivisor = 2;
    [SerializeField] private int rowOffset = 1;


    private int column;
    private List<Enemy> spawnedEnemies = new List<Enemy>();
    private int defeatedEnemies = 0;

    private void Start()
    {
        SpawnEnemyFromPool();
    }

    public void NotifyEnemyDefeated()
    {
        defeatedEnemies++;
        Debug.Log($"Enemigos derrotados: {defeatedEnemies}");
        var levelConfigs = GameManager.Instance.LevelConfig;
        var levelConfig = levelConfigs[Mathf.Clamp(currentLevelIndex, 0, levelConfigs.Length - 1)];
        if (defeatedEnemies >= levelConfig.maxEnemies)
        {
            currentLevelIndex++;
            defeatedEnemies = 0;
            spawnedEnemies.Clear();
            enemyGroupController = FindObjectOfType<EnemyGroupController>();
            enemyGroupController.enabled = true;
            SpawnEnemyFromPool();
        }
    }

    private void SpawnEnemyFromPool()
    {
        var levelConfigs = GameManager.Instance.LevelConfig;
        var levelConfig = levelConfigs[Mathf.Clamp(currentLevelIndex, 0, levelConfigs.Length - 1)];
        int rows = levelConfig.maxEnemies / levelConfig.rows;
        int columns = levelConfig.maxEnemiesPerRow;
        float levelSpeed = levelConfig.levelSpeed;

        enemyGroupController = FindObjectOfType<EnemyGroupController>();
        if (enemyGroupController == null) return;
        enemyGroupController.enabled = true;

        spawnedEnemies.Clear();
        for (int j = 0; j < (rows / rowDivisor)-rowOffset; j++)
        {
            for (int i = 0; i < columns; i++)
            {
                var Enemy = GameManager.Instance.EnemyPool.GetEnemy();
                // Aquí deberías usar PositionManager para obtener la posición inicial
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
        // Aquí deberías consultar a tu PositionManager para la posición inicial
        // Por ahora, se mantiene la lógica anterior como fallback
        // return PositionManager.Instance.GetEnemyPosition(row, col);
        return new Vector3(columns + col, rows - row, 0);
    }
}
