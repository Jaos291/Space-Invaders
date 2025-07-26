using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Configuration", menuName = "Level Configuration")]
public class LevelConfig : ScriptableObject
{
    public int currentLevel;
    public float levelSpeed;
    public int initialCountdown;
    public int maxEnemies;
    public int maxEnemiesPerRow;
    public int rows;
}
