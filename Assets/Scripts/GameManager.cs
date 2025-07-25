using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private EnemyPool enemyPool;

    public static GameManager Instance { get; private set; }
    public LevelConfig LevelConfig => levelConfig;
    public EnemyPool EnemyPool => enemyPool;

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
