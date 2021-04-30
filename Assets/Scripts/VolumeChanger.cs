using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeChanger : MonoBehaviour
{
    public Slider musicSlider;
    public Slider oneshotSlider;
    public AudioSource audioSource;
    private string mv = "MusicVolume";
    private string ov = "OneShotVolume";
    void Start()
    {
        if (PlayerPrefs.HasKey(mv))
            musicSlider.value = PlayerPrefs.GetFloat(mv);

        if (PlayerPrefs.HasKey(ov))
            oneshotSlider.value = PlayerPrefs.GetFloat(ov);
    }

    // Update is called once per frame
    void Update()
    {
        ChangeMusicVolumeValue();
    }
    public void WriteVolumeValue()
    {
        PlayerPrefs.SetFloat(mv, musicSlider.value);
        PlayerPrefs.Save();
        PlayerPrefs.SetFloat(ov, oneshotSlider.value);
        PlayerPrefs.Save();
    }
    private void ChangeMusicVolumeValue()
    {
        if(PlayerPrefs.HasKey(mv))
        audioSource.volume = PlayerPrefs.GetFloat(mv);
    }
}
