﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform[] pointsArray;
    private int currentPointID;
    public float MoveSpeed;
    private float speedDelta;
    private int direction;
    private Transform enemyTransform;
    private Vector3[] vectorsArray;
    private Animator animator;
    // Start is called before the first frame update
    enum EnemyStates
    {
        Move = 0,
        Pause
    }
    private EnemyStates currentState = EnemyStates.Move;
    void Start()
    {
        animator = GetComponent<Animator>();
        speedDelta = MoveSpeed * Time.fixedDeltaTime;
        currentPointID = 0;
        enemyTransform = GetComponent<Transform>();
        direction = 1;

        vectorsArray = new Vector3[pointsArray.Length];

        for (int i=0; i < pointsArray.Length; i++)
        {
            vectorsArray[i] = pointsArray[i].position;
        }

        
        enemyTransform.position = vectorsArray[0];
        
    }
    private void FixedUpdate()
    {
        if (currentState == EnemyStates.Move)
        {
            if (enemyTransform.position != vectorsArray[currentPointID])
            {
                Move();
            }
            else
            {

                if (currentPointID == vectorsArray.Length - 1)
                {
                    direction = -1;
                }
                if (currentPointID == 0)
                {
                    direction = 1;
                }

                currentPointID += direction;
            }
        }
    }
    public void EnemyPause()
    {
        Debug.Log("Pause");
        currentState = EnemyStates.Pause;
    }
    public void EnemyUnpause()
    {
        Debug.Log("Unpause");
        currentState = EnemyStates.Move;
    }
    private void Move()
    {
        enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, vectorsArray[currentPointID], speedDelta);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.Instance.PlayerHurt();
            animator.SetTrigger("Attack");
            Debug.Log("Enemy collided with player");
        }
    }
}
