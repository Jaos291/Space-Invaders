using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPooling : MonoBehaviour
{
    [SerializeField] private GameObject[] enemiesBulletPrefab;
    [SerializeField] private int initialBulletPoolSize = 10;

    private Queue<GameObject> enemiesBullets = new Queue<GameObject>();

    public static EnemyBulletPooling Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        GrowPool();
    }

    private void GrowPool()
    {
        for (int j = 0; j < initialBulletPoolSize; j++)
        {
            var instanceToAdd = Instantiate(enemiesBulletPrefab[j]);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        enemiesBullets.Enqueue(instance);
    }

    public GameObject GetEnemy()
    {
        if (enemiesBullets.Count.Equals(0))
        {
            GrowPool();
        }

        var instance = enemiesBullets.Dequeue();
        instance.SetActive(true);
        return instance;
    }
}
