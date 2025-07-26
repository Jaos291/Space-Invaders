using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float moveDistance = 0.2f;
    [SerializeField] private float moveDownDistance = 0.5f;
    [SerializeField] private float moveInterval = 0.5f;
    [SerializeField] private float leftBound = -8f;
    [SerializeField] private float rightBound = 8f;

    private List<Enemy> enemies = new List<Enemy>();
    private bool movingRight = true;
    private float lastMoveTime;

    public void RegisterEnemy(Enemy enemy)
    {
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }

    private void Update()
    {
        if (Time.time - lastMoveTime > moveInterval && enemies.Count > 0)
        {
            MoveGroup();
            lastMoveTime = Time.time;
        }
    }

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
