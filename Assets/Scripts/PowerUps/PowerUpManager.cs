using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour {

    public Transform locations;
    public float spawnTime = 10;
    public float spawnTimeSpread = 5;
    public List<GameObject> list = new List<GameObject>();

    private bool ready = true;
    private float timer;

	// Use this for initialization
	void Awake () {
        timer = NewTimer();
	}
	
	// Update is called once per frame
	void Update () {
        if (ready)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            timer = NewTimer();
            ready = false;

            int rand1 = Random.Range(0, list.Count);
            int rand2 = Random.Range(0, locations.childCount);
            Transform tempTrans = locations.GetChild(rand2);
            GameObject tempUp = Instantiate(list[rand1],tempTrans.position, tempTrans.rotation);
            tempUp.GetComponent<PowerUp>().SetManager(this);
        }
	}

    private float NewTimer()
    {
        return spawnTime + Random.Range(-spawnTimeSpread, spawnTimeSpread);
    }

    public void Free()
    {
        ready = true;
    }
}
