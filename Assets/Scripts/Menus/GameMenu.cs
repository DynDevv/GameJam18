using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour {

    public AudioMixer audioMixer;
    public GameManager gameManager;

    void Start()
    {
        audioMixer = FindObjectOfType<AudioMixer>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void PauseGame()
    {
        gameManager.ChangePauseState(true);
    }

    public void ResumeGame()
    {
        gameManager.ChangePauseState(false);
    }

    public void ChangeMusicVolume(Slider slider)
    {
        if (gameManager.muted)
        {
            gameManager.muted = false;
            audioMixer.MuteAll(false);
        }
        audioMixer.EditSlider(1, slider.value);
        //Debug.Log("Music volume: " + slider.value);
    }

    public void ChangeAmbientVolume(Slider slider)
    {
        if (gameManager.muted)
        {
            gameManager.muted = false;
            audioMixer.MuteAll(false);
        }
        audioMixer.EditSlider(0, slider.value);
        //Debug.Log("Ambient Sounds volume: " + slider.value);
    }

    public void ChangeSFXVolume(Slider slider)
    {
        if (gameManager.muted)
        {
            gameManager.muted = false;
            audioMixer.MuteAll(false);
        }
        audioMixer.EditSlider(2, slider.value);
        //Debug.Log("Sound Effects volume: " + slider.value);
    }

    public void RestartGame()
    {
        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("Game");
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
