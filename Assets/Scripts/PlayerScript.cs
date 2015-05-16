using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlayerScript : MonoBehaviour
{

    private Transform _transform;
    private float _speed = 1.0f; 

	// Use this for initialization
	void Start () {
        Debug.Log("start panacik");
        _transform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void FixedUpdate()
    {
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Collide");
        var ss = other.gameObject.GetComponent<SquareScript>();
        if (ss != null)
        {
            Debug.Log("not null");
            if (!ss.walkable)
            {
                Debug.Log("walkable");
                _transform.position += new Vector3(-_speed * Time.deltaTime, 0.0f, 0.0f);
            }
        }
    }
}
