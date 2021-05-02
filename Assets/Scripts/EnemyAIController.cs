using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    public int Id;
    public double[,] ValueMatrix = new double[20, 20];
    public int posX, posY;
    private Vector3 target;
    public float moveSpeed = 5f;
    private EnemyStates currentState;
    private Animator animator;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();

        UpdatePosition();

        target = transform.position;

        FindTarget();

        EnemyPause();
    }
    private void FixedUpdate()
    {
        if (currentState==EnemyStates.Move)
        {
            //обновить координаты матрицы
            UpdatePosition();

            if (transform.position != target) //если координаты не целые двигаться до точки назначения
            {
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.fixedDeltaTime);
            }
            else //если целые то вычислить следующую точку
            {
                FindTarget();
            }
        }
    }
    private void FindTarget()
    {
        List<Tuple<int, double>> values = new List<Tuple<int, double>>();
        values.Add(new Tuple<int, double>(1, ValueMatrix[posY, posX + 1]));//right value
        values.Add(new Tuple<int, double>(2, ValueMatrix[posY, posX - 1]));//left value
        values.Add(new Tuple<int, double>(3, ValueMatrix[posY - 1, posX]));//top value
        values.Add(new Tuple<int, double>(4, ValueMatrix[posY + 1, posX]));//down value
        List<int> maxIndexes = new List<int>();

        values = (from v in values
                  orderby v.Item2 descending
                  select v).ToList();

        double maxValue = values[0].Item2;
        
        foreach (var v in values)
        {
            if (v.Item2 == maxValue)
                maxIndexes.Add(v.Item1);
        }

        int random = UnityEngine.Random.Range(0, maxIndexes.Count - 1);

        int index = maxIndexes[random];

        Vector3 newTarget = target; 

        if (index == 1)
        {
            newTarget.x += 1;
        }
        else if (index == 2)
        {
            newTarget.x -= 1;
        }
        else if (index == 3)
        {
            newTarget.y += 1;
        }
        else if (index == 4)
        {
            newTarget.y -= 1;
        }

        target = newTarget;
    }
    private int MirrorYByMiddle(int y)
    {
        int mirrored = 19 - y;
        return mirrored;
    }
    private void UpdatePosition()
    {
        int intX = (int)transform.position.x;
        float dX = transform.position.x-intX;
        if (dX > 0.5f)
        {
            posX = Mathf.CeilToInt(transform.position.x);
        }
        else
        {
            posX = Mathf.FloorToInt(transform.position.x);
        }

        int intY = (int)transform.position.y;
        float dY = transform.position.y - intY;
        if (dY > 0.5f)
        {
            posY = Mathf.CeilToInt(transform.position.y);
        }
        else
        {
            posY = Mathf.FloorToInt(transform.position.y);
        }
        posY = MirrorYByMiddle(posY);
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {
            GameManager.Instance.PlayerHurt();
            animator.SetTrigger("Attack");
        }
        
    }
}
