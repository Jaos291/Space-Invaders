using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet Configuration", menuName = "Bullet Configuration")]
public class BulletConfig : ScriptableObject
{
    public bool isPlayerBullet;
    public float speed;
    public float damage;
    public float lifetime;
    public float fireRate;
    public Sprite bulletSprite;
}
