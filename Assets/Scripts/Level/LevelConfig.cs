using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Configuration", menuName = "Level Configuration")]
// Level configuration data (ScriptableObject)
public class LevelConfig : ScriptableObject
{
    // --- Public fields ---
    public int currentLevel;
    public float levelSpeed;
    public int maxEnemies;
    public int maxEnemiesPerRow;
    public int rows;
}
