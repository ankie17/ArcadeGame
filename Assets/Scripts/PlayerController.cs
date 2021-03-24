using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum States
{   
    Start = 0,
    Pause,
    Right,
    Left,
    Top,
    Down,
    Dead
}
public class PlayerController : MonoBehaviour
{

    public float MoveSpeed = 5.0f;
    private Vector3 spawnPosition;
    private States currentState = States.Start;
    private States previousState;
    private float horizontalInput;
    private float verticalInput;
    public GameObject Sprite;
    private Rigidbody2D rb;
    public WallChecker wc;
    private bool walking;
    private int stateID;
    private Animator animator;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;
    }
    public void PausePlayer()
    {
        if (currentState != States.Pause)
        {
            previousState = currentState;
            currentState = States.Pause;
        }
    }

    public void Respawn()
    {
        currentState = States.Start;
        transform.position = spawnPosition;
    }

    public void UnpausePlayer()
    {
        if (currentState == States.Pause)
        {
            currentState = previousState;
            previousState = States.Pause;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //получить инпут стрелки с клавиатуры
        if (currentState != States.Pause)
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
            currentState = States.Left;
        }
        if(horizontalInput > 0&&wc.Right)
        {
            currentState = States.Right;
        }
        if (verticalInput < 0&&wc.Bottom)
        {
            currentState = States.Down;
        }
        if (verticalInput > 0&&wc.Top)
        {
            currentState = States.Top;
        }
        //изменить координату объект, передвинув его на несколько условных единиц
        Vector3 moveVector = Vector3.zero;
        if (currentState == States.Top)
        {
            moveVector = Vector3.up;
        }
        if (currentState == States.Down)
        {
            moveVector = Vector3.down;
        }
        if (currentState == States.Left)
        {
            moveVector = Vector3.left;
        }
        if (currentState == States.Right)
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
}
