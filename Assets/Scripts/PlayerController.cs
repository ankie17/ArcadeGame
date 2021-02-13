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
    private States currentState = States.Start;
    private States previousState;
    private float horizontalInput;
    private float verticalInput;

    public void PausePlayer()
    {
        previousState = currentState;
        currentState = States.Pause;
    }

    public void Respawn()
    {
        currentState = States.Start;
    }

    public void UnpausePlayer()
    {
        currentState = previousState;
        previousState = States.Pause;
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

        moveVector = moveVector * MoveSpeed * Time.fixedDeltaTime;

        transform.position = transform.position + moveVector;
    }
}
