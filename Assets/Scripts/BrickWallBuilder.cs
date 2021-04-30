﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BrickWallBuilder : MonoBehaviour
{
    public GameObject brickPrefab;
    private Vector3 position;
    private List<Sprite> sprites = new List<Sprite>();
    private int[] brickID;
    [ContextMenu("BuildWall")]
    void Start()
    {
        if (transform.childCount == 0) { 
        position = transform.position;
        //create 4 random ints from 0 to 31
        brickID = new int[4];
        for(int i = 0; i < brickID.Length; i++)
        {
            brickID[i] = Random.Range(0, 31);
        }
        //load sprites
        for (int i = 0; i < brickID.Length; i++)
        {
            Texture2D texture = new Texture2D(28, 14);
            var bytes = File.ReadAllBytes($"Textures\\brick{brickID[i]}.png");
            texture.LoadImage(bytes);
            Rect rect = new Rect(0f, 0f, texture.width, texture.height);
            Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 28);
            sprites.Add(sprite);
        }

            //build wall
            for (int i = 0; i < 40; i++)
            {

                if (i % 2 == 0)
                {
                    //build normal row
                    for (int j = 0; j < 20; j++)
                    {
                        Vector3 pos = position;
                        pos.y += 0.5f * ((float)i);
                        pos.x += j;
                        Instantiate(brickPrefab, pos, Quaternion.identity, transform).GetComponent<SpriteRenderer>().sprite = sprites[j % 2];
                    }
                }
                else
                {
                    Vector3 posFirst = position;
                    posFirst.y += 0.5f * ((float)i);
                    posFirst.x -= 0.25f;
                    //build half sized brick at the beginning
                    var firstBrick = Instantiate(brickPrefab, posFirst, Quaternion.identity, transform);
                    firstBrick.GetComponent<Transform>().localScale = new Vector3(0.5f, 1, 1);
                    firstBrick.GetComponent<SpriteRenderer>().sprite = sprites[2];
                    //build shorted row
                    for (int j = 1; j < 20; j++)
                    {
                        Vector3 pos = position;
                        pos.y += 0.5f * ((float)i);
                        pos.x -= 0.5f;
                        pos.x += j;
                        Instantiate(brickPrefab, pos, Quaternion.identity, transform).GetComponent<SpriteRenderer>().sprite = sprites[j % 2 + 2];
                    }
                    Vector3 posLast = position;
                    posLast.y += 0.5f * ((float)i);
                    posLast.x += 19.25f;
                    //build half sized brick at the beginning
                    var lastBrick = Instantiate(brickPrefab, posLast, Quaternion.identity, transform);
                    lastBrick.GetComponent<Transform>().localScale = new Vector3(0.5f, 1, 1);
                    lastBrick.GetComponent<SpriteRenderer>().sprite = sprites[2];
                    //build half sized brick at the and
                }
            }
        }
    }
}