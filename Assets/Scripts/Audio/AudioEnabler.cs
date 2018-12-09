using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioEnabler : MonoBehaviour
{

    public GameObject[] buttons;
    public GameObject backbutton;

    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Ambient");
        FindObjectOfType<AudioManager>().Play("Music");
    }

    public void findButtons()
    {
        buttons = GameObject.FindGameObjectsWithTag("Button");
        backbutton = GameObject.Find("backButton");
        Debug.LogError(backbutton);

        foreach (GameObject b in buttons)
        {
            b.GetComponent<Button>().GetComponent<Button>().onClick.AddListener(PlayButtonSound);
            Debug.LogError(b);
        }

        backbutton.GetComponent<Button>().GetComponent<Button>().onClick.AddListener(PlayBackSound);
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
