using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player configuration")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float[] sideBoundary;

    [Header("Bullet")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletCadence;

    [SerializeField]private PlayerInput playerInput;
    private Vector2 moveInput;
    float moveHorizontal;
    float posX;

    private void Awake()
    {
        if (playerInput.Equals(null))
        {
            Debug.LogWarning("PlayerInput component is not assigned. Attempting to get it from the GameObject.");
            playerInput = GetComponent<PlayerInput>();
        }
    }

    private void Update()
    {
        MovePlayer();
        if (playerInput.actions["Shoot"].triggered)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Debug.Log("Player is shooting.");
    }

    private void MovePlayer()
    {
        moveInput = playerInput.actions["Movement"].ReadValue<Vector2>();
        moveHorizontal = moveInput.x * moveSpeed * Time.deltaTime;
        posX = transform.position.x + moveHorizontal;

        posX = Mathf.Clamp(posX, sideBoundary[0], sideBoundary[1]);

        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
    }
}
