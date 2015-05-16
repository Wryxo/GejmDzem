using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlayerScript : MonoBehaviour
{
    private Transform _transform;
    private float _cd = 0.0f;
    private bool _stuck = false;
    private MasterScript _masterScript;

	// Use this for initialization
	void Start () {
        _transform = GetComponent<Transform>();
        _masterScript = (GameObject.FindGameObjectWithTag("GameController")).GetComponent<MasterScript>();
    }

    // Update is called once per frame
    void Update () {
	}

    private void FixedUpdate()
    {
        if (!_masterScript.pause)
        {
            var horzExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
            var _leftBound = -horzExtent + 1;
            if (_transform.position.x < _leftBound)
            {
                _masterScript.pause = true;
            }
            if (_stuck)
                _transform.position += new Vector3(-_masterScript.speed * Time.deltaTime, 0.0f, 0.0f);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!_masterScript.pause)
        {
            Debug.Log("Collide");
            var ss = other.gameObject.GetComponent<SquareScript>();
            if (ss != null)
            {
                if (!ss.walkable)
                {
                    _stuck = true;
                }
                else
                {
                    if (!ss.zamok)
                    {
                        _cd += Time.deltaTime;
                    }
                    if (_cd > 0.3f)
                    {
                        _stuck = false;
                        ss.zamok = true;
                        _cd = 0.0f;
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        var ss = other.gameObject.GetComponent<SquareScript>();
        if (ss != null)
            ss.createLife();
    }
}
