using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour {

    public float force = 5f;
    public float triggerRadius = 0.2f;

    private Rigidbody2D body;
    private Vector3 movement;

    private CircleCollider2D trigger;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0;
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
        }
    }
    

    private void run(Collider2D collision)
    {
        if (collision.GetComponent<Dog>())
        {
            body.AddForce((transform.position - collision.transform.position).normalized * force);
        }
    }

    private void herdRun(Collision2D collision)
    {
        Sheep tempSheep = collision.collider.GetComponent<Sheep>();

        if (tempSheep && tempSheep.body.velocity.magnitude > body.velocity.magnitude)
        {
            body.AddForce((transform.position - collision.transform.position).normalized * force);
        }

        if(collision.collider.tag == "Wall")
        {
            //*Working but not perfect
            Vector3 collisionPoint = collision.GetContact(0).point;
            body.AddForce((transform.position - collisionPoint).normalized * force * force);
            //*/

            /*//
            Vector3 collisionPoint = collision.GetContact(0).point;
            Vector3 direction = (collisionPoint - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            body.AddForce(Vector3.forward * force * force);
            //*/
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
        herdRun(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        herdRun(collision);
    }
}
