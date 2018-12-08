using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : PowerUp {

    public float duration = 3;
    public float addSpeed = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Dog dog = other.GetComponent<Dog>();

        if (dog)
        {
            dog.SpeedUp(duration, addSpeed);
            Destroy(gameObject);
        }
    }
}
