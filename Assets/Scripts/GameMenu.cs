using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour {

    //public AudioManager audioManager;
    public GameManager gameManager;

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
        Debug.Log("Music volume: " + slider.value);
        //audioManager.
    }

    public void ChangeAmbientVolume(Slider slider)
    {
        Debug.Log("Ambient Sounds volume: " + slider.value);
    }

    public void ChangeSFXVolume(Slider slider)
    {
        Debug.Log("Sound Effects volume: " + slider.value);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
