using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GrassLoader : MonoBehaviour
{
    [SerializeField]
    private Sprite[] grassSprites = new Sprite[4];
    private int counter = 0;

    [ContextMenu("LoadGrass")]
    public void LoadGrass()
    {
        for (int i = 0; i < 4; i++)
        {
            int id = Random.Range(0, 31);
            Texture2D texture = new Texture2D(28, 28);
            var bytes = File.ReadAllBytes($"Textures\\grass{id}.png");
            texture.LoadImage(bytes);
            Rect rect = new Rect(0f, 0f, texture.width, texture.height);
            Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 28);
            grassSprites[i] = sprite;
        }
    }
    public Sprite GetGrassSprite()
    {
        counter++;
        return grassSprites[counter%4];
    }
}
