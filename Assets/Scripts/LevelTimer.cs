﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    public float levelTime;
    public bool running;
    // Start is called before the first frame update
    void Start()
    {
        levelTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            levelTime += Time.deltaTime;
        }
    }

    public void switchStates()
    {
        running = !running;
    }
}
