using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPooling : MonoBehaviour
{
    [SerializeField] private GameObject enemiesBulletPrefab;
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
        var instanceToAdd = Instantiate(enemiesBulletPrefab);
        instanceToAdd.transform.SetParent(transform);
        AddToPool(instanceToAdd);
        instanceToAdd.GetComponent<Bullet>().enemyBulletPooling = this; // Set the enemy bullet pooling reference
    }

    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        enemiesBullets.Enqueue(instance);
    }

    public GameObject GetBullet()
    {
        if (enemiesBullets.Count.Equals(0))
        {
            GrowPool();
        }

        var instance = enemiesBullets.Dequeue();
        instance.SetActive(true);
        instance.transform.SetParent(null); // Detach from the pool parent
        return instance;
    }
}
