using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SettingsMenu : MonoBehaviour {

    public GameManager gameManager;
    PlayerObject[] players;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        players = gameObject.GetComponentsInChildren<PlayerObject>();
        foreach(PlayerObject p in players)
        {
            p.gameObject.transform.Find("name").GetComponent<TextMeshPro>().SetText(p.playerName.ToString());
            p.gameObject.transform.Find("icon").GetComponent<Image>().sprite = p.icon;
        }
    }

    public void PlayGame () {
        List<PlayerObject> playerList = new List<PlayerObject>();
        bool error = false;
        
        foreach (PlayerObject p in players)
        {
            if (p.active)
            {
                if (p.left != null && p.right != null)
                {
                    playerList.Add(p);
                }
                else
                {
                    error = true;
                    break;
                }
            }
        }

        /*if (error || playerList.Count <= 0)
        {
            ShowSettingsErrorDialog();
        }
        else*/
        {
            gameManager.StartGame(playerList);
        }
	}

    public void ShowSettingsErrorDialog()
    {
        Debug.Log("Settings Error! Choose at least 1 Player and be sure to assign your controls.");
    }

    public void AssignKeyCode()
    {
        //listen for input
        //assign keycode to player
        //change button text to key
    }

    public void TogglePlayer(PlayerObject player)
    {
        //int index = Array.IndexOf(players, player);
        Toggle toggle = player.gameObject.GetComponentInChildren<Toggle>();
        player.active = toggle.isOn;

        foreach (Button b in player.gameObject.GetComponentsInChildren<Button>())
        {
            b.interactable = toggle.isOn;
        }
<<<<<<< HEAD

        ToggleImageVisibility(toggle.isOn);
    }

    private void ToggleImageVisibility(bool visible)
    {
        Image image = GetComponent<Image>();
        var tempColor = image.color;
        if (visible)
        {
            tempColor.a = 1f;
        } else
        {
            tempColor.a = 0.75f;
        }
        image.color = tempColor;
=======
>>>>>>> d5361c4c5f1b1ff07a9fe1fda2e8643e00409d96
    }

    public void AdjustTimeSlider(GameObject Time)
    {
        //read slider value
        Slider slider = Time.GetComponentInChildren<Slider>();

        //output as min and sec to value text
        string text = System.Math.Floor(slider.value / 60) + " min " + slider.value % 60 + " sec";
        Time.transform.Find("Value").GetComponent<TextMeshPro>().SetText(text);

        //change in settings
        gameManager.timeLimit = ((int)slider.value);
    }

    public void AdjustHerdSlider(GameObject Herd)
    {
        //read slider value
        Slider slider = Herd.GetComponentInChildren<Slider>();

        //output to value text
        Herd.transform.Find("Value").GetComponent<TextMeshPro>().SetText(slider.value + " sheep");

        //change in settings
        gameManager.sheepLimit = ((int)slider.value);
    }
}
