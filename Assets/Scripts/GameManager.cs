using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Player> players;
    private List<GameObject> spawns;
    private float time = 0;
    private bool running = true;

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
        players = new List<Player>();
        spawns = new List<GameObject>();
        players.Add(new Player());
    }

	// Use this for initialization
	void Start ()
    {
        spawns.AddRange(GameObject.FindGameObjectsWithTag("Spawn"));
        StartGame();
    }
	
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            if (running = !running)
                PauseGame();
            else
                ContinueGame();
        }

        //UPDATE UI-TIME
        time += Time.deltaTime;

        if (running && time >= timeLimit)
            StopGame();
    }

    private void StartGame()
    {
        //SPAWN DOGS
        foreach (Player player in players)
        {
            GameObject spawn = spawns[(int)Random.Range(0, spawns.Count - 1)];
            GameObject dog = Instantiate(dogPrefab, spawn.transform.position, spawn.transform.rotation);

            spawn.GetComponent<SpawnArea>().SetOwner(dog);
            dog.GetComponent<Dog>().SetPlayer(player);

            dog.name = "Dog";
            dog.transform.parent = GameObject.Find("Dogs").transform;
            spawns.Remove(spawn);
        }

        //SPAWN SHEEPS
        for (int i = 0; i < sheepLimit; i++)
        {
            GameObject sheep = Instantiate(sheepPrefab, new Vector3(), new Quaternion());
            sheep.name = "Sheep" + (i + 1);
            sheep.transform.parent = GameObject.Find("Sheeps").transform;
        }
    }

    public static void PauseGame()
    {
        //map.SetActive(running);
        Debug.Log("PAUSED");
        Time.timeScale = 0;
    }

    public static void ContinueGame()
    {
        //map.SetActive(running);
        Time.timeScale = 1;
    }

    private void StopGame()
    {
        time = 0;
        int maxSheep = 0;

        foreach (SpawnArea spawn in FindObjectsOfType<SpawnArea>())
            if (spawn.GetSheeps() > maxSheep)
                maxSheep = spawn.GetSheeps();

        Debug.Log("WINNER");
        foreach (SpawnArea spawn in FindObjectsOfType<SpawnArea>())
            if (spawn.GetOwner() != null && spawn.GetSheeps() == maxSheep)
                Debug.Log(spawn.GetOwner().name + ": " + spawn.GetSheeps());

        PauseGame();
    }
}