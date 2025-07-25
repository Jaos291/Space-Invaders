using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    private Queue<GameObject> enemyPool = new Queue<GameObject>();
}
