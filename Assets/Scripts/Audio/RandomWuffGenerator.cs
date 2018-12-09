using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWuffGenerator : MonoBehaviour {

    public float mintime = 6;
    public float maxtime = 18;
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

    public void playRandomSound() {
        randomnumber = Random.Range(1, 6);
        FindObjectOfType<AudioManager>().Play("Dog " + randomnumber);
    }
}
