using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject[] enemiesPrefab;
    [SerializeField] private int initialPoolSizePerEnemy = 10;

    private Queue<GameObject> availableEnemies = new Queue<GameObject>();

    public static EnemyPool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        GrowPool();
    }

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

    private void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        availableEnemies.Enqueue(instance);
    }

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
