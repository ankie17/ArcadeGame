using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    Move = 0,
    Pause,
    Attacking
}

public class EnemyController : MonoBehaviour
{
    private int currentPointID;
    [SerializeField]
    private float MoveSpeed;
    private float speedDelta;
    private int direction;
    private Transform enemyTransform;
    private Vector3[] vectorsArray;
    private Animator animator;
    private EnemyStates currentState = EnemyStates.Move;
    public void SetVectorsArray(Vector3[] vectors)
    {
        vectorsArray = vectors;
    }
    public void Prepare()
    {
        animator = GetComponent<Animator>();
        speedDelta = MoveSpeed * Time.fixedDeltaTime;
        currentPointID = 0;
        enemyTransform = GetComponent<Transform>();
        direction = 1;

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
        currentState = EnemyStates.Pause;
        animator.speed = 0;
    }
    public void EnemyUnpause()
    {
        currentState = EnemyStates.Move;
        animator.speed = 1;
    }
    public void StartAttack()
    {
        currentState = EnemyStates.Attacking;
    }
    public void FinishAttack()
    {
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
        }
    }
}
