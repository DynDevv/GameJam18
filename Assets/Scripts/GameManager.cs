using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private List<PlayerObject> players;
    private List<GameObject> spawns;
    private float time = 0;
    private bool running = false;
    private GameMenu menu;

    [Header("Gameplay Settings")]
    [Range(10, 30)]
    public int sheepLimit = 10;
    [Range(30, 300)]
    [Tooltip("Time in seconds")]
    public int timeLimit = 30;

    public bool muted = false;
    public GameObject dogPrefab, sheepPrefab, shepardPrefab;

    void Start()
    {
        SceneManager.LoadScene("Menu");
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
            time += Time.deltaTime;

            if (time >= timeLimit)
                StopGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            ChangePauseState(!running);
    }

    public void StartGame(List<PlayerObject> playerList)
    {
        SceneManager.UnloadSceneAsync("Menu");
        SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);

        if (playerList != null)
            players = playerList;
        else
            return;

        Time.timeScale = 1;
        StartCoroutine(InitAfterDelay());        
    }

    private void UpdateCountdown(string value)
    {
        if (menu == null)
            menu = FindObjectOfType<GameMenu>();

        menu.SetCountdownText(value.ToString());
    }

    IEnumerator InitAfterDelay()
    {
        yield return new WaitForSeconds(1.1f);
        UpdateCountdown("2");
        yield return new WaitForSeconds(1f);
        UpdateCountdown("1");
        yield return new WaitForSeconds(1f);
        UpdateCountdown("START");
        yield return new WaitForSeconds(0.3f);
        menu.SetCountdownActive(false);

        spawns.AddRange(GameObject.FindGameObjectsWithTag("Spawn"));
        running = true;
        time = 0;

        //SPAWN DOGS & SHEPARDS
        foreach (PlayerObject player in players)
        {
            GameObject spawn = spawns[(int)Random.Range(0, spawns.Count)];
            GameObject dog = Instantiate(dogPrefab, spawn.transform.position, spawn.transform.rotation);

            spawn.GetComponent<SpawnArea>().SetOwner(dog);
            dog.transform.parent = GameObject.Find("Dogs").transform;
            //PlayerObject playerScript = dog.AddComponent<PlayerObject>();
            //playerScript = player;
            dog.GetComponent<Dog>().SetPlayer(player);

            GameObject shepard = Instantiate(shepardPrefab, spawn.transform.position, spawn.transform.rotation);
            shepard.transform.parent = GameObject.Find("Shepards").transform;
            shepard.GetComponentInChildren<Shepherd>().SetOwnDog(dog.GetComponent<Dog>());
            shepard.GetComponentInChildren<Shepherd>().SetHat(player.hat);

            shepard.transform.Rotate(new Vector3(0, 0, 1), 180);
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

    public int GetTime()
    {
        return (int)time;
    }
}