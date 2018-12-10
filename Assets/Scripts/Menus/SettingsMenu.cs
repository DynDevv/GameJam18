using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SettingsMenu : MonoBehaviour {

    private GameManager gameManager;
    PlayerObject[] players;
    public TextMeshProUGUI errorMessage;
    private Event keyEvent;
    private KeyCode newKey;
    private bool waitingForKey;
    public GameObject timeObject;
    public GameObject herdObject;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        players = gameObject.GetComponentsInChildren<PlayerObject>();

        foreach(PlayerObject p in players)
        {
            p.gameObject.transform.Find("name").GetComponent<TextMeshProUGUI>().SetText(p.playerName.ToString());
            p.gameObject.transform.Find("icon").GetComponent<Image>().sprite = p.icon;
            setLeftButtonText(p, p.left.ToString());
            setRightButtonText(p, p.right.ToString());
            TogglePlayer(p);

            foreach (PlayerObject player in gameManager.GetPlayers())
            {
                if (p.playerName == player.playerName)
                {
                    p.left = player.left;
                    p.right = player.right;
                    setLeftButtonText(p, p.left.ToString());
                    setRightButtonText(p, p.right.ToString());
                    p.gameObject.GetComponentInChildren<Toggle>().isOn = true;
                    TogglePlayer(p);
                }
            }
        }

        LoadSliderValues();
    }

    public void PlayGame ()
    {
        List<PlayerObject> playerList = new List<PlayerObject>();
        bool error = false;
        
        foreach (PlayerObject p in gameObject.GetComponentsInChildren<PlayerObject>())
        {
            if (p.active)
            {
                if (p.left != KeyCode.None && p.right != KeyCode.None)
                    playerList.Add(p);
                else
                    error = true;
            }
        }

        if (!error && playerList.Count > 0)
        {
            SaveSliderValues();
            gameManager.StartGame(playerList);
        }
        else
        {
            SaveSliderValues();
            ShowSettingsErrorDialog("Choose at least one dog and be sure to assign all keys!");
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

    private void SaveSliderValues()
    {
        PlayerPrefs.SetInt("Time", (int)timeObject.GetComponentInChildren<Slider>().value);
        PlayerPrefs.SetInt("Herd", (int)herdObject.GetComponentInChildren<Slider>().value);
        PlayerPrefs.Save();
    }

    private void LoadSliderValues()
    {
        int time = PlayerPrefs.GetInt("Time");
        int herd = PlayerPrefs.GetInt("Herd");

        if (time > 0)
            timeObject.GetComponentInChildren<Slider>().value = time;

        if (herd > 0)
            herdObject.GetComponentInChildren<Slider>().value = herd;

        AdjustTimeSlider();
        AdjustHerdSlider();
    }

    public void AdjustTimeSlider()
    {
        //read slider value
        Slider slider = timeObject.GetComponentInChildren<Slider>();
        int seconds = 30 + ((int)slider.value * 5);

        //output as min and sec to value text
        string text = Math.Floor(seconds / 60f) + " min " + seconds % 60 + " sec";
        timeObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().SetText(text);

        //change in settings
        gameManager.timeLimit = (seconds);
    }

    public void AdjustHerdSlider()
    {
        //read slider value
        Slider slider = herdObject.GetComponentInChildren<Slider>();

        //output to value text
        herdObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().SetText(slider.value + " sheep");

        //change in settings
        gameManager.sheepLimit = ((int)slider.value);
    }

    #endregion
}
