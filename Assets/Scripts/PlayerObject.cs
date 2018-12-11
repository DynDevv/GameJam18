using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour {
    public bool active;
    public KeyCode left;
    public KeyCode right;

    public enum dogName { Fluffy, Cuddly, Fuzzy, Soft, Fleecy, Dog };
    public dogName playerName;

    public Sprite icon;
    public Sprite[] images;
    public Sprite hat;

    public void Awake()
    {
        string iconPath = "Icons/" + playerName.ToString() + "_dog";
        string imagePath = "Dogs/" + playerName.ToString() + "_parts";
        string hatPath = "Shepherd/" + playerName.ToString() + "_hat";
        icon = Resources.Load<Sprite>(iconPath);
        images = Resources.LoadAll<Sprite>(imagePath);
        hat = Resources.Load<Sprite>(hatPath);
    }
}