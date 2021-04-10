using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GrassLoader : MonoBehaviour
{
    public List<Sprite> grassSprites = new List<Sprite>();
    private int counter = 0;
    void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            int id = Random.Range(0, 24);
            grassSprites.Add(Resources.Load<Sprite>($"grass{id}"));
        }
    }
    public Sprite GetGrassSprite()
    {
        counter++;
        return grassSprites[0];
    }
}
