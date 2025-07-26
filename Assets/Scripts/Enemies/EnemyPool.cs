using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    // --- Serialized fields ---
    [SerializeField] private GameObject[] enemiesPrefab;
    [SerializeField] private int initialPoolSizePerEnemy = 10;

    // --- Private fields ---
    private Queue<GameObject> availableEnemies = new Queue<GameObject>();

    // --- Public properties ---
    public static EnemyPool Instance { get; private set; }

    // --- Unity event methods ---
    // Initialize singleton and grow pool
    private void Awake()
    {
        Instance = this;
        GrowPool();
    }

    // --- Private methods ---
    // Add more enemies to the pool
    private void GrowPool()
    {
        for (int i = 0; i < enemiesPrefab.Length; i++)
        {
            for (int j = 0; j < initialPoolSizePerEnemy; j++)
            {
                var instanceToAdd = Instantiate(enemiesPrefab[i]);
                instanceToAdd.transform.SetParent(transform);
                AddToPool(instanceToAdd);
            }
        }
    }

    // --- Public methods ---
    // Add an enemy instance back to the pool
    public void AddToPool(GameObject instance)
    {
        instance.GetComponent<Enemy>().ResetValues();
        instance.SetActive(false);
        availableEnemies.Enqueue(instance);
    }

    // Get an enemy from the pool
    public GameObject GetEnemy()
    {
        if (availableEnemies.Count.Equals(0))
        {
            GrowPool();
        }

        var instance = availableEnemies.Dequeue();
        instance.SetActive(true);
        return instance;
    }
}
