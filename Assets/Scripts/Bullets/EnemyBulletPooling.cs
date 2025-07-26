using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPooling : MonoBehaviour
{
    // --- Serialized fields ---
    [SerializeField] private GameObject enemiesBulletPrefab;
    [SerializeField] private int initialBulletPoolSize = 10;

    // --- Private fields ---
    private Queue<GameObject> enemiesBullets = new Queue<GameObject>();

    // --- Public properties ---
    public static EnemyBulletPooling Instance { get; private set; }

    // --- Unity event methods ---
    // Initialize singleton and grow pool
    private void Awake()
    {
        Instance = this;
        GrowPool();
    }

    // --- Private methods ---
    // Add more bullets to the pool
    private void GrowPool()
    {
        var instanceToAdd = Instantiate(enemiesBulletPrefab);
        instanceToAdd.transform.SetParent(transform);
        AddToPool(instanceToAdd);
        instanceToAdd.GetComponent<Bullet>().enemyBulletPooling = this; // Set the enemy bullet pooling reference
    }

    // --- Public methods ---
    // Add a bullet instance back to the pool
    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        enemiesBullets.Enqueue(instance);
    }

    // Get a bullet from the pool
    public GameObject GetBullet()
    {
        if (enemiesBullets.Count.Equals(0))
        {
            GrowPool();
        }

        var instance = enemiesBullets.Dequeue();
        instance.SetActive(true);
        instance.transform.SetParent(GameManager.Instance.enemiesBulletsParent); // Attach to enemy bullets parent
        return instance;
    }

    // Return all active bullets to the pool
    public void ReturnAllBulletsToPool()
    {
        // Return all active bullets under the enemy bullets parent to the pool
        foreach (Transform bullet in GameManager.Instance.enemiesBulletsParent)
        {
            if (bullet.gameObject.activeInHierarchy)
                AddToPool(bullet.gameObject);
        }
    }
}
