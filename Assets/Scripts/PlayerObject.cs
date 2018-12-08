using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour {
    public bool active;
    public KeyCode left;
    public KeyCode right;

    public enum dogName { Fluffy, Fuzzy, Soft, Cuddly, Fleecy, Dog };
    public dogName playerName;
    //public string playerName;

    public Sprite icon;
    public Sprite image;

    public void Awake()
    {
        string iconPath = playerName.ToString() + "_icon.png";
        string imagePath = playerName.ToString() + "_image.png";
        icon = Resources.Load<Sprite>(iconPath);
        image = Resources.Load<Sprite>(imagePath);
    }

    //TODO Input.GetKeyDown(KeyCode.Space))
}
