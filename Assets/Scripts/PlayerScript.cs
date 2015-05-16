using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlayerScript : MonoBehaviour
{

    private bool _stuck = false;
    private Transform _transform;
    private float _speed = 0.0f; 

	// Use this for initialization
	void Start () {
        Debug.Log("start panacik");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        Debug.Log("LATE UPDATE");
        if (_stuck)
        {
            _speed = 1.0f;
        }
    }

    private void FixedUpdate()
    {
        _transform.position += new Vector3(-_speed, 0.0f);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        var ss = other.gameObject.GetComponent<SquareScript>();
        if (ss != null)
        {
            Debug.Log("Collide: " + ss.Id);
        }
        if (!other.GetComponent<SquareScript>().correct)
        {
            _stuck = true;
        }
    }
}
