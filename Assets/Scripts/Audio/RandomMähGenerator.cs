using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMähGenerator : MonoBehaviour {

    public float mintime = 3;
    public float maxtime = 12;
    int randomnumber;
    float time;
    bool countingDown;

    private void Start()
    {
        countingDown = false;
        randomnumber = 1;
    }

    void Update () {
        if (countingDown == false)
        {
            time = Random.Range(mintime, maxtime);
            countingDown = true;
        } else
        {
            time = time - Time.deltaTime;
        }

        if (time <= 0)
        {
            countingDown = false;
            time = 0;
            playRandomSound();
        }
    }

    void playRandomSound() {
        randomnumber = Random.Range(1, 12);
        FindObjectOfType<AudioManager>().Play("Mäh " + randomnumber.ToString("00"));
    }
}
