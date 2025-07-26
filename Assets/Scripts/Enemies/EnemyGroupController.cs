using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupController : MonoBehaviour
{
    // --- Serialized fields ---
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float moveDistance = 0.2f;
    [SerializeField] private float moveDownDistance = 0.5f;
    [SerializeField] private float moveInterval = 0.5f;
    [SerializeField] private float leftBound = -8f;
    [SerializeField] private float rightBound = 8f;

    // --- Private fields ---
    private List<Enemy> enemies = new List<Enemy>();
    private bool movingRight = true;
    private float moveTimer;
    private GameManager gameManager;

    // --- Unity event methods ---
    // Setup reference to GameManager
    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    // --- Public methods ---
    // Register a new enemy to the group
    public void RegisterEnemy(Enemy enemy)
    {
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }

    // Set the interval between group moves
    public void SetMoveInterval(float interval)
    {
        moveInterval = interval;
    }

    // --- Unity Update ---
    // Handles group movement timing
    private void Update()
    {
        if (!gameManager.canPlay) return;
        if (enemies.Count == 0) return;

        moveTimer += Time.deltaTime;
        if (moveTimer > moveInterval)
        {
            MoveGroup();
            moveTimer = 0f;
        }
    }

    // --- Private methods ---
    // Move the group horizontally or down
    private void MoveGroup()
    {
        float edge = movingRight ? GetRightmostX() : GetLeftmostX();
        bool changeDirection = movingRight ? edge + moveDistance > rightBound : edge - moveDistance < leftBound;

        if (changeDirection)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.gameObject.activeInHierarchy)
                    enemy.transform.position += Vector3.down * moveDownDistance;
            }
            movingRight = !movingRight;
        }
        else
        {
            Vector3 move = (movingRight ? Vector3.right : Vector3.left) * moveDistance;
            foreach (var enemy in enemies)
            {
                if (enemy.gameObject.activeInHierarchy)
                    enemy.transform.position += move;
            }
        }
    }

    // Get the leftmost X position among active enemies
    private float GetLeftmostX()
    {
        float min = float.MaxValue;
        foreach (var enemy in enemies)
        {
            if (enemy.gameObject.activeInHierarchy && enemy.transform.position.x < min)
                min = enemy.transform.position.x;
        }
        return min;
    }

    // Get the rightmost X position among active enemies
    private float GetRightmostX()
    {
        float max = float.MinValue;
        foreach (var enemy in enemies)
        {
            if (enemy.gameObject.activeInHierarchy && enemy.transform.position.x > max)
                max = enemy.transform.position.x;
        }
        return max;
    }
}
