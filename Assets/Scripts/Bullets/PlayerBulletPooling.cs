using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletPooling : MonoBehaviour
{
    [SerializeField] private GameObject[] playerBulletsPrefab;
    [SerializeField] private int initialBulletPoolSize = 10;

    private Queue<GameObject> playerBullets = new Queue<GameObject>();

    public static PlayerBulletPooling Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        GrowPool();
    }

    private void GrowPool()
    {
        for (int j = 0; j < initialBulletPoolSize; j++)
        {
            var instanceToAdd = Instantiate(playerBulletsPrefab[j]);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        playerBullets.Enqueue(instance);
    }

    public GameObject GetEnemy()
    {
        if (playerBullets.Count.Equals(0))
        {
            GrowPool();
        }

        var instance = playerBullets.Dequeue();
        instance.SetActive(true);
        instance.transform.SetParent(null); // Detach from the pool parent
        return instance;
    }
}
