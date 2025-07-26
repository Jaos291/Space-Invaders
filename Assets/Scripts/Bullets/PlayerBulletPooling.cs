using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletPooling : MonoBehaviour
{
    // --- Serialized fields ---
    [SerializeField] private GameObject playerBulletsPrefab;
    [SerializeField] private int initialBulletPoolSize = 10;

    // --- Private fields ---
    private Queue<GameObject> playerBullets = new Queue<GameObject>();
    private GameManager gameManager;

    // --- Public properties ---
    public static PlayerBulletPooling Instance { get; private set; }

    // --- Unity event methods ---
    // Initialize singleton and grow pool
    private void Awake()
    {
        Instance = this;
        GrowPool();
    }

    // Setup reference to GameManager
    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    // --- Private methods ---
    // Add more bullets to the pool
    private void GrowPool()
    {
        var instanceToAdd = Instantiate(playerBulletsPrefab);
        instanceToAdd.transform.SetParent(transform);
        AddToPool(instanceToAdd);
        instanceToAdd.GetComponent<Bullet>().playerBulletPooling = this; // Set the player bullet pooling reference
    }

    // --- Public methods ---
    // Add a bullet instance back to the pool
    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        playerBullets.Enqueue(instance);
    }

    // Get a bullet from the pool
    public GameObject GetBullet()
    {
        if (playerBullets.Count.Equals(0))
        {
            GrowPool();
        }

        var instance = playerBullets.Dequeue();
        instance.SetActive(true);
        instance.transform.SetParent(gameManager.playerBulletsParent); // Attach to player bullets parent
        return instance;
    }

    // Return all active bullets to the pool
    public void ReturnAllBulletsToPool()
    {
        // Return all active bullets under the player bullets parent to the pool
        foreach (Transform bullet in gameManager.playerBulletsParent)
        {
            if (bullet.gameObject.activeInHierarchy)
                AddToPool(bullet.gameObject);
        }
    }
}
