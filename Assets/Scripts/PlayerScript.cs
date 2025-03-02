﻿using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Transform _transform;
    private float _cd = 0.0f;
    private bool _stuck = false;
    private MasterScript _masterScript;
    private Animator _animator;

	// Use this for initialization
	void Start () {
        _transform = GetComponent<Transform>();
        _masterScript = (GameObject.FindGameObjectWithTag("GameController")).GetComponent<MasterScript>();
        _animator = GetComponent<Animator>();
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
                Text tmp = (GameObject.FindGameObjectWithTag("GG")).GetComponent<Text>();
                tmp.text = "Game Over";
                tmp = (GameObject.Find("Restart")).GetComponent<Text>();
                tmp.text = "Click to restart";
            }
            if (_stuck)
            {
                _transform.position += new Vector3(-_masterScript.speed * Time.deltaTime, 0.0f, 0.0f);
                _animator.SetInteger("anim_state", 2);
            } else
            {
                if (transform.position.x < 0.0f)
                {
                    _transform.position += new Vector3(_masterScript.healingSpeed*Time.deltaTime, 0.0f, 0.0f);
                }
                _animator.SetInteger("anim_state", 0);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!_masterScript.pause)
        {
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

    void OnTriggerExit2D(Collider2D other)
    {
        if (!_masterScript.pause)
        {
            var ss = other.gameObject.GetComponent<SquareScript>();
            if (ss != null)
            {
                _masterScript.addCombo(ss.colorsLeft[ss.set], ss.colorsRight[ss.set]);
                _masterScript.queue.Remove(ss.gameObject);
                Destroy(ss.gameObject);
                _masterScript.score += 1;
                //_masterScript.Shoot(Random.Range(0,3));
                _masterScript.addNextCube();
            } else
            {
                SpriteRenderer sr = other.gameObject.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            } 
        }
    }
}
