using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
        FindObjectOfType<AudioEnabler>().FindButtons();

        yield return new WaitForSeconds(0.3f);
        if (menu == null) menu = FindObjectOfType<GameMenu>();

        yield return new WaitForSeconds(0.8f);
        UpdateCountdown("2");
        yield return new WaitForSeconds(1f);
        UpdateCountdown("1");
        yield return new WaitForSeconds(1f);
        UpdateCountdown("START");
        yield return new WaitForSeconds(0.3f);
        menu.SetCountdownActive(false);

        spawns.Clear();
        spawns.AddRange(GameObject.FindGameObjectsWithTag("Spawn"));
        menu.ShowIngameUI(players);
        time = timeLimit;
        running = true;

        //SPAWN DOGS & SHEPHERDS
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

        //int maxSheep = 0;
        //foreach (SpawnArea spawn in FindObjectsOfType<SpawnArea>())
        //    if (spawn.GetSheeps() > maxSheep)
        //        maxSheep = spawn.GetSheeps();

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