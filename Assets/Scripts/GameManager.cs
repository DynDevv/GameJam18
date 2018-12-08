using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private List<PlayerObject> players;
    private List<GameObject> spawns;
    private float time = 0;
    private bool running = true;
    public Transform dogs, sheeps;

    [Header("Gameplay Settings")]
    [Range(10, 30)]
    public int sheepLimit = 10;
    [Range(30, 300)]
    [Tooltip("Time in seconds")]
    public int timeLimit = 30;
    public GameObject dogPrefab;
    public GameObject sheepPrefab;

    void Awake()
    {
        spawns = new List<GameObject>();
        players = new List<PlayerObject>();
    }

	// Use this for initialization
	void Start ()
    {
        spawns.AddRange(GameObject.FindGameObjectsWithTag("Spawn"));
        //players.Add(new Player());
        //StartGame(players);
    }
	
    void Update()
    {
        time += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
            ChangePauseState(!running);

        if (running && time >= timeLimit)
            StopGame();
    }

    public void StartGame(List<PlayerObject> playerList)
    {
        SceneManager.UnloadSceneAsync("Menu");
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);

        if (playerList == null)
            return;

        players = playerList;
        time = 0;

        //SPAWN DOGS
        foreach (PlayerObject player in players)
        {
            GameObject spawn = spawns[(int)Random.Range(0, spawns.Count)];
            GameObject dog = Instantiate(dogPrefab, spawn.transform.position, spawn.transform.rotation);

            spawn.GetComponent<SpawnArea>().SetOwner(dog);
            dog.GetComponent<Dog>().SetPlayer(player);

            //NULL
            dog.transform.parent = GameObject.Find("Dogs").transform;
            spawns.Remove(spawn);
        }

        //SPAWN SHEEPS
        for (int i = 0; i < sheepLimit; i++)
        {
            GameObject sheep = Instantiate(sheepPrefab, new Vector3(), new Quaternion(0,0, Random.Range(0, 360), 0));
            //NULL
            sheep.transform.parent = GameObject.Find("Sheeps").transform;
            sheep.name = "Sheep" + (i + 1);
        }
    }

    public void ChangePauseState(bool state)
    {
        running = state;
        //Time.timeScale = (running) ? 1 : 0;
    }
    private void StopGame()
    {
        int maxSheep = 0;

        foreach (SpawnArea spawn in FindObjectsOfType<SpawnArea>())
            if (spawn.GetSheeps() > maxSheep)
                maxSheep = spawn.GetSheeps();

        Debug.Log("WINNER");
        foreach (SpawnArea spawn in FindObjectsOfType<SpawnArea>())
            if (spawn.GetOwner() != null && spawn.GetSheeps() == maxSheep)
                Debug.Log(spawn.GetOwner().name + ": " + spawn.GetSheeps());
    }

    public int GetTime()
    {
        return (int)time;
    }

    public void SetTimeLimit(int time)
    {
        timeLimit = time;
    }

    public void SetSheepLimit(int sheep)
    {
        sheepLimit = sheep;
    }
}