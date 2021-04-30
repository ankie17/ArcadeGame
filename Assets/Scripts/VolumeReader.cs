using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeReader : MonoBehaviour
{
    void Start()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
        //print($"MusicVolume : {PlayerPrefs.GetFloat("MusicVolume")}");
        GetComponent<FXManager>().VolumeScale = PlayerPrefs.GetFloat("OneShotVolume");
        //print($"MusicVolume : {PlayerPrefs.GetFloat("OneShotVolume")}");
    }
}
