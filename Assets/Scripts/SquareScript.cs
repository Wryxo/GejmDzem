using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SquareScript : MonoBehaviour {

    private Transform _transform;
    private MasterScript _masterScript;
    private float _transtime=1.0f;

    public int[] colorsLeft;
    public int[] colorsRight;
    public SquareScript predecessor;
    public SquareScript ancestor;
    public SpriteRenderer childLeft;
    public SpriteRenderer childRight;
    public int set = 0;
    public bool walkable = false;
    public bool zamok = false;
    public PowerupScript power;

    private bool clicked;

    // Use this for initialization
    void Start () {
        _transform = GetComponent<Transform>();
        _masterScript = (GameObject.FindGameObjectWithTag("GameController")).GetComponent<MasterScript>();

        set = 0;

        childLeft.color = _masterScript.colors[colorsLeft[set]];
        childRight.color = _masterScript.colors[colorsRight[set]];

        checkWalkable();
    }

    // Update is called once per frame
    void Update () {
        
    }

    void FixedUpdate()
    {
        if (!_masterScript.pause) {
            
            if (_transtime < 1.0f)
            {
                _transtime += 2*Time.deltaTime;
                Mathf.Clamp(_transtime, 0.0f, 1.0f);
            }
            if (!walkable)
            {
                childLeft.transform.localScale = Vector2.Lerp(new Vector2(1.0f, 1.0f), new Vector2(0.7f, 1.0f), _transtime);
                childRight.transform.localScale = Vector2.Lerp(new Vector2(1.0f, 1.0f), new Vector2(0.7f, 1.0f), _transtime);
            }
            else
            {
                childLeft.transform.localScale = Vector2.Lerp(new Vector2(0.7f, 1.0f), new Vector2(1.0f, 1.0f),_transtime);
                childRight.transform.localScale = Vector2.Lerp(new Vector2(0.7f, 1.0f), new Vector2(1.0f, 1.0f),_transtime);
            }
        }
        _transform.position += new Vector3(-_masterScript.speed * Time.deltaTime, 0.0f);
        /*_playerTransform = (GameObject.FindGameObjectWithTag("Player")).GetComponent<Transform>();
        if (childLeft.transform.position.x < _playerTransform.position.x)
        {
            childLeft.color = new Color(1.0f, 0.5f, 0.0f);
        }
        if ((childRight.transform.position.x + childRight.bounds.size.x) < _playerTransform.position.x)
        {
            childRight.color = new Color(1.0f, 0.5f, 0.0f);
        }*/
    }

    void OnMouseExit()
    {
        if (clicked) {
            clicked = false;
            var tmp = colorsLeft[set];
            colorsLeft[set] = colorsRight[set];
            colorsRight[set] = tmp;

            var tmpc = childLeft.color;
            childLeft.color = childRight.color;
            childRight.color = tmpc;

            checkWalkable();
            ancestor.checkWalkable();

            if (power != null)
            {
                power.checkPlatform();
            }
        }
    }

    void OnMouseOver()
    {
        if (!_masterScript.pause)
        {
            if (!zamok)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    clicked = false;
                    set++;
                    set = set % colorsLeft.Length;
                    childLeft.color = _masterScript.colors[colorsLeft[set]];
                    childRight.color = _masterScript.colors[colorsRight[set]];

                    checkWalkable();
                    ancestor.checkWalkable();

                    if (power != null)
                    {
                        power.checkPlatform();
                    }
                }
                if (Input.GetMouseButtonDown(0))
                {
                    clicked = true;
                }
                if (Input.GetMouseButtonDown(1))
                {
                    Debug.Log("Right");
                    var tmp = colorsLeft[set];
                    colorsLeft[set] = colorsRight[set];
                    colorsRight[set] = tmp;

                    var tmpc = childLeft.color;
                    childLeft.color = childRight.color;
                    childRight.color = tmpc;

                    checkWalkable();
                    ancestor.checkWalkable();

                    if (power != null)
                    {
                        power.checkPlatform();
                    }
                }
            }
        }
    }

    public void checkWalkable()
    {
        bool orig = walkable;
        if (predecessor != null)
        {
            if (predecessor.getColor(true) == colorsLeft[set])
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
        if (orig != walkable) _transtime = 0f;
    }

    public int getColor(bool direction)
    {
        // false = lavy, true = pravy
        return direction ? colorsRight[set] : colorsLeft[set];
    }
}
