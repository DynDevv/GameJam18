using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHuhGenerator : MonoBehaviour {

    public float mintime = 4;
    public float maxtime = 16;
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
            //playRandomSound();
        }
    }

    void playRandomSound() {
        randomnumber = Random.Range(1, 9);
        FindObjectOfType<AudioManager>().Play("Natural " + randomnumber.ToString("00"));
    }

    public void playAngrySound()
    {
        randomnumber = Random.Range(1, 3);
        if (randomnumber == 1)
        {
            randomnumber = Random.Range(1, 9);
            FindObjectOfType<AudioManager>().Play("Natural " + randomnumber.ToString("00"));
        }
        else
        {
            randomnumber = Random.Range(1, 8);
            FindObjectOfType<AudioManager>().Play("Angry " + randomnumber.ToString("00"));
        }
    }

    public void playHappySound()
    {
        randomnumber = Random.Range(1, 5);
        FindObjectOfType<AudioManager>().Play("Happy " + randomnumber.ToString("00"));
    }

    public void PlaySweep()
    {
        randomnumber = Random.Range(1, 3);
        FindObjectOfType<AudioManager>().Play("whip " + randomnumber);
    }
}
