using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour {

    private Rigidbody2D body;
    public float speed = 1;
    public float rotation = 1;
    private Player player;
    private bool stunned = false;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0;  
        body.drag = 0;
        body.freezeRotation = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (stunned)
            return;
        
        body.velocity = transform.right * speed;
        if (Input.GetKey("a"))
            body.rotation += rotation;
        if (Input.GetKey("d"))
            body.rotation -= rotation;
	}

    internal void stun(float stunTime)
    {
        body.velocity = Vector2.zero;
        stunned = true;
        StartCoroutine(removeStun(stunTime));
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    IEnumerator removeStun(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        stunned = false;
    }
}
