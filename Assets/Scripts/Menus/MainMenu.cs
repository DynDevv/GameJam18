using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public AudioMixer audioMixer;
    public GameManager gameManager;
    public GameObject muteButton;
    public GameObject unmuteButton;

    void Start()
    {
        audioMixer = FindObjectOfType<AudioMixer>();
        gameManager = FindObjectOfType<GameManager>();

        if (PlayerPrefs.GetInt("Muted") == 1)
            Mute();
        else
            Unmute();

        unmuteButton.SetActive(gameManager.muted);
        muteButton.SetActive(!gameManager.muted);
    }

    public void Mute()
    {
        gameManager.muted = true;
        audioMixer.MuteAll(true);
        PlayerPrefs.SetInt("Muted", 1);
        PlayerPrefs.Save();
    }

    public void Unmute()
    {
        gameManager.muted = false;
        audioMixer.MuteAll(false);
        PlayerPrefs.SetInt("Muted", 0);
        PlayerPrefs.Save();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
