using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum States
{   
    Idle = 0,
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
    private States currentState = States.Idle;
    private States previousState;
    private float horizontalInput;
    private float verticalInput;
    public GameObject Sprite;
    private Rigidbody2D rb;
    public WallChecker wc;
    private bool walking;
    private int stateID;
    private Animator animator;
    public GameObject PlayerDeathPrefab;
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
            animator.speed = 0;
            previousState = currentState;
            currentState = States.Pause;
        }
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        currentState = States.Idle;
        transform.position = spawnPosition;
        FindObjectOfType<FXManager>().PlayOneShot(1);
    }

    public void UnpausePlayer()
    {
        if (currentState == States.Pause)
        {
            animator.speed = 1;
            currentState = previousState;
            previousState = States.Pause;
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
