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
    public Vector3 target;
    public float moveSpeed = 5f;
    public double top;
    public double down;
    public double left;
    public double right;
    public double middle;

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
                FindPlayerPos();
            }
        }
    }
    private void FindPlayerPos()
    {
        double max=-999.0;
        int x = 0;
        int y = 0;
        //int counter = 0;
        for(int i = 0; i < 20; i++)
        {
            for(int j = 0; j < 20; j++)
            {
                if (ValueMatrix[i, j] > max)
                {
                    max = ValueMatrix[i, j];
                    x = j;
                    y = i;
                }
            }
        }
        print($"player position {x},{y}");
    }
    private void FindTarget()
    {
        List<Tuple<int, double>> values = new List<Tuple<int, double>>();
        {
            //middle = ValueMatrix[posY, posX];
            right = ValueMatrix[posY, posX + 1];
            left = ValueMatrix[posY, posX - 1];
            top = ValueMatrix[posY - 1, posX];
            down = ValueMatrix[posY + 1, posX];
            //top is down down is top?
        }
        //values.Add(new Tuple<int, double>(0, ValueMatrix[posY, posX]));//middle value
        values.Add(new Tuple<int, double>(1, ValueMatrix[posY, posX + 1]));//right value
        values.Add(new Tuple<int, double>(2, ValueMatrix[posY, posX - 1]));//left value
        values.Add(new Tuple<int, double>(3, ValueMatrix[posY - 1, posX]));//top value
        values.Add(new Tuple<int, double>(4, ValueMatrix[posY + 1, posX]));//down value
        foreach(var v in values)
        {
            //print($"index {v.Item1} value {v.Item2}");
        }
        List<int> maxIndexes = new List<int>(); //индексы максимальных значений

        //сортируем по возрастанию
        values = (from v in values
                  orderby v.Item2 descending
                  select v).ToList();

        double maxValue = values[0].Item2; //по умолчанию максимальное значение первое
        print(maxValue);
        foreach (var v in values) //в цикле добавляем индексы если есть клетки с таким же значением
        {
            if (v.Item2 == maxValue)
                maxIndexes.Add(v.Item1);
        }

        //select random index from list

        int random = UnityEngine.Random.Range(0, maxIndexes.Count - 1);

        int index = maxIndexes[random]; //присваиваем индекс

        Vector3 newTarget = target; //в зависимости от индекса клетки будем менять координаты сл. цели

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
        //newTarget.y = MirrorByMiddle(newTarget.y);
        //newTarget.x = MirrorByMiddle(newTarget.x);
        target = newTarget;
    }
    /*private void FindTarget() 
    {
        List<Tuple<int, double>> values = new List<Tuple<int, double>>();

        var middleVal = ValueMatrix[posY][posX]
    }*/
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
        //print(posX);
        //print(posY);
    }
    public void EnemyPause()
    {
        Debug.Log("Pause");
        currentState = EnemyStates.Pause;
        animator.speed = 0;
    }
    public void EnemyUnpause()
    {
        Debug.Log("Unpause");
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
        if (collision.tag == "Player")
        {
            GameManager.Instance.PlayerHurt();
            animator.SetTrigger("Attack");
        }
    }
}
