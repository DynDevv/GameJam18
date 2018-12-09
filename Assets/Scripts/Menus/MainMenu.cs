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
    }

    private void OnGUI()
    {
        //TODO clean that because it is called several times each update
        unmuteButton.SetActive(gameManager.muted);
        muteButton.SetActive(!gameManager.muted);
    }

    public void Mute()
    {
        gameManager.muted = true;
        audioMixer.MuteAll(true);
    }

    public void Unmute()
    {
        gameManager.muted = false;
        audioMixer.MuteAll(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
