using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SquareScript : MonoBehaviour {

    private SpriteRenderer _spriteRenderer;
    private Color[] _colors = new Color[] { Color.red, Color.blue, Color.green, Color.magenta, Color.cyan };
    private Transform _transform;
    private MasterScript _masterScript;
    private float _speed = 3.0f;

    public SquareScript predecessor;
    public SquareScript ancestor;
    public SpriteRenderer childLeft;
    public SpriteRenderer childRight;
    public int left = -1;
    public int right = -1;
    public bool walkable = false;
    public bool zamok = false;

    // Use this for initialization
    void Start () {
        _transform = GetComponent<Transform>();
        _masterScript = (GameObject.FindGameObjectsWithTag("GameController"))[0].GetComponent<MasterScript>();

        left = UnityEngine.Random.Range(0, 5);
        right = UnityEngine.Random.Range(0, 5);

        childLeft.color = _colors[left];
        childRight.color = _colors[right];

        checkWalkable();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void FixedUpdate()
    {
        if (!_masterScript.pause) { 
            if (!walkable)
            {
                childLeft.transform.localScale = new Vector2(0.5f, 0.5f);
                childRight.transform.localScale = new Vector2(0.5f, 0.5f);
            } else
            {
                childLeft.transform.localScale = new Vector2(1.0f, 1.0f);
                childRight.transform.localScale = new Vector2(1.0f, 1.0f);
            }
        }
        _transform.position += new Vector3(-_speed * Time.deltaTime, 0.0f);
    }

    void OnMouseDown()
    {
        if (!_masterScript.pause)
        {
            Debug.Log("Click " + zamok);
            if (!zamok)
            {
                left = UnityEngine.Random.Range(0, 5);
                right = UnityEngine.Random.Range(0, 5);

                childLeft.color = _colors[left];
                childRight.color = _colors[right];

                checkWalkable();
                ancestor.checkWalkable();
            }
        }
    }

    public void checkWalkable()
    {
        if (predecessor != null)
        {
            if (predecessor.getColor(true) == left)
            {
                walkable = true;
            }
            else
            {
                walkable = false;
            }
        }
        else
        {
            walkable = true;
        }
    }

    public int getColor(bool direction)
    {
        // false = lavy, true = pravy
        return direction ? right : left;
    }
}
