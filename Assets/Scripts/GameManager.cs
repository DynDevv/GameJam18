using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private List<PlayerObject> players;
    private List<GameObject> spawns;
    private float time = 999;
    private float timer;
    private bool running = false;
    private Color color = Color.white;
    private GameMenu menu;

    [Header("Gameplay Settings")]
    [Range(10, 50)]
    public int sheepLimit = 40;
    [Range(30, 300)]
    [Tooltip("Time in seconds")]
    public int timeLimit = 120;
    [Range(10, 60)]
    public int startTimer = 30;

    public bool muted = false;
    public bool startIcons = true;
    public GameObject dogPrefab, sheepPrefab, shepherdPrefab;

    void Start()
    {
        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);
        FindObjectOfType<AudioEnabler>().FindButtons();
    }

    void Awake()
    {
        spawns = new List<GameObject>();
        players = new List<PlayerObject>();
    }
	
    void Update()
    {
        if (running)
        {
            time -= Time.deltaTime;

            if (time <= 0)
                StopGame();

            UpdateTimer();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            ChangePauseState(!running);
    }

    private void UpdateTimer()
    {
        //TODO SOMETIMES SKIPS A SECOND, TAKES LONGER THAN A SECOND AND SHOWS 1 ON SCORES
        timer += Time.deltaTime;

        if (color.a == 0f && time <= startTimer)
        {
            color.a = 1f;
            GameObject.Find("Timer").GetComponent<TextMeshProUGUI>().color = color;
        }

        if(timer >= 1f)
        {
            timer -= 1f;

            if ((int)time >= 0)
                GameObject.Find("Timer").GetComponent<TextMeshProUGUI>().SetText(((int)time + 1).ToString());
        }
    }

    public void StartGame(List<PlayerObject> playerList)
    {
        SceneManager.UnloadSceneAsync("Menu");
        SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);

        if (playerList != null)
            players = playerList;
        else
            return;

        color.a = 0f;
        Time.timeScale = 1;
        StopAllCoroutines();
        StartCoroutine(InitAfterDelay());        
    }

    private void UpdateCountdown(string value)
    {
        menu.SetCountdownText(value.ToString());
    }

    IEnumerator InitAfterDelay()
    {
        spawns.Clear();
        FindObjectOfType<AudioEnabler>().FindButtons();

        yield return new WaitForSeconds(0.3f);
        if (menu == null) menu = FindObjectOfType<GameMenu>();
        spawns.AddRange(GameObject.FindGameObjectsWithTag("Spawn"));

        //SPAWN SHEPHERDS & ASSIGN SPAWNS
        AddStartIcon(spawns[0].GetComponent<SpawnArea>());



        yield return new WaitForSeconds(0.8f);
        UpdateCountdown("2");
        yield return new WaitForSeconds(1f);
        UpdateCountdown("1");
        yield return new WaitForSeconds(1f);
        UpdateCountdown("START");
        yield return new WaitForSeconds(0.3f);
        menu.SetCountdownActive(false);

        menu.ShowIngameUI(players);
        RemoveStartIcons();
        time = timeLimit;
        running = true;

        //SPAWN DOGS & ASSIGN SPAWNS
        foreach (PlayerObject player in players)
        {
            GameObject spawn = spawns[(int)Random.Range(0, spawns.Count)];
            GameObject dog = Instantiate(dogPrefab, spawn.transform.position, spawn.transform.rotation);

            spawn.GetComponent<SpawnArea>().SetOwner(dog);
            dog.transform.parent = GameObject.Find("Dogs").transform;
            dog.GetComponent<Dog>().SetPlayer(player);

            GameObject shepherd = Instantiate(shepherdPrefab, spawn.transform.position, spawn.transform.rotation);
            shepherd.transform.parent = GameObject.Find("Shepherds").transform;
            shepherd.GetComponentInChildren<Shepherd>().SetOwnDog(dog.GetComponent<Dog>());
            shepherd.GetComponentInChildren<Shepherd>().SetHat(player.hat);

            shepherd.transform.Rotate(new Vector3(0, 0, 1), 180);
            spawns.Remove(spawn);

            //TODO CHECK WHERE DOG GETS SPAWNED AND SHOW ICON THERE
        }

        //SPAWN SHEEPS
        for (int i = 0; i < sheepLimit; i++)
        {
            GameObject sheep = Instantiate(sheepPrefab, new Vector3(), new Quaternion());

            sheep.transform.Rotate(new Vector3(0, 0, 1), Random.Range(0, 360));
            sheep.transform.parent = GameObject.Find("Sheeps").transform;
            sheep.name = "Sheep" + (i + 1);
        }
    }

    //IEnumerator InitAfterDelay()
    //{
    //    spawns.Clear();
    //    FindObjectOfType<AudioEnabler>().FindButtons();

    //    yield return new WaitForSeconds(0.3f);
    //    if (menu == null) menu = FindObjectOfType<GameMenu>();
    //    spawns.AddRange(GameObject.FindGameObjectsWithTag("Spawn"));

    //    //SPAWN SHEPHERDS & ASSIGN SPAWNS
    //    AddStartIcon(spawns[0].GetComponent<SpawnArea>());

    //    yield return new WaitForSeconds(0.8f);
    //    UpdateCountdown("2");
    //    yield return new WaitForSeconds(1f);
    //    UpdateCountdown("1");
    //    yield return new WaitForSeconds(1f);
    //    UpdateCountdown("START");
    //    yield return new WaitForSeconds(0.3f);
    //    menu.SetCountdownActive(false);

    //    menu.ShowIngameUI(players);
    //    RemoveStartIcons();
    //    time = timeLimit;
    //    running = true;

    //    //SPAWN DOGS & ASSIGN SPAWNS
    //    foreach (PlayerObject player in players)
    //    {
    //        GameObject spawn = spawns[(int)Random.Range(0, spawns.Count)];
    //        GameObject dog = Instantiate(dogPrefab, spawn.transform.position, spawn.transform.rotation);

    //        spawn.GetComponent<SpawnArea>().SetOwner(dog);
    //        dog.transform.parent = GameObject.Find("Dogs").transform;
    //        dog.GetComponent<Dog>().SetPlayer(player);

    //        GameObject shepherd = Instantiate(shepherdPrefab, spawn.transform.position, spawn.transform.rotation);
    //        shepherd.transform.parent = GameObject.Find("Shepherds").transform;
    //        shepherd.GetComponentInChildren<Shepherd>().SetOwnDog(dog.GetComponent<Dog>());
    //        shepherd.GetComponentInChildren<Shepherd>().SetHat(player.hat);

    //        shepherd.transform.Rotate(new Vector3(0, 0, 1), 180);
    //        spawns.Remove(spawn);

    //        //TODO CHECK WHERE DOG GETS SPAWNED AND SHOW ICON THERE
    //    }

    //    //SPAWN SHEEPS
    //    for (int i = 0; i < sheepLimit; i++)
    //    {
    //        GameObject sheep = Instantiate(sheepPrefab, new Vector3(), new Quaternion());

    //        sheep.transform.Rotate(new Vector3(0, 0, 1), Random.Range(0, 360));
    //        sheep.transform.parent = GameObject.Find("Sheeps").transform;
    //        sheep.name = "Sheep" + (i + 1);
    //    }
    //}

    private void AddStartIcon(SpawnArea spawn)
    {
        if(startIcons)
            foreach (PlayerObject player in players)
            {
                GameObject representation = Instantiate(menu.IngamePrefab, spawn.transform.position, menu.IngamePrefab.transform.rotation);
                representation.transform.SetParent(gameObject.transform.Find("startIcons").transform, false);
                representation.transform.Find("icon").GetComponent<Image>().sprite = player.icon;
                representation.transform.Find("name").GetComponent<TextMeshProUGUI>().SetText(player.playerName.ToString());
            }
    }

    private void RemoveStartIcons()
    {
        foreach (Transform icon in GameObject.Find("startIcons").transform)
            Destroy(icon.gameObject);
    }

    public void ChangePauseState(bool state)
    {
        running = !state;
        Time.timeScale = (running) ? 1 : 0;
    }

    private void StopGame()
    {
        color.a = 0f;
        GameObject.Find("Timer").GetComponent<TextMeshProUGUI>().color = color;
        FindObjectOfType<RandomHuhGenerator>().playHappySound();
        ChangePauseState(true);

        List<SpawnArea> list = new List<SpawnArea>();

        foreach (SpawnArea spawn in FindObjectsOfType<SpawnArea>())
            if (spawn.GetOwner() != null)
                list.Add(spawn);

        menu.ShowResults(list);
    }

    public List<PlayerObject> GetPlayers()
    {
        return players;
    }
}