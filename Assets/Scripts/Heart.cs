using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Object.Destroy(gameObject);
            GameManager.Instance.HeartPickup();
            Debug.Log("Star coolided with player");
        }
    }
}
