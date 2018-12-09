using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAudioEnabler : MonoBehaviour {

    private GameObject[] buttons;

    public void FindButtons()
    {
        buttons = GameObject.FindGameObjectsWithTag("Button");

        foreach (GameObject b in buttons)
        {
            Debug.LogError(b.name);
            b.GetComponent<Button>().onClick.AddListener(PlayButtonSound);
        }
    }

    void PlayButtonSound()
    {
        FindObjectOfType<AudioManager>().Play("Normal Button");
    }
}
