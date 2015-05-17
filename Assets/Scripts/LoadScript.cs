using UnityEngine;
using System.Collections;

public class LoadScript : MonoBehaviour {

    public string Scene = "GameScene";

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
    void Update()
    {

    }

	void OnMouseUp()
    {
        Application.LoadLevel(Scene);
    }
}
