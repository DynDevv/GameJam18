using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioMixer : MonoBehaviour
{

    [Range(0f, 1f)]
    public float AmbientVolume;

    [Range(0f, 1f)]
    public float MusicVolume;

    [Range(0f, 1f)]
    public float FXVolume;

    private AudioManager audiomanager;

    private int mute = 1;

    private void Start()
    {
        audiomanager = gameObject.GetComponent<AudioManager>();
    }

    private void Update()
    {
        foreach (Sound s in audiomanager.sounds)
        {
            if (s.type == 0)
            {
                s.volume = AmbientVolume * mute;
            }

            if (s.type == 1)
            {
                s.volume = MusicVolume * mute;
            }

            if (s.type == 2)
            {
                s.volume = FXVolume * mute;
            }
        }
    }

    public void EditSlider(int type, float value)
    {
        if (type == 0)
        {
            AmbientVolume = value;
        }

        if (type == 1)
        {
            MusicVolume = value;
        }

        if (type == 2)
        {
            FXVolume = value;
        }
    }

    public void MuteAll(bool yesno)
    {
        if (yesno == true)
        {
            mute = 0;
        }

        if (yesno == false)
        {
            mute = 1;
        }
    }
}
