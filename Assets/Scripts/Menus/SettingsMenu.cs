using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SettingsMenu : MonoBehaviour {

    public GameManager gameManager;
    PlayerObject[] players;
    TextMeshProUGUI errorMessage;
    private Event keyEvent;
    private KeyCode newKey;
    private bool waitingForKey;

    void Start()
    {
        //TODO ev load new scene with playerlist from gameManager if given

        gameManager = FindObjectOfType<GameManager>();
        players = gameObject.GetComponentsInChildren<PlayerObject>();
        foreach(PlayerObject p in players)
        {
            p.gameObject.transform.Find("name").GetComponent<TextMeshProUGUI>().SetText(p.playerName.ToString());
            p.gameObject.transform.Find("icon").GetComponent<Image>().sprite = p.icon;
            setLeftButtonText(p, p.left.ToString());
            setRightButtonText(p, p.right.ToString());
            TogglePlayer(p);
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
            ShowSettingsErrorDialog("Choose at least 1 Player and be sure to assign your controls.");
        }
        else*/
        {
            gameManager.StartGame(playerList);
        }
	}

    public void ShowSettingsErrorDialog(string output)
    {
        errorMessage.SetText(output);
    }

    #region KEYCODES

    public void AssignLeftKey(PlayerObject player)
    {
        StartAssignment("left", player);
    }

    public void AssignRightKey(PlayerObject player)
    {
        StartAssignment("right", player);
    }

    private void setLeftButtonText(PlayerObject p, string value)
    {
        p.gameObject.transform.Find("leftButton").GetComponentInChildren<TextMeshProUGUI>().SetText(value);
    }

    private void setRightButtonText(PlayerObject p, string value)
    {
        p.gameObject.transform.Find("rightButton").GetComponentInChildren<TextMeshProUGUI>().SetText(value);
    }

    //public static IEnumerator WaitInput()
    //{
    //    bool wait = true;
    //    while (wait)
    //    {
    //        if (Input.anyKeyDown)
    //        {
    //            KeyCode pressed = (KeyCode)Enum.Parse(typeof(KeyCode), Input.inputString, true);
    //            Debug.Log(pressed);
    //            wait = false;
    //        }
    //        yield return null;
    //    }
    //}

    private void OnGUI()
    {
        keyEvent = Event.current;

        if(keyEvent.isKey && waitingForKey)
        {
            newKey = keyEvent.keyCode;
            waitingForKey = false;
        }
    }

    private void StartAssignment(string keyName, PlayerObject player)
    {
        if (!waitingForKey)
            StartCoroutine(AssignKey(keyName, player));
    }

    private IEnumerator WaitForKey()
    {
        while (!keyEvent.isKey)
            yield return null;
    }

    private IEnumerator AssignKey(string keyName, PlayerObject player)
    {
        waitingForKey = true;
        yield return WaitForKey();

        switch (keyName)
        {
            case "left":
                setLeftButtonText(player, newKey.ToString());
                player.left = newKey;
            break;
            case "right":
                setRightButtonText(player, newKey.ToString());
                player.right = newKey;
            break;
        }
    }

    #endregion

    #region TOGGLE PLAYER

    public void TogglePlayer(PlayerObject player)
    {
        //int index = Array.IndexOf(players, player);
        Toggle toggle = player.gameObject.GetComponentInChildren<Toggle>();
        player.active = toggle.isOn;

        foreach (Button b in player.gameObject.GetComponentsInChildren<Button>())
        {
            b.interactable = toggle.isOn;
        }

        ToggleImageVisibility(player, toggle.isOn);
        ToggleImageVisibility(player, toggle.isOn);
    }

    private void ToggleImageVisibility(PlayerObject p, bool visible)
    {
        Image image = p.gameObject.transform.Find("icon").GetComponent<Image>();
        var tempColor = image.color;
        if (visible)
        {
            tempColor.a = 1f;
        } else
        {
            tempColor.a = 0.75f;
        }
        image.color = tempColor;
    }

    private void ToggleNameVisibility(PlayerObject p, bool visible)
    {
        TextMeshProUGUI text = p.gameObject.transform.Find("icon").GetComponent<TextMeshProUGUI>();
        var tempColor = text.color;
        if (visible)
        {
            tempColor.a = 1f;
        }
        else
        {
            tempColor.a = 0.75f;
        }
        text.color = tempColor;
    }

    #endregion

    #region SLIDER

    public void AdjustTimeSlider(GameObject Time)
    {
        //read slider value
        Slider slider = Time.GetComponentInChildren<Slider>();
        int seconds = 30 + ((int)slider.value * 5);

        //output as min and sec to value text
        string text = System.Math.Floor(seconds / 60f) + " min " + seconds % 60 + " sec";
        Time.transform.Find("Value").GetComponent<TextMeshProUGUI>().SetText(text);

        //change in settings
        gameManager.timeLimit = (seconds);
    }

    public void AdjustHerdSlider(GameObject Herd)
    {
        //read slider value
        Slider slider = Herd.GetComponentInChildren<Slider>();

        //output to value text
        Herd.transform.Find("Value").GetComponent<TextMeshProUGUI>().SetText(slider.value + " sheep");

        //change in settings
        gameManager.sheepLimit = ((int)slider.value);
    }

    #endregion
}
