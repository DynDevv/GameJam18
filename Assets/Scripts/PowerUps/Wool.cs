using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wool : PowerUp {

    public float duration = 3;
    public GameObject costume;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Dog dog = other.GetComponent<Dog>();

        if (dog)
        {
            dog.AddWool(duration, costume);
            Destroy(gameObject);
        }
    }
}
