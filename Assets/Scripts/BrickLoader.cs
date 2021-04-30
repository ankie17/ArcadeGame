using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class BrickLoader : MonoBehaviour
{
    public GameObject prefab;
    void Start()
    {
        for (int i = 0; i < 32; i++)
        {
            var spr = Resources.Load<Sprite>($"brick{i}");
            Instantiate(prefab, new Vector3(0, i), Quaternion.identity).GetComponent<SpriteRenderer>().sprite = spr;
        }
    }
}
