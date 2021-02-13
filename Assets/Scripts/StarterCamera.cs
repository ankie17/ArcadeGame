using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterCamera : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Debug.Log("Awake on Starter Camera");
    }
    private void Start()
    {
        Debug.Log("Start on Starter Camera");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update on Starter Camera");
    }

    private void LateUpdate()
    {
        Debug.Log("LateUpdate on Starter Camera");
    }
}
