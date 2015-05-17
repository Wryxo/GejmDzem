using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class LoadScript : MonoBehaviour {

    public string Scene;

    // Use this for initialization
    void Start () {
        Debug.Log("Start");
	}
	
	// Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        Debug.Log(Scene);
        Application.LoadLevel(Scene);
    }
}
