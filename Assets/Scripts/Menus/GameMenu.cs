using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameMenu : MonoBehaviour {

    public AudioMixer audioMixer;
    public GameManager gameManager;
    public GameObject ResultPrefab;

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

    public void SetCountdownActive(bool state)
    {
        gameObject.transform.Find("Countdown").gameObject.SetActive(state);
    }

    public void SetCountdownText(string value)
    {
        gameObject.transform.Find("Countdown").GetComponentInChildren<TextMeshProUGUI>().SetText(value);
    }

    public void ShowResults(List<SpawnArea> activeDogs)
    {
        gameObject.transform.Find("pauseButton").gameObject.SetActive(false);
        gameObject.transform.Find("ResultsMenu").gameObject.SetActive(true);

        // TODO sort

        gameObject.transform.Find("players");
        float height = ResultPrefab.transform.position.y;
        foreach (SpawnArea spawn in activeDogs)
        {
            // spawn at correct height
            Vector3 pos = new Vector3(ResultPrefab.transform.position.x, height, ResultPrefab.transform.position.z);
            GameObject score = Instantiate(ResultPrefab, pos, ResultPrefab.transform.rotation);

            //NULLPOINTER?
            score.transform.parent = gameObject.transform.Find("players").transform;
            height -= 100;

            // set rank, icon, name and score
            //spawn.GetOwner().GetComponent<Dog>().GetPlayer();
            //spawn.GetSheep();
            //output e.g. 1. 1. 3.
        }
    }

    public void RestartGame()
    {
        SceneManager.UnloadSceneAsync("Game");
        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);
        StartCoroutine(DelayedFindButtons());
    }

    private IEnumerator DelayedFindButtons()
    {
        yield return new WaitForSeconds(0.3f);
        FindObjectOfType<AudioEnabler>().findButtons();
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
