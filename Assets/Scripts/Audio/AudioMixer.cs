using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioMixer : MonoBehaviour{

    [Range(0f, 1f)]
    public float AmbientVolume;

    [Range(0f, 1f)]
    public float MusicVolume;

    [Range(0f, 1f)]
    public float FXVolume;

    private AudioManager audiomanager;

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
                s.volume = AmbientVolume;
            }

            if (s.type == 1)
            {
                s.volume = MusicVolume;
            }

            if (s.type == 2)
            {
                s.volume = FXVolume;
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
}
