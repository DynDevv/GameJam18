using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEnabler : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Ambient");
        FindObjectOfType<AudioManager>().Play("Music");
    }
}
