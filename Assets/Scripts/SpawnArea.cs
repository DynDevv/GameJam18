using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    private int sheep = 0;
    private PlayerObject player = null;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger && other.gameObject.tag == "Sheep")
        {
            //Debug.Log(other.gameObject.name + " ENTER " + name);
            sheep++;
            other.GetComponent<Sheep>().MoveSoftly(transform.GetChild(0).position, true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.isTrigger && other.gameObject.tag == "Sheep")
        {
            //Debug.Log(other.gameObject.name + " EXIT " + name);
            sheep--;
            other.GetComponent<Sheep>().MoveSoftly(Vector3.zero, false);
        }
    }

    public void SetOwner(PlayerObject owner)
    {
        player = owner;
    }

    public PlayerObject GetOwner()
    {
        return player;
    }

    public int GetSheep()
    {
        return sheep;
    }
}