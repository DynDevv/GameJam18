using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioMixer : MonoBehaviour
{

    [Range(0f, 1f)]
    public float AmbientVolume;

    [Range(0f, 1f)]
    public float MusicVolume;

    [Range(0f, 1f)]
    public float FXVolume;

    private AudioManager audiomanager;

    private float mute = 1;

    private void Start()
    {
        audiomanager = gameObject.GetComponent<AudioManager>();
    }

    private void Update()
    {
        foreach (Sound s in audiomanager.sounds)
        {
            if (s.type == 0)
            {
                s.volume = AmbientVolume * 0.8f * mute;
            }

            if (s.type == 1)
            {
                s.volume = MusicVolume * 0.7f * mute;
            }

            if (s.type == 2)
            {
                s.volume = FXVolume * mute;
            }
        }
    }

    public void EditSlider(int type, float value)
    {
        if (type == 0)
        {
            AmbientVolume = value;
        }

        if (type == 1)
        {
            MusicVolume = value;
        }

        if (type == 2)
        {
            FXVolume = value;
        }
    }

    public void MuteAll(bool yesno)
    {
        if (yesno == false)
        {
            WaitUp();
            //StartCoroutine(Wait(mute, yesno, value =>
            //{
            //    Debug.Log(value.ToString());
            //}));
        }

        if (yesno == true)
        {
            WaitDown();
            //StartCoroutine(Wait(mute, yesno, value =>
            //{
            //    Debug.Log(value.ToString());
            //}));
        }
    }

    //    IEnumerator Wait(float a, bool up, Action<float> onComplete)
    //    {
    //        if (up == true)
    //        {
    //            a = a - 0.1f;
    //            yield return new WaitForSeconds(0.1f);
    //        }
    //        if (up == false)
    //        {
    //            a = a + 0.1f;
    //            yield return new WaitForSeconds(0.1f);
    //        }
    //        onComplete(a);
    //    }

    //IEnumerator Wait()
    //{
    //    //Debug.Log("Before Waiting 2 seconds");
    //    yield return new WaitForSeconds(1);
    //    //Debug.LogError("After Waiting 2 Seconds");
    //}

    void WaitUp()
    {
        if (mute <= 1)
        {
            mute = mute + 0.1f;
            Invoke("WaitUp", 0.05f);
        }
        return;
    }

    void WaitDown()
    {
        if (mute >= 0)
        {
            mute = mute - 0.1f;
            Invoke("WaitDown", 0.05f);
        }
        return;
    }
}
