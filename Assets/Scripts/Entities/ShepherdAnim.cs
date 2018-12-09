using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShepherdAnim : MonoBehaviour {

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}

    private void DisableAttacking()
    {
        anim.SetBool("attacking", false);
    }
}
