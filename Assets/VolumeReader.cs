using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeReader : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<AudioSource>().volume = PlayerPrefs.GetFloat(Strings.StringNames.Volume.ToString());
    }
}
