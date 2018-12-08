using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    private int sheeps = 0;
    private GameObject owner = null;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger && other.gameObject.tag == "Sheep")
        {
            //Debug.Log(other.gameObject.name + " ENTER " + name);
            sheeps++;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger && other.gameObject.tag == "Sheep")
        {
            //Debug.Log(other.gameObject.name + " EXIT " + name);
            sheeps--;
        }
    }

    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
    }

    public GameObject GetOwner()
    {
        return owner;
    }

    public int GetSheeps()
    {
        return sheeps;
    }
}