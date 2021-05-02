using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeChanger : MonoBehaviour
{
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider oneshotSlider;
    [SerializeField]
    private AudioSource audioSource;
    private const string mv = "MusicVolume";
    private const string ov = "OneShotVolume";
    void Start()
    {
        if (PlayerPrefs.HasKey(mv))
            musicSlider.value = PlayerPrefs.GetFloat(mv);

        if (PlayerPrefs.HasKey(ov))
            oneshotSlider.value = PlayerPrefs.GetFloat(ov);
    }
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
