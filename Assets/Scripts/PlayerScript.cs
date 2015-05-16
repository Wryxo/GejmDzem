using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("start panacik");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay2D(Collider2D other)
    {
        var ss = other.gameObject.GetComponent<SquareScript>();
        if (ss != null)
        {
            Debug.Log("Collide: " + ss.Id);
        }
    }
}
