using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform initialPosition;

    private int column;

    private void Start()
    {
        SpawnEnemyFromPool();
    }

    private void SpawnEnemyFromPool()
    {
        int rows = GameManager.Instance.LevelConfig.maxEnemies / GameManager.Instance.LevelConfig.rows; //Max lines
        int columns = GameManager.Instance.LevelConfig.maxEnemiesPerRow; //Max columns

        for (int j = 0; j < rows/2; j++)
        {
            for (int i = 0; i < columns; i++)
            {
                var Enemy = GameManager.Instance.EnemyPool.GetEnemy();
                Vector3 startingPosition = new Vector3(columns+i,rows-j,0);
                Enemy.transform.position = startingPosition;
            }
        }
    }
}
