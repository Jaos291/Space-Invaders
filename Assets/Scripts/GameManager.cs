using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Bullet Parents")]
    public Transform playerBulletsParent;
    public Transform enemiesBulletsParent;
    [SerializeField] private LevelConfig[] levelConfig;
    [SerializeField] private EnemyPool enemyPool;
    
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private EnemyGroupController enemyGroupController;
    [SerializeField] private BoxCollider2D gameOverAreaCollilder;

    [Header("Game Configuration")]
    public int scorePerEnemy=100;

    public GameObject player;
    private PlayerController playerController;
    public bool canPlay = false;
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
        playerController = player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canPlay = false;
        if (playerController != null)
        {
            playerController.PublicDie();
        }

    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Game has been started, reseted or level restarted.");
    }
}
