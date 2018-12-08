using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour {

    public float speed = 0f;
    public float triggerRadius = 0.2f;
    public float stopDelays = 1;

    private Rigidbody2D body;
    private Vector3 movement;

    private CircleCollider2D trigger;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0;
        body.drag = 0;
        body.freezeRotation = true;

        trigger = GetComponent<CircleCollider2D>();
        trigger.radius = triggerRadius;
	}
	
	// Update is called once per frame
	void Update () {

        if(body.velocity.magnitude > 0.1)
        {
            float angle = Mathf.Atan2(body.velocity.y, body.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            body.velocity -= body.velocity * stopDelays * Time.deltaTime;
        }
        else
        {
            body.velocity = Vector2.zero;
        }
    }
    

    private void run(Collider2D collision)
    {
        
        if (collision.GetComponent<Dog>())
        {
            body.velocity = (transform.position - collision.transform.position).normalized * speed;
        }
    }

    private void herdRun(Collider2D collision)
    {
        Sheep tempSheep = collision.GetComponent<Sheep>();

        if (tempSheep && tempSheep.body.velocity.magnitude > body.velocity.magnitude)
        {
            body.velocity = tempSheep.body.velocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        run(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        run(collision);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        herdRun(collision.collider);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        herdRun(collision.collider);
    }
}
