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
    Down
}
public class PlayerController : MonoBehaviour
{

    public float MoveSpeed = 5.0f;
    private Vector3 spawnPosition;
    private States currentState = States.Start;
    private States previousState;
    private float horizontalInput;
    private float verticalInput;

    private void Start()
    {
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
        if (horizontalInput < 0)
        {
            currentState = States.Left;
        }
        if(horizontalInput > 0)
        {
            currentState = States.Right;
        }
        if (verticalInput < 0)
        {
            currentState = States.Down;
        }
        if (verticalInput > 0)
        {
            currentState = States.Top;
        }
        //изменить координату объект, передвинув его на несколько условных единиц
        Vector3 moveVector = Vector3.zero;
        if (currentState == States.Top)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            transform.rotation = rotation;
            moveVector = Vector3.up;
        }
        if (currentState == States.Down)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 270));
            transform.rotation = rotation;
            moveVector = Vector3.down;
        }
        if (currentState == States.Left)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            transform.rotation = rotation;
            moveVector = Vector3.left;
        }
        if (currentState == States.Right)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            transform.rotation = rotation;
            moveVector = Vector3.right;
        }

        moveVector = moveVector * MoveSpeed * Time.fixedDeltaTime;

        transform.position = transform.position + moveVector;
    }
}
