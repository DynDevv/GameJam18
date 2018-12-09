using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour {

    [Header("0 - Ambient", order = 0)]
    [Space(-10, order = 1)]
    [Header("1 - Music", order = 2)]
    [Space(-10, order = 3)]
    [Header("2 - FX", order = 4)]
    [Space(10, order = 5)]

    public Sound[] sounds;


	void Awake ()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audio;
            s.source.loop = s.loop;
        }		
	}

    private void Update()
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Audio clip" + name + "not found!");
            return;
        }
        s.source.Play();
    }
}
