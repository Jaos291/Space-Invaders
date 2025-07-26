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
    [HideInInspector]public int maxEnemies = 40;
    [HideInInspector] public int maxEnemiesPerRow=10;
    [HideInInspector] public int rows=4;
}
