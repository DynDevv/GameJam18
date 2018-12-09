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
    private GameObject countdown;

    [Header("Gameplay Settings")]
    [Range(10, 30)]
    public int sheepLimit = 10;
    [Range(30, 300)]
    [Tooltip("Time in seconds")]
    public int timeLimit = 30;

    public bool muted = false;
    public GameObject dogPrefab, sheepPrefab, shepardPrefab;

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
        if(countdown == null)
            countdown = GameObject.Find("Countdown");

        countdown.gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().SetText(value.ToString());
    }

    IEnumerator InitAfterDelay()
    {
        yield return new WaitForSeconds(1.1f);
        UpdateCountdown("2");
        yield return new WaitForSeconds(1f);
        UpdateCountdown("1");
        yield return new WaitForSeconds(1f);
        UpdateCountdown("Start");
        yield return new WaitForSeconds(0.3f);
        countdown.SetActive(false);

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
            dog.GetComponent<Dog>().SetPlayer(player);

            GameObject shepard = Instantiate(shepardPrefab, spawn.transform.position, spawn.transform.rotation);
            shepard.transform.parent = GameObject.Find("Shepards").transform;
            shepard.GetComponent<Shepherd>().SetOwnDog(dog.GetComponent<Dog>());

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
        int maxSheep = 0;

        foreach (SpawnArea spawn in FindObjectsOfType<SpawnArea>())
            if (spawn.GetSheeps() > maxSheep)
                maxSheep = spawn.GetSheeps();

        Debug.Log("RESULT");
        foreach (SpawnArea spawn in FindObjectsOfType<SpawnArea>())
            if (spawn.GetOwner() != null && spawn.GetSheeps() == maxSheep)
                Debug.Log(spawn.GetOwner().name + ": " + spawn.GetSheeps());
    }

    public int GetTime()
    {
        return (int)time;
    }
}