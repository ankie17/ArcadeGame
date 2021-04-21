﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float MoveSpeed = 5.0f;
    private Vector3 spawnPosition;
    private PlayerStates currentState = PlayerStates.Idle;
    private PlayerStates previousState;
    private float horizontalInput;
    private float verticalInput;
    public GameObject Sprite;
    private Rigidbody2D rb;
    public WallChecker wc;
    private bool walking;
    private int stateID;
    private Animator animator;
    public GameObject PlayerDeathPrefab;


    enum PlayerStates
    {
        Idle = 0,
        Pause,
        Right,
        Left,
        Top,
        Down,
        Dead
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;
    }
    public void PausePlayer()
    {
        if (currentState != PlayerStates.Pause)
        {
            animator.speed = 0;
            previousState = currentState;
            currentState = PlayerStates.Pause;
        }
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        currentState = PlayerStates.Idle;
        transform.position = spawnPosition;
        FindObjectOfType<FXManager>().PlayOneShot(1);
    }

    public void UnpausePlayer()
    {
        if (currentState == PlayerStates.Pause)
        {
            animator.speed = 1;
            currentState = previousState;
            previousState = PlayerStates.Pause;
        }
    }
    public void PlayDead(bool canRespawn)
    {
        if (canRespawn)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
        var s = Instantiate(PlayerDeathPrefab, this.transform.position, Quaternion.identity);
        Destroy(s, 1.0f);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //получить инпут стрелки с клавиатуры
        if (currentState != PlayerStates.Pause)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            if (horizontalInput != 0)
            {
                verticalInput = 0;
            }
        }
        //изменить направление движения
        if (horizontalInput < 0&&wc.Left)
        {
            currentState = PlayerStates.Left;
        }
        if(horizontalInput > 0&&wc.Right)
        {
            currentState = PlayerStates.Right;
        }
        if (verticalInput < 0&&wc.Bottom)
        {
            currentState = PlayerStates.Down;
        }
        if (verticalInput > 0&&wc.Top)
        {
            currentState = PlayerStates.Top;
        }
        //изменить координату объект, передвинув его на несколько условных единиц
        Vector3 moveVector = Vector3.zero;
        if (currentState == PlayerStates.Top)
        {
            moveVector = Vector3.up;
        }
        if (currentState == PlayerStates.Down)
        {
            moveVector = Vector3.down;
        }
        if (currentState == PlayerStates.Left)
        {
            moveVector = Vector3.left;
        }
        if (currentState == PlayerStates.Right)
        {
            moveVector = Vector3.right;
        }

        moveVector = moveVector * MoveSpeed;

        rb.velocity = moveVector;
    }
    private void Update()
    {
        stateID = (int)currentState;
        animator.SetInteger("State", stateID);
    }
    public Vector2 GetPlayerPos()
    {
        Vector2 pos = new Vector2();

        int intX = (int)transform.position.x;
        float dX = transform.position.x - intX;
        if (dX > 0.5f)
        {
            pos.x = Mathf.CeilToInt(transform.position.x);
        }
        else
        {
            pos.x = Mathf.FloorToInt(transform.position.x);
        }

        int intY = (int)transform.position.y;
        float dY = transform.position.y - intY;
        if (dY > 0.5f)
        {
            pos.y = Mathf.CeilToInt(transform.position.y);
        }
        else
        {
            pos.y = Mathf.FloorToInt(transform.position.y);
        }

        return pos;
    }
}
