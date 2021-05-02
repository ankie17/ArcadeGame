using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(gameObject);
            GameManager.Instance.StarPickup();
        }
    }
    public Vector2 GetStarPos()
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
