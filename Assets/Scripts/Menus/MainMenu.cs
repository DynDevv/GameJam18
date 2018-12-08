using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public AudioMixer audioMixer;

    public void Start()
    {
        audioMixer = FindObjectOfType<AudioMixer>();
    }

    public void Mute()
    {
        audioMixer.MuteAll(true);
    }

    public void Unmute()
    {
        audioMixer.MuteAll(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
