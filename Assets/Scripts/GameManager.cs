using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Bullet Parents")]
    public Transform playerBulletsParent;
    public Transform enemiesBulletsParent;
    [SerializeField] private LevelConfig[] levelConfig;
    [SerializeField] private EnemyPool enemyPool;
    [SerializeField] public GameObject player;
    [SerializeField] private EnemySpawner enemySpawner;

    public static GameManager Instance { get; private set; }
    public LevelConfig[] LevelConfig => levelConfig;
    public EnemyPool EnemyPool => enemyPool;
    public EnemySpawner EnemySpawner => enemySpawner;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        
    }

    public void ResetGame()
    {
        // Reset game logic here
        // For example, reset scores, player state, etc.
        Debug.Log("Game has been started, reseted or level restarted.");
    }
}
