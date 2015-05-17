using UnityEngine;
using System.Collections;

public class BlackholeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	 
        var hor = Camera.main.orthographicSize * Screen.width / Screen.height;
        transform.position = new Vector3(-hor+0.5f, transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
