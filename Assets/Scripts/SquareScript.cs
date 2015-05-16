using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    }

    void FixedUpdate()
    {
        _transform.position += new Vector3(-_speed * Time.deltaTime, 0.0f);
    }

    void OnMouseDown()
    {
        Debug.Log("Click");

        int rand = Random.Range(0, 5);

        if (rand != _currentColor)
        {
            _spriteRenderer.sprite = _gradients[_currentColor].sprite;
            _currentColor = rand;
        }
    }
}
