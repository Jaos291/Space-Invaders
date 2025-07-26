using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Enemy", menuName ="Enemy Configuration")]
// Enemy configuration data (ScriptableObject)
public class EnemyConfig : ScriptableObject
{
    // --- Public fields ---
    public int identifier;
    public float speed;
    public Sprite sprite;
    public int health;
    public float fireRate;
}
