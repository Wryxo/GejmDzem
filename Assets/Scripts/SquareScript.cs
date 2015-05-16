using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SquareScript : MonoBehaviour {

    private SpriteRenderer _spriteRenderer;
    private Color[] _colors = new Color[] { Color.red, Color.blue, Color.green, Color.magenta, Color.cyan };
    private Transform _transform;
    private MasterScript _masterScript;

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
        _masterScript = (GameObject.FindGameObjectWithTag("GameController")).GetComponent<MasterScript>();

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
                childLeft.transform.localScale = new Vector2(1.0f, 3.0f);
                childRight.transform.localScale = new Vector2(1.0f, 3.0f);
            } else
            {
                childLeft.transform.localScale = new Vector2(1.0f, 1.0f);
                childRight.transform.localScale = new Vector2(1.0f, 1.0f);
            }
        }
        _transform.position += new Vector3(-_masterScript.speed * Time.deltaTime, 0.0f);
    }

    void OnMouseOver()
    {
        if (!_masterScript.pause)
        {
            if (!zamok)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    left = UnityEngine.Random.Range(0, 5);
                    right = UnityEngine.Random.Range(0, 5);

                    childLeft.color = _colors[left];
                    childRight.color = _colors[right];

                    checkWalkable();
                    ancestor.checkWalkable();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    var tmp = left;
                    left = right;
                    right = tmp;

                    var tmpc = childLeft.color;
                    childLeft.color = childRight.color;
                    childRight.color = tmpc;

                    checkWalkable();
                    ancestor.checkWalkable();
                }
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

    public void createLife()
    {
        childLeft.color = new Color(1.0f, 0.5f, 0.0f);
        childRight.color = new Color(1.0f, 0.5f, 0.0f);
    }
}
