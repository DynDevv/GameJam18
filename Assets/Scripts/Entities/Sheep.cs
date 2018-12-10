using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour {

    public float force = 5f;
    public float triggerRadius = 1f;
    public float centerMovement = 8;

    private Rigidbody2D body;
    private Vector3 movement;
    private Animator anim;

    private CircleCollider2D trigger;
    private Vector3 targetPosition;
    private bool inSpawner;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        trigger = GetComponent<CircleCollider2D>();
        trigger.radius = triggerRadius;
	}

    // Update is called once per frame
    void Update () {

        Vector3 slowDir = inSpawner ? targetPosition - transform.position : (targetPosition - transform.position).normalized / centerMovement;
        body.AddForce(slowDir * Time.deltaTime * 50);

        anim.SetFloat("speed", body.velocity.magnitude);
        if(body.velocity.magnitude > 0.05)
        {
            float angle = Mathf.Atan2(body.velocity.y, body.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            body.velocity = Vector2.zero;
        }
    }
    

    private void Run(Collider2D collision)
    {
        Dog tempDog = collision.GetComponent<Dog>();
        if (tempDog)
        {
            if (tempDog.IsSheep())
            {
                body.AddForce((collision.transform.position - transform.position).normalized * force * 4);
            }
            else
            {
                body.AddForce((transform.position - collision.transform.position).normalized * force);
            }
            
        }
    }

    private void HerdRun(Collision2D collision)
    {
        if (collision == null)
            return;

        Sheep tempSheep = collision.collider.GetComponent<Sheep>();

        if (tempSheep && tempSheep.body.velocity.magnitude > body.velocity.magnitude)
        {
            body.AddForce((transform.position - collision.transform.position).normalized * force);
        }

        if(collision.collider.tag == "Wall")
        {
            //*Working but not perfect
            Vector3 collisionPoint = collision.GetContact(0).point;
            body.AddForce((transform.position - collisionPoint).normalized * force * 2);
            //*/

            /*//Not working
            Vector3 collisionPoint = collision.GetContact(0).point;
            Vector3 direction = (collisionPoint - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            body.AddForce(Vector3.forward * force * force);
            //*/
        }
    }

    public void MoveSoftly(Vector3 target, bool entered)
    {
        targetPosition = target;
        inSpawner = entered;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Run(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Run(collision);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        HerdRun(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HerdRun(collision);
    }
}
