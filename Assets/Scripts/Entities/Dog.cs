using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour {

    public float speed = 2;
    public float rotation = 3;

    private Rigidbody2D body;
    private PlayerObject player;
    private bool stunned = false;
    private Animator anim;
    private GameObject costume;

    //Power Ups
    private bool hasWool = false;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0;  
        body.drag = 0;
        body.freezeRotation = true;

        anim = GetComponent<Animator>();
        GetComponent<SpriteRenderer>().sprite = player.image;
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

    public void SetPlayer(PlayerObject playerObject)
    {
        player = playerObject;
        name = player.playerName.ToString();
    }

    //Stun
    public void stun(float stunTime)
    {
        body.velocity = Vector2.zero;
        stunned = true;
        anim.SetBool("stunned", true);
        StartCoroutine(removeStun(stunTime));
    }
    IEnumerator removeStun(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        stunned = false;
        anim.SetBool("stunned", false);
    }

    //Wool PowerUp
    public void AddWool(float duration, GameObject costume)
    {
        hasWool = true;
        costume.transform.parent = transform;
        costume.transform.localPosition = Vector3.zero;
        this.costume = costume;
        StartCoroutine(RemoveWool(duration));
    }
    IEnumerator RemoveWool(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        hasWool = false;
        Destroy(costume);
    }
    public bool IsSheep()
    {
        return hasWool;
    }

    //SpeedUp
    public void SpeedUp(float duration, float add)
    {
        speed += add;
        StartCoroutine(RemoveSpeed(duration, add));
    }
    IEnumerator RemoveSpeed(float seconds, float sub)
    {
        yield return new WaitForSeconds(seconds);
        speed -= sub;
    }
}
