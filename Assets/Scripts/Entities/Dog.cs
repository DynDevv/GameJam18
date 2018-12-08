using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour {

    public float speed = 1;
    public float rotation = 1;

    private Rigidbody2D body;
    private PlayerObject player;
    private bool stunned = false;
    private Animator anim;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0;  
        body.drag = 0;
        body.freezeRotation = true;

        anim = GetComponent<Animator>();
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
        anim.SetBool("stunned", true);
        StartCoroutine(removeStun(stunTime));
    }

    public void SetPlayer(PlayerObject playerObject)
    {
        player = playerObject;
        name = player.playerName.ToString();
    }

    IEnumerator removeStun(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        stunned = false;
        anim.SetBool("stunned", false);

    }
}
