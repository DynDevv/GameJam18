using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioEnabler : MonoBehaviour
{

    private GameObject[] buttons;
    private GameObject backbutton;

    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Ambient");
        FindObjectOfType<AudioManager>().Play("Music");
    }

    public void FindButtons()
    {
        StartCoroutine(delayedFind());
    }

    private IEnumerator delayedFind()
    {
        yield return new WaitForSeconds(0.3f);
        Debug.LogError("Finding Buttons:");
        buttons = GameObject.FindGameObjectsWithTag("Button");
        backbutton = GameObject.Find("backButton");

        foreach (GameObject b in buttons)
        {
            Debug.LogError(b.name);
            b.GetComponent<Button>().onClick.AddListener(PlayButtonSound);
        }

        if (backbutton != null)
        {
            backbutton.GetComponent<Button>().onClick.AddListener(PlayBackSound);
        }
    }

    public void FindButtonsInstant()
    {
        Debug.LogError("Finding Buttons:");
        buttons = GameObject.FindGameObjectsWithTag("Button");
        backbutton = GameObject.Find("backButton");

        foreach (GameObject b in buttons)
        {
            Debug.LogError(b.name);
            b.GetComponent<Button>().onClick.AddListener(PlayButtonSound);
        }

        if (backbutton != null)
        {
            backbutton.GetComponent<Button>().onClick.AddListener(PlayBackSound);
        }
    }

    void PlayButtonSound()
    {
        FindObjectOfType<AudioManager>().Play("Normal Button");
    }

    void PlayBackSound()
    {
        FindObjectOfType<AudioManager>().Play("Back Button");
    }
}
