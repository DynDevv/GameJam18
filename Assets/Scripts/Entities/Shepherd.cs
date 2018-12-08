using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shepherd : MonoBehaviour {

    public float triggerRadius = 0.3f;
    public float hitTime = 2;
    public float stunTime = 2;
    public float readyTime = 2;
    
    private Dog ownDog;
    private List<Dog> otherDogs = new List<Dog>();
    private float timer = 0;
    private bool ready = true;
    private Animator anim;

	// Use this for initialization
	void Start () {
        readyTime += stunTime;
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (otherDogs.Count > 0)
        {
            Vector3 direction = transform.position - otherDogs[0].transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //transform.LookAt(anotherDog.transform);
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                foreach (Dog doggo in otherDogs)
                {
                    doggo.stun(stunTime);
                }
                otherDogs.Clear();
                ready = false;
                anim.SetBool("attacking", true);
                StartCoroutine(makeReady(readyTime));
            }
        }
	}

    public void SetOwnDog(Dog own)
    {
        ownDog = own;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Dog tempDog = collision.GetComponent<Dog>();
        if (ready && tempDog && tempDog != ownDog && !otherDogs.Contains(tempDog))
        {
            if (otherDogs.Count==0)
            {
                timer = hitTime;
            }
            otherDogs.Add(tempDog);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Dog tempDog = collision.GetComponent<Dog>();
        if (tempDog)
        {
            otherDogs.Remove(tempDog);
        }
    }

    private void disableAttacking()
    {
        anim.SetBool("attacking", false);
    }

    IEnumerator makeReady(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ready = true;
    }
}
