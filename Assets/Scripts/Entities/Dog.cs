using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour {

    private Rigidbody2D body;
    public float speed = 1;
    public float rotation = 1;
    private Player player;

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
        body.velocity = transform.right * speed;
        if (Input.GetKey("a"))
            body.rotation += rotation;
        if (Input.GetKey("d"))
            body.rotation -= rotation;
	}

    public void SetPlayer(Player player)
    {
        this.player = player;
    }
}
