using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnim : MonoBehaviour {
    public Animator Anim;
	// Use this for initialization
	void Start () {
        Anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space))
        {
            Anim.SetInteger("ActionID", 1);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Anim.SetInteger("ActionID", 2);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Anim.SetInteger("ActionID", 3);
        }
    }
}
