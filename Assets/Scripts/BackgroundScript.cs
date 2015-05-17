using UnityEngine;
using System.Collections;

public class BackgroundScript : MonoBehaviour {
    public float Speed = 0.05f;
    private MasterScript _masterScript;
    private float _horzExtent;

	// Use this for initialization
	void Start () {
        _masterScript = (GameObject.FindGameObjectWithTag("GameController")).GetComponent<MasterScript>();
        _horzExtent = _masterScript.GetHorzExtent();
    }
	void FixedUpdate()
    {
        transform.position += new Vector3(-Speed, 0, 0);
        if (transform.position.x < -_horzExtent)
        {
            var pos = new Vector3(3 * _horzExtent, transform.position.y, transform.position.z);
            transform.position = pos;
           
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
