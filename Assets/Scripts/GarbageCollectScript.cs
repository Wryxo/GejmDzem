﻿using UnityEngine;
using System.Collections;

public class GarbageCollectScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerExit2D(Collider2D other)
    {
        Destroy(other.gameObject);
    }
}
