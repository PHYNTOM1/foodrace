using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public Slider masterS;
    public Slider musicS;
    public Slider soundsS;

    public AudioMixer generalAM;

    void Start()
    {
        if (generalAM == null)
        {
            generalAM = FindObjectOfType<SoundManagement>().generalM;
        }

        if (masterS == null)
        {
            masterS = GameObject.Find("MasterSlider").GetComponent<Slider>();
            masterS.minValue = 0.0001f;
            masterS.maxValue = 1f;
        }
        if (musicS == null)
        {
            musicS = GameObject.Find("MusicSlider").GetComponent<Slider>();
            musicS.minValue = 0.0001f;
            musicS.maxValue = 1f;
        }
        if (soundsS == null)
        {
            soundsS = GameObject.Find("SoundsSlider").GetComponent<Slider>();
            soundsS.minValue = 0.0001f;
            soundsS.maxValue = 1f;
        }
    }

    public void SetMasterVolume(float vol)
    {
        generalAM.SetFloat("MasterVolume", (Mathf.Log10(vol) * 20));
    }

    public void SetMusicVolume(float vol)
    {
        generalAM.SetFloat("MusicVolume", (Mathf.Log10(vol) * 20));
    }

    public void SetSoundsVolume(float vol)
    {
        generalAM.SetFloat("SoundsVolume", (Mathf.Log10(vol) * 20));
    }
}
