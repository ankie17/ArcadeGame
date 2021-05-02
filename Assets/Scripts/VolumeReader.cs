using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeReader : MonoBehaviour
{
    void Start()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
        GetComponent<FXManager>().VolumeScale = PlayerPrefs.GetFloat("OneShotVolume");
    }
}
