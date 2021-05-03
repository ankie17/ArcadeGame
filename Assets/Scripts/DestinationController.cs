using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationController : MonoBehaviour
{
    [SerializeField]
    private AIDestinationSetter setter;

    private void FixedUpdate()
    {
        var stars = GameObject.FindGameObjectsWithTag("Star");
        int closestID = 0;
        float minDist = 999;
        int counter = 0;
        foreach (var s in stars)
        {
            float dist = Vector3.Distance(s.transform.position, transform.position);
            if (dist < minDist)
            {
                closestID = counter;
                minDist = dist;
            }
            counter++;
        }

        setter.target = stars[closestID].transform;
    }
}
