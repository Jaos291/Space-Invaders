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
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private BoxCollider2D boxCollider2D;

    [Header("Bullet")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletCadence;

    [SerializeField]private PlayerInput playerInput;
    private Vector2 moveInput;
    private float moveHorizontal;
    private float posX;
    private Vector3 initialPosition;

    private void Awake()
    {
        if (playerInput.Equals(null))
        {
            Debug.LogWarning("PlayerInput component is not assigned. Attempting to get it from the GameObject.");
            playerInput = GetComponent<PlayerInput>();
        }

        if(rigidbody2D.Equals(null))
        {
            Debug.LogWarning("Rigidbody2D component is not assigned. Attempting to get it from the GameObject.");
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        if (boxCollider2D.Equals(null))
        {
            Debug.LogWarning("BoxCollider2D component is not assigned. Attempting to get it from the GameObject.");
            boxCollider2D = GetComponent<BoxCollider2D>();
        }
    }

    private void Start()
    {
        initialPosition = transform.position;
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

    private void RestartPosition()
    {
        // Reset player position to the center of the screen
        transform.position = initialPosition;
    }
}
