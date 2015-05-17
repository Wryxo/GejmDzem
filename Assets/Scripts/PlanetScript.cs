using UnityEngine;
using System.Collections;

public class PlanetScript : MonoBehaviour {
    public float minSpeed = 0.02f;
    public float maxSpeed = 0.08f;
    public float Scale = 0.3f;
    public Sprite[] _planetSprites;
    private SpriteRenderer _spriteRenderer;
    private float _horzExtent;
    private float _speed = 0.05f;
    // Use this for initialization
    void Start () {
         _planetSprites = Resources.LoadAll<Sprite>("Sprites/planets");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _horzExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
        RandomSprite();
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(-_speed,0,0);
        if (transform.position.x < -_horzExtent)
        {
            var pos = new Vector3(Random.Range(2,5)*_horzExtent, transform.position.y, transform.position.z);
            transform.position = pos;
            RandomSprite();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void RandomSprite()
    {
        _spriteRenderer.sprite = _planetSprites[Random.Range(0, _planetSprites.Length)];        
        _speed = Random.Range(minSpeed, maxSpeed);
        transform.localScale = _speed / maxSpeed * new Vector3(Scale, Scale, Scale);

    }
}
