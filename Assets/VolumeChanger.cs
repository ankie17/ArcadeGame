using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeChanger : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource audioSource;
    private string volume = Strings.StringNames.Volume.ToString();
    void Start()
    {
        if (PlayerPrefs.HasKey(volume))
        volumeSlider.value = PlayerPrefs.GetFloat(volume);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.HasKey(volume))
            ChangeVolumeValue(); 
    }
    public void WriteVolumeValue()
    {
        PlayerPrefs.SetFloat(volume, volumeSlider.value);
        PlayerPrefs.Save();
    }
    private void ChangeVolumeValue()
    {
        audioSource.volume = PlayerPrefs.GetFloat(volume);
    }
}
