using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SquareScript : MonoBehaviour {

    struct myGradient{
        public int left;
        public int right;
        public Sprite sprite;

        public myGradient(int l, int r, Sprite s)
        {
            left = l;
            right = r;
            sprite = s;
        }
    }

    private SpriteRenderer _spriteRenderer;
    private List<myGradient> _gradients = new List<myGradient>();
    private int _currentColor = 0;
    private Transform _transform;
    private float _speed = 1.0f;

    public int Id = -1;
    public SquareScript predecessor;
    public SquareScript ancestor;
    public bool correct = false;


    // Use this for initialization
    void Start () {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _transform = GetComponent<Transform>();

        Sprite tmps = (Sprite)Resources.Load("Sprites/s1", typeof(Sprite));
        _gradients.Add(new myGradient(1, 2, tmps));
        tmps = (Sprite)Resources.Load("Sprites/s2", typeof(Sprite));
        _gradients.Add(new myGradient(2, 3, tmps));
        tmps = (Sprite)Resources.Load("Sprites/s3", typeof(Sprite));
        _gradients.Add(new myGradient(3, 4, tmps));
        tmps = (Sprite)Resources.Load("Sprites/s4", typeof(Sprite));
        _gradients.Add(new myGradient(4, 5, tmps));
        tmps = (Sprite)Resources.Load("Sprites/s5", typeof(Sprite));
        _gradients.Add(new myGradient(5, 1, tmps));

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            checkCorrect();
        }
    }

    private void checkCorrect()
    {
        int p = _gradients[_currentColor].left;
        if (predecessor != null)
            predecessor.getColor(true);
        int a = _gradients[_currentColor].right;
        if (ancestor != null)
            ancestor.getColor(false);

        if (p == _gradients[_currentColor].left && a == _gradients[_currentColor].right)
            correct = true;
        correct = false;
        Debug.Log(correct);
    }

    void FixedUpdate()
    {
        _transform.position += new Vector3(-_speed * Time.deltaTime, 0.0f);
        if (correct)
        {
            _transform.localScale = new Vector2(1.0f, 1.0f);
        }
        else
        {
            _transform.localScale = new Vector2(0.5f, 0.5f);
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Click");

        int rand = UnityEngine.Random.Range(0, 5);

        if (rand != _currentColor)
        {
            _spriteRenderer.sprite = _gradients[_currentColor].sprite;
            _currentColor = rand;
        }
    }

    int getColor(bool direction)
    {
        // false = lavy, true = pravy
        return !direction ? _gradients[_currentColor].left : _gradients[_currentColor].right;
    }
}
