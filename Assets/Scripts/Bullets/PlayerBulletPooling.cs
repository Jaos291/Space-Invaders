using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletPooling : MonoBehaviour
{
    [SerializeField] private GameObject playerBulletsPrefab;
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
        var instanceToAdd = Instantiate(playerBulletsPrefab);
        instanceToAdd.transform.SetParent(transform);
        AddToPool(instanceToAdd);
        instanceToAdd.GetComponent<Bullet>().playerBulletPooling = this; // Set the player bullet pooling reference
    }

    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        playerBullets.Enqueue(instance);
    }

    public GameObject GetBullet()
    {
        if (playerBullets.Count.Equals(0))
        {
            GrowPool();
        }

        var instance = playerBullets.Dequeue();
        instance.SetActive(true);
        instance.transform.SetParent(GameManager.Instance.playerBulletsParent); // Attach to player bullets parent
        return instance;
    }

    public void ReturnAllBulletsToPool()
    {
        // Return all active bullets under the player bullets parent to the pool
        foreach (Transform bullet in GameManager.Instance.playerBulletsParent)
        {
            if (bullet.gameObject.activeInHierarchy)
                AddToPool(bullet.gameObject);
        }
    }
}
