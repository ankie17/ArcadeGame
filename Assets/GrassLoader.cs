using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassLoader : MonoBehaviour
{
    public Sprite[] grassSprites = new Sprite[4];
    private int counter = 0;

    [ContextMenu("LoadGrass")]
    void LoadGrass()
    {
        for (int i = 0; i < 4; i++)
        {
            int id = Random.Range(0, 24);
            grassSprites[i] = Resources.Load<Sprite>($"grass{id}");
        }
    }
    public Sprite GetGrassSprite()
    {
        counter++;
        return grassSprites[counter%4];
    }
}
