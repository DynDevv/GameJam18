using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderChange : MonoBehaviour {

    [Header("0 - Ambient", order = 0)]
    [Space(-10, order = 1)]
    [Header("1 - Music", order = 2)]
    [Space(-10, order = 3)]
    [Header("2 - FX", order = 4)]
    [Space(10, order = 5)]

    public Slider slider;
    public Transform audioManager;

    [Range(0, 2)]
    public int type;

    void Update () {
        audioManager.GetComponent<AudioMixer>().EditSlider(type, slider.value);
    }
}
