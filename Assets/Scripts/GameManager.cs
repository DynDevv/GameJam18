using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private List<PlayerObject> players;
    private List<SpawnArea> spawns;
    private float time, timer = 1f;
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
        spawns = new List<SpawnArea>();
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

        if(timer >= 1f && time <= startTimer)
        {
            timer = 0f;
            GameObject.Find("Timer").GetComponent<TextMeshProUGUI>().SetText(((int)Mathf.Clamp(time + 1, 1, 30)).ToString());
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

        time = 999f;
        color.a = 0f;
        Time.timeScale = 1f;
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

        spawns.Clear();
        List<GameObject> tempSpawns = new List<GameObject>();
        tempSpawns.AddRange(GameObject.FindGameObjectsWithTag("Spawn"));

        foreach (PlayerObject player in players)
        {
            GameObject temp = tempSpawns[(int)Random.Range(0, tempSpawns.Count)];
            tempSpawns.Remove(temp);

            SpawnArea spawn = temp.GetComponent<SpawnArea>();
            spawn.SetOwner(player);
            spawns.Add(spawn);

            GameObject shepherd = Instantiate(shepherdPrefab, spawn.transform.position, spawn.transform.rotation);
            shepherd.transform.parent = GameObject.Find("Shepherds").transform;
            shepherd.transform.Rotate(new Vector3(0, 0, 1), 180);
            shepherd.GetComponent<Shepherd>().SetOwner(player);

            if (startIcons)
                AddStartIcon(spawn);
        }

        yield return new WaitForSeconds(0.8f);
        UpdateCountdown("2");
        yield return new WaitForSeconds(1f);
        UpdateCountdown("1");
        yield return new WaitForSeconds(1f);
        UpdateCountdown("START");
        RemoveStartIcons();
        yield return new WaitForSeconds(0.3f);

        menu.SetCountdownActive(false);
        menu.ShowIngameUI(players);
        time = timeLimit;
        running = true;

        //SPAWN DOGS & ASSIGN SPAWN
        foreach (SpawnArea spawn in spawns)
        {
            GameObject dog = Instantiate(dogPrefab, spawn.transform.position, spawn.transform.rotation);
            dog.transform.parent = GameObject.Find("Dogs").transform;
            dog.GetComponent<Dog>().SetOwner(spawn.GetOwner());
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
    
    private void AddStartIcon(SpawnArea spawn)
    {
        GameObject representation = Instantiate(menu.IconPrefab, spawn.transform.GetChild(0).transform.position, menu.IconPrefab.transform.rotation);
        representation.transform.Find("icon").GetComponent<Image>().sprite = spawn.GetOwner().icon;
        representation.transform.parent = GameObject.Find("startIcons").transform;
    }

    private void RemoveStartIcons()
    {
        foreach (Transform icon in GameObject.Find("startIcons").transform)
            Destroy(icon.gameObject);
    }

    public void ChangePauseState(bool state)
    {
        running = !state;
        Time.timeScale = (running) ? 1f : 0f;
    }

    private void StopGame()
    {
        ChangePauseState(true);
        menu.ShowResults(spawns);
        GameObject.Find("Timer").SetActive(false);
        FindObjectOfType<RandomHuhGenerator>().playHappySound();  
    }

    public List<PlayerObject> GetPlayers()
    {
        return players;
    }
}