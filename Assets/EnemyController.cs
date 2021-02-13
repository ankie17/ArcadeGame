using System.Collections;
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
    // Start is called before the first frame update
    enum EnemyStates
    {
        Move = 0,
        Pause
    }
    private EnemyStates currentState = 0;
    void Start()
    {
  
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

    // Update is called once per frame
    void Update()
    {
        
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
    }
    public void EnemyUnpause()
    {
        currentState = EnemyStates.Move;
    }
    private void Move()
    {
        LookAt2D(transform, vectorsArray[currentPointID]);
        enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, vectorsArray[currentPointID], speedDelta);
    }
    void LookAt2D(Transform me, Vector3 target)
    {
        float signedAngle = Vector2.SignedAngle(me.up, target - me.position);

        if (Mathf.Abs(signedAngle) >= 1e-3f)
        {
            var angles = me.eulerAngles;
            angles.z += signedAngle;
            me.eulerAngles = angles;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            FindObjectOfType<GameManager>().PlayerHurt();
            Debug.Log("Enemy collided with player");
        }
    }
}
