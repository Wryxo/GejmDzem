using UnityEngine;
using System.Collections;

public class SquareScript : MonoBehaviour {

    private SpriteRenderer _spriteRenderer;
    private Color[] _colors = new Color[] { Color.red, Color.blue, Color.green, Color.cyan, Color.magenta };
    private int _currentColor = 0;
    private Transform _transform;
    private float _speed = 1.0f;
    public int Id = -1;


    // Use this for initialization
    void Start () {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _transform = GetComponent<Transform>();
        _spriteRenderer.color = _colors[0];
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
            _spriteRenderer.color = _colors[_currentColor];
            _currentColor = rand;
        }
    }
}
