using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Enemy", menuName ="Enemy Configuration")]
public class EnemyConfig : MonoBehaviour
{
    public int identifier;
    public float speed;
    public Sprite sprite;
    public int health;
}
