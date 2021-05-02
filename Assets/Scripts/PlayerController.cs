using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float MoveSpeed = 5.0f;
    private bool paused;
    private Vector3 spawnPosition;
    [SerializeField]
    private PlayerMode mode;
    private PlayerStates currentState = PlayerStates.Idle;
    private PlayerStates previousState;
    private float horizontalInput;
    private float verticalInput;
    [SerializeField]
    private GameObject Sprite;
    private Rigidbody2D rb;
    [SerializeField]
    private WallChecker wc;
    private int stateID;
    private Animator animator;
    [SerializeField]
    private GameObject PlayerDeathPrefab;
    enum PlayerMode
    {
        Normal,
        AI
    }
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

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;
    }
    public void PausePlayer()
    {
        paused = true;
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
    }

    public void UnpausePlayer()
    {
        paused = false;
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
            FindObjectOfType<FXManager>().PlayOneShot(1);
        }
        else
        {
            Destroy(gameObject);
        }
        var s = Instantiate(PlayerDeathPrefab, this.transform.position, Quaternion.identity);
        Destroy(s, 1.0f);
    }
    void FixedUpdate()
    {
        if (paused)
        {
            rb.velocity = Vector3.zero;
            PausePlayer();
        }
        else
        {
            if (currentState != PlayerStates.Pause)
            {
                horizontalInput = Input.GetAxis("Horizontal");
                verticalInput = Input.GetAxis("Vertical");
                if (horizontalInput != 0)
                {
                    verticalInput = 0;
                }
            }
            if (horizontalInput < 0 && wc.Left)
            {
                currentState = PlayerStates.Left;
            }
            if (horizontalInput > 0 && wc.Right)
            {
                currentState = PlayerStates.Right;
            }
            if (verticalInput < 0 && wc.Bottom)
            {
                currentState = PlayerStates.Down;
            }
            if (verticalInput > 0 && wc.Top)
            {
                currentState = PlayerStates.Top;
            }
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

            if(mode!=PlayerMode.AI)
            rb.velocity = moveVector;
        }
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
