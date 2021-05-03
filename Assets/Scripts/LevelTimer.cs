using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    public float levelTime;
    private bool running;
    void Start()
    {
        running = true;
        levelTime = 0;
    }
    void Update()
    {
        if (running)
        {
            levelTime += Time.deltaTime;
        }
    }

    public void SwitchStates()
    {
        running = !running;
    }
}
