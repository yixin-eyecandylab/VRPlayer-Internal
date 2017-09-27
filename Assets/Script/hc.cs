using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class hc : MonoBehaviour {

	// Use this for initialization
	void Start () {
        HttpHelper.HttpHelper hp = new HttpHelper.HttpHelper();
        Debug.Log(hp.Getstring());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
