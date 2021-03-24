using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Object.Destroy(gameObject);
            GameManager.Instance.StarPickup();
            Debug.Log("Star coolided with player");
        }
    }
}
