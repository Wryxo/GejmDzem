using System;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MasterScript : MonoBehaviour {

    public List<GameObject> queue=new List<GameObject>();
    public bool active = false;
    public int numCubes = 50;
    public float cubeSize;
    public float horzExtent;
    public bool pause = false;
    public float speed = 1.0f;
    public Color[] colors = new Color[] { new Color(248,252,106), new Color(174,226,57), new Color(78,205,196), new Color(255,107,107), Color.yellow, Color.cyan };
    public int diffRange = 6;
    public int diffCount = 20;
    public int combo3Reward = 15;
    public int combo4Reward = 50;
    public int combo5Reward = 150;
    public int score;
    public float _leftBound;
    private int _lastScore;

    private int _cubeCount;
    private int _sanityLeft;
    private int _sanityRight;

    public float GetHorzExtent()
    {
        return Camera.main.orthographicSize * Screen.width / Screen.height;
    }

    private Transform _playerTransform;

    //powerup stuff
    private float powerCd = 0.0f;
    private float tierCd = 0.0f;
    private float retardCd = 0.0f;
    private float chaosCd = 0.0f;
    private float hpCd = 0.0f;
    private float speedCd = 0.0f;
    private float slowCd = 0.0f;
    private float poopCd = 0.0f;

    private float countCd = 0.0f;
    private float rangeCd = 0.0f;

    private bool retardCheck = false;
    private bool chaosCheck = false;
    private bool hpCheck = false;
    private bool speedCheck = false;
    private bool slowCheck = false;
    private bool poopCheck = false;

    private int origRange;
    private int origCount;
    private float origSpeed;
    public float healingSpeed=0.0f;

    private Text _scoreText;
    private List<int> _combo = new List<int>();

    private bool _combo3Active = false;
    private bool _combo4Active = false;
    private bool _combo5Active = false;

    private int[] _combo3;
    private int[] _combo4;
    private int[] _combo5;

    private Image[] _combo3Img;
    private Image[] _combo4Img;
    private Image[] _combo5Img;

    bool combo3Done = false;
    bool combo4Done = false;
    bool combo5Done = false;
    private Sprite[] _animalsSprites;


    // Use this for initialization
    void Start ()
    {
        powerCd = Random.Range(10.0f,15.0f);
        tierCd = 30.0f;
        _playerTransform = (GameObject.FindGameObjectWithTag("Retard")).GetComponent<Transform>();
        GameObject _scoreTmp = (GameObject.FindGameObjectWithTag("Score"));
        if (_scoreTmp != null)
            _scoreText = _scoreTmp.GetComponent<Text>();

        _animalsSprites = Resources.LoadAll<Sprite>("Sprites/animals");

        GameObject[] tmp = (GameObject.FindGameObjectsWithTag("3combo"));
        _combo3Img = new Image[tmp.Length];
        for (int i = 0; i < tmp.Length; i++)
        {
            tmp[i].GetComponent<Transform>().position = new Vector3(70 + i * 40, tmp[i].GetComponent<Transform>().position.y);
            _combo3Img[i] = tmp[i].GetComponent<Image>();
            _combo3Img[i].color = new Color(_combo3Img[i].color.r, _combo3Img[i].color.g, _combo3Img[i].color.b, 0.0f);
        }

        tmp = (GameObject.FindGameObjectsWithTag("4combo"));
        _combo4Img = new Image[tmp.Length];
        for (int i = 0; i < tmp.Length; i++)
        {
            tmp[i].GetComponent<Transform>().position = new Vector3(70 + i * 40, tmp[i].GetComponent<Transform>().position.y);
            _combo4Img[i] = tmp[i].GetComponent<Image>();
            _combo4Img[i].color = new Color(_combo4Img[i].color.r, _combo4Img[i].color.g, _combo4Img[i].color.b, 0.0f);
        }

        tmp = (GameObject.FindGameObjectsWithTag("5combo"));
        _combo5Img = new Image[tmp.Length];
        for (int i = 0; i < tmp.Length; i++)
        {
            tmp[i].GetComponent<Transform>().position = new Vector3(70 + i * 40, tmp[i].GetComponent<Transform>().position.y);
            _combo5Img[i] = tmp[i].GetComponent<Image>();
            _combo5Img[i].color = new Color(_combo5Img[i].color.r, _combo5Img[i].color.g, _combo5Img[i].color.b, 0.0f);
        }

        for (int i = 0; i < 5; i++)
        {
            _combo.Add(-1);
        }

        if (!pause)
            setupCubes();
        //pregen. enough cubes
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (!pause)
        {
            speed += 0.01f*Time.deltaTime;
            checkCombos();
            _scoreText.text = score.ToString();
        } else
        {
            if(Input.GetMouseButtonUp(0))
            {
                Application.LoadLevel("GameScene");
            }
        }
    }

    void Update()
    {
        if (!pause)
            testCd();
    }

    void setupCubes()
    {
        horzExtent = GetHorzExtent();
        cubeSize=horzExtent*2.0f/numCubes;
        queue.Clear();
        _cubeCount = numCubes + 4;
        _leftBound = -horzExtent-2;
        int size = 1;
        int[] left = new int[size];
        int[] right = new int[size];
        for (int i = 0; i < size; i++)
        {
            int l = Random.Range(0, diffRange);
            int r = Random.Range(0, diffRange);
            left[i] = l;
            right[i] = r;
        }
        _sanityLeft = left[Random.Range(0, size)];
        _sanityRight = right[Random.Range(0, size)];
        GameObject cube = (GameObject) Instantiate(Resources.Load("Prefabs/Square", typeof (GameObject)));
        SquareScript ssc = cube.GetComponent<SquareScript>();
        ssc.colorsLeft = new int[size];
        ssc.colorsRight = new int[size];
        left.CopyTo(ssc.colorsLeft, 0);
        right.CopyTo(ssc.colorsRight, 0);
        cube.GetComponent<Transform>().position=new Vector3(1.5f,0,0);
        cube.GetComponent<Transform>().localScale = new Vector3(cubeSize, cubeSize, 0);
        queue.Add(cube);
        cube = (GameObject)Instantiate(Resources.Load("Prefabs/Square", typeof(GameObject)));
        SquareScript ssc2 = cube.GetComponent<SquareScript>();
        ssc2.predecessor = ssc;
        ssc2.colorsLeft = new int[size];
        ssc2.colorsRight = new int[size];
        right.CopyTo(ssc2.colorsLeft, 0);
        left.CopyTo(ssc2.colorsRight, 0);
        cube.GetComponent<Transform>().position = new Vector3(1.5f + (2 * ssc.childLeft.bounds.size.x), 0, 0);
        cube.GetComponent<Transform>().localScale = new Vector3(cubeSize, cubeSize, 0);
        queue.Add(cube);
        for (int i = 0; i < _cubeCount / 2; i++)
        {
            addNextCube();
        }
    }

    public void addNextCube()
    {
        GameObject previousCube = queue[queue.Count - 1];
        Vector3 prevPos = previousCube.GetComponent<Transform>().position;
        int size = Random.Range(3, diffCount);
        int[] left = new int[size];
        int[] right = new int[size];
        left[0] = Random.Range(0, diffRange);
        right[0] = Random.Range(0, diffRange);
        for (int i = 1; i < size; i++)
        {
            int l = Random.Range(0, diffRange);
            if (i == size -1) { 
                while (l == left[0])
                    l = Random.Range(0, diffRange);
            }
            while (l == left[i - 1])
                l = Random.Range(0, diffRange);
            int r = Random.Range(0, diffRange);
            if (i == size - 1)
            {
                while (r == right[0])
                    r = Random.Range(0, diffRange);
            }
            while (r == right[i - 1])
                r = Random.Range(0, diffRange);

            left[i] = l;
            right[i] = r;
        }
        left[Random.Range(0, size)] = _sanityLeft;
        right[Random.Range(0, size)] = _sanityRight;
        _sanityLeft = left[Random.Range(0, size)];
        _sanityRight = right[Random.Range(0, size)];

        GameObject cube = (GameObject) Instantiate(Resources.Load("Prefabs/Square", typeof (GameObject)));
        SquareScript ssc = cube.GetComponent<SquareScript>();
        ssc.colorsLeft = new int[size];
        ssc.colorsRight = new int[size];
        left.CopyTo(ssc.colorsLeft, 0);
        right.CopyTo(ssc.colorsRight, 0);
        cube.GetComponent<Transform>().localScale = new Vector3(cubeSize, cubeSize, 0);
        cube.GetComponent<Transform>().position = new Vector3(prevPos.x + (2*ssc.childLeft.bounds.size.x), 0);
        SquareScript ssq = previousCube.GetComponent<SquareScript>();
        ssc.predecessor = ssq;
        ssq.ancestor = ssc;
        queue.Add(cube);
    }

    public void Shoot(int i) {
        GameObject zivot = (GameObject)Instantiate(Resources.Load("Prefabs/zivot", typeof(GameObject)));
        zivot.GetComponent<SpriteRenderer>().sprite = _animalsSprites[i];
        Transform trans = zivot.GetComponent<Transform>();
        trans.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y + 0.6f);
        Rigidbody2D test = zivot.GetComponent<Rigidbody2D>();
        test.AddForce(new Vector2(-4.0f, 8.0f), ForceMode2D.Impulse);
    }

    public void addCombo(int left, int right)
    {
        _combo.RemoveAt(0);
        //_combo.RemoveAt(1);
        //_combo.Add(left);
        _combo.Add(right);
    }

    public void setCombo3()
    {
        _combo3 = new int[3];
        int rand = Random.Range(0, diffRange);
        _combo3[0] = rand;
        rand = Random.Range(0, diffRange);
        _combo3[1] = rand;
        rand = Random.Range(0, diffRange);
        _combo3[2] = rand;

        Color tmpc = colors[_combo3[0]];
        _combo3Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
        tmpc = colors[_combo3[1]];
        _combo3Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
        tmpc = colors[_combo3[2]];
        _combo3Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
        _combo3Active = true;
    }

    public void setCombo4()
    {
        _combo4 = new int[4];
        int rand = Random.Range(0, diffRange);
        _combo4[0] = rand;
        rand = Random.Range(0, diffRange);
        _combo4[1] = rand;
        rand = Random.Range(0, diffRange);
        _combo4[2] = rand;
        rand = Random.Range(0, diffRange);
        _combo4[3] = rand;

        Color tmpc = colors[_combo4[0]];
        _combo4Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
        tmpc = colors[_combo4[1]];
        _combo4Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
        tmpc = colors[_combo4[2]];
        _combo4Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
        tmpc = colors[_combo4[3]];
        _combo4Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
        _combo4Active = true;
    }

    public void setCombo5()
    {
        _combo5 = new int[5];
        int rand = Random.Range(0, diffRange);
        _combo5[0] = rand;
        rand = Random.Range(0, diffRange);
        _combo5[1] = rand;
        rand = Random.Range(0, diffRange);
        _combo5[2] = rand;
        rand = Random.Range(0, diffRange);
        _combo5[3] = rand;
        rand = Random.Range(0, diffRange);
        _combo5[4] = rand;

        Color tmpc = colors[_combo5[0]];
        _combo5Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
        tmpc = colors[_combo5[1]];
        _combo5Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
        tmpc = colors[_combo5[2]];
        _combo5Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
        tmpc = colors[_combo5[3]];
        _combo5Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
        tmpc = colors[_combo5[3]];
        _combo5Img[4].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
        _combo5Active = true;
    }

    private void checkCombos()
    {
        /*StringBuilder sb = new StringBuilder();
        for (int i = 4; i >= 0; i--)
        {
            sb.Append(_combo[i] + " ");
        }
        Debug.Log("current: " + sb);
        sb = new StringBuilder();
        for (int i = 2; i >= 0; i--)
        {
            sb.Append(_combo3[i] + " ");
        }
        Debug.Log("combo: " + sb);*/
        int possibru = 0;
        Color tmpc;
        if (_combo3Active)
        {
            if (_combo[4] == _combo3[0])
            {
                tmpc = _combo3Img[0].color;
                _combo3Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                possibru = possibru | 1;
            }
            else
            {
                if (combo3Done)
                {
                    combo3Done = false;
                    _combo[4] = -1;
                    _combo[3] = -1;
                    _combo[2] = -1;
                    _combo[1] = -1;
                    _combo[0] = -1;
                    score += combo3Reward;
                    _scoreText.text = score.ToString();
                    Shoot(0);
                    tmpc = _combo3Img[0].color;
                    _combo3Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo3Img[1].color;
                    _combo3Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo3Img[2].color;
                    _combo3Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                }

                tmpc = _combo3Img[0].color;
                _combo3Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                tmpc = _combo3Img[1].color;
                _combo3Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                tmpc = _combo3Img[2].color;
                _combo3Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
            }
            if (_combo[4] == _combo3[1] && _combo[3] == _combo3[0])
            {
                tmpc = _combo3Img[0].color;
                _combo3Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                tmpc = _combo3Img[1].color;
                _combo3Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                possibru = possibru | 1;
            }
            else
            {
                if (combo3Done)
                {
                    combo3Done = false;
                    _combo[4] = -1;
                    _combo[3] = -1;
                    _combo[2] = -1;
                    _combo[1] = -1;
                    _combo[0] = -1;
                    score += combo3Reward;
                    _scoreText.text = score.ToString();
                    Shoot(0);
                    tmpc = _combo3Img[0].color;
                    _combo3Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo3Img[1].color;
                    _combo3Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo3Img[2].color;
                    _combo3Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                }
                tmpc = _combo3Img[1].color;
                _combo3Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                tmpc = _combo3Img[2].color;
                _combo3Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
            }
            if (_combo[4] == _combo3[2] && _combo[3] == _combo3[1] && _combo[2] == _combo3[0])
            {
                tmpc = _combo3Img[0].color;
                _combo3Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                tmpc = _combo3Img[1].color;
                _combo3Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                tmpc = _combo3Img[2].color;
                _combo3Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                combo3Done = true;
                possibru = possibru | 1;
            }
            else
            {
                if (combo3Done)
                {
                    combo3Done = false;
                    _combo[4] = -1;
                    _combo[3] = -1;
                    _combo[2] = -1;
                    _combo[1] = -1;
                    _combo[0] = -1;
                    score += combo3Reward;
                    _scoreText.text = score.ToString();
                    Shoot(0);
                    tmpc = _combo3Img[0].color;
                    _combo3Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo3Img[1].color;
                    _combo3Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo3Img[2].color;
                    _combo3Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                }
                tmpc = _combo3Img[2].color;
                _combo3Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
            }
        }
        if (_combo4Active)
        {
            if (_combo[4] == _combo4[0])
            {
                tmpc = _combo4Img[0].color;
                _combo4Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                possibru = possibru | 2;
            }
            else
            {
                if (combo4Done)
                {
                    combo4Done = false;
                    _combo[4] = -1;
                    _combo[3] = -1;
                    _combo[2] = -1;
                    _combo[1] = -1;
                    _combo[0] = -1;
                    score += combo4Reward;
                    _scoreText.text = score.ToString();
                    Shoot(0);
                    tmpc = _combo4Img[0].color;
                    _combo4Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo4Img[1].color;
                    _combo4Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo4Img[2].color;
                    _combo4Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo4Img[3].color;
                    _combo4Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                }
                tmpc = _combo4Img[0].color;
                _combo4Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                tmpc = _combo4Img[1].color;
                _combo4Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                tmpc = _combo4Img[2].color;
                _combo4Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                tmpc = _combo4Img[3].color;
                _combo4Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
            }
            if (_combo[4] == _combo4[1] && _combo[3] == _combo4[0])
            {
                tmpc = _combo4Img[0].color;
                _combo4Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                tmpc = _combo4Img[1].color;
                _combo4Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                possibru = possibru | 2;
            }
            else
            {
                tmpc = _combo4Img[1].color;
                _combo4Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                tmpc = _combo4Img[2].color;
                _combo4Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                tmpc = _combo4Img[3].color;
                _combo4Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                if (combo4Done)
                {
                    combo4Done = false;
                    _combo[4] = -1;
                    _combo[3] = -1;
                    _combo[2] = -1;
                    _combo[1] = -1;
                    _combo[0] = -1;
                    score += combo4Reward;
                    _scoreText.text = score.ToString();
                    Shoot(1);
                    tmpc = _combo4Img[0].color;
                    _combo4Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo4Img[1].color;
                    _combo4Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo4Img[2].color;
                    _combo4Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo4Img[3].color;
                    _combo4Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                }
            }
            if (_combo[4] == _combo4[2] && _combo[3] == _combo4[1] && _combo[2] == _combo4[0])
            {
                tmpc = _combo4Img[0].color;
                _combo4Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                tmpc = _combo4Img[1].color;
                _combo4Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                tmpc = _combo4Img[2].color;
                _combo4Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                possibru = possibru | 2;
            }
            else
            {
                tmpc = _combo4Img[2].color;
                _combo4Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                tmpc = _combo4Img[3].color;
                _combo4Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                if (combo4Done)
                {
                    combo4Done = false;
                    _combo[4] = -1;
                    _combo[3] = -1;
                    _combo[2] = -1;
                    _combo[1] = -1;
                    _combo[0] = -1;
                    score += combo4Reward;
                    _scoreText.text = score.ToString();
                    Shoot(1);
                    tmpc = _combo4Img[0].color;
                    _combo4Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo4Img[1].color;
                    _combo4Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo4Img[2].color;
                    _combo4Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo4Img[3].color;
                    _combo4Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                }
            }
            if (_combo[4] == _combo4[3] && _combo[3] == _combo4[2] && _combo[2] == _combo4[1] && _combo[1] == _combo4[0])
            {
                tmpc = _combo4Img[0].color;
                _combo4Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                tmpc = _combo4Img[1].color;
                _combo4Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                tmpc = _combo4Img[2].color;
                _combo4Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                tmpc = _combo4Img[3].color;
                _combo4Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                combo4Done = true;
                possibru = possibru | 2;
            }
            else
            {
                if (combo4Done)
                {
                    combo4Done = false;
                    _combo[4] = -1;
                    _combo[3] = -1;
                    _combo[2] = -1;
                    _combo[1] = -1;
                    _combo[0] = -1;
                    score += combo4Reward;
                    _scoreText.text = score.ToString();
                    Shoot(1);
                    tmpc = _combo4Img[0].color;
                    _combo4Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo4Img[1].color;
                    _combo4Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo4Img[2].color;
                    _combo4Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo4Img[3].color;
                    _combo4Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                }
                tmpc = _combo4Img[3].color;
                _combo4Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
            }
        }
        if (_combo5Active)
        {
            if (_combo[4] == _combo5[0])
            {
                tmpc = _combo5Img[0].color;
                _combo5Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                possibru = possibru | 4;
            }
            else
            {
                if (combo5Done)
                {
                    combo5Done = false;
                    _combo[4] = -1;
                    _combo[3] = -1;
                    _combo[2] = -1;
                    _combo[1] = -1;
                    _combo[0] = -1;
                    score += combo5Reward;
                    _scoreText.text = score.ToString();
                    Shoot(2);
                    tmpc = _combo5Img[0].color;
                    _combo5Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[1].color;
                    _combo5Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[2].color;
                    _combo5Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[3].color;
                    _combo5Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[4].color;
                    _combo5Img[4].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                }

                tmpc = _combo5Img[0].color;
                _combo5Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                tmpc = _combo5Img[1].color;
                _combo5Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                tmpc = _combo5Img[2].color;
                _combo5Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                tmpc = _combo5Img[3].color;
                _combo5Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                tmpc = _combo5Img[4].color;
                _combo5Img[4].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
            }
            if (_combo[4] == _combo5[1] && _combo[3] == _combo5[0])
            {
                tmpc = _combo5Img[0].color;
                _combo5Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                tmpc = _combo5Img[1].color;
                _combo5Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                possibru = possibru | 4;
            }
            else
            {

                if (combo5Done)
                {
                    combo5Done = false;
                    _combo[4] = -1;
                    _combo[3] = -1;
                    _combo[2] = -1;
                    _combo[1] = -1;
                    _combo[0] = -1;
                    score += combo5Reward;
                    _scoreText.text = score.ToString();
                    Shoot(2);
                    tmpc = _combo5Img[0].color;
                    _combo5Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[1].color;
                    _combo5Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[2].color;
                    _combo5Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[3].color;
                    _combo5Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[4].color;
                    _combo5Img[4].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                }
                tmpc = _combo5Img[1].color;
                _combo5Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                tmpc = _combo5Img[2].color;
                _combo5Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                tmpc = _combo5Img[3].color;
                _combo5Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                tmpc = _combo5Img[4].color;
                _combo5Img[4].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
            }
            if (_combo[4] == _combo5[2] && _combo[3] == _combo5[1] && _combo[2] == _combo5[0])
            {
                tmpc = _combo5Img[0].color;
                _combo5Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                tmpc = _combo5Img[1].color;
                _combo5Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                tmpc = _combo5Img[2].color;
                _combo5Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                possibru = possibru | 4;
            }
            else
            {
                if (combo5Done)
                {
                    combo5Done = false;
                    _combo[4] = -1;
                    _combo[3] = -1;
                    _combo[2] = -1;
                    _combo[1] = -1;
                    _combo[0] = -1;
                    score += combo5Reward;
                    _scoreText.text = score.ToString();
                    Shoot(2);
                    tmpc = _combo5Img[0].color;
                    _combo5Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[1].color;
                    _combo5Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[2].color;
                    _combo5Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[3].color;
                    _combo5Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[4].color;
                    _combo5Img[4].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                }
                tmpc = _combo5Img[2].color;
                _combo5Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                tmpc = _combo5Img[3].color;
                _combo5Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                tmpc = _combo5Img[4].color;
                _combo5Img[4].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
            }
            if (_combo[4] == _combo5[3] && _combo[3] == _combo5[2] && _combo[2] == _combo5[1] && _combo[1] == _combo5[0])
            {
                tmpc = _combo5Img[0].color;
                _combo5Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                tmpc = _combo5Img[1].color;
                _combo5Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                tmpc = _combo5Img[2].color;
                _combo5Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                tmpc = _combo5Img[3].color;
                _combo5Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                possibru = possibru | 4;
            }
            else
            {
                if (combo5Done)
                {
                    combo5Done = false;
                    _combo[4] = -1;
                    _combo[3] = -1;
                    _combo[2] = -1;
                    _combo[1] = -1;
                    _combo[0] = -1;
                    score += combo5Reward;
                    _scoreText.text = score.ToString();
                    Shoot(2);
                    tmpc = _combo5Img[0].color;
                    _combo5Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[1].color;
                    _combo5Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[2].color;
                    _combo5Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[3].color;
                    _combo5Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[4].color;
                    _combo5Img[4].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                }
                tmpc = _combo5Img[3].color;
                _combo5Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                tmpc = _combo5Img[4].color;
                _combo5Img[4].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
            }
            if (_combo[4] == _combo5[4] && _combo[3] == _combo5[3] && _combo[2] == _combo5[2] && _combo[1] == _combo5[1] && _combo[0] == _combo5[0])
            {
                tmpc = _combo5Img[0].color;
                _combo5Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                tmpc = _combo5Img[1].color;
                _combo5Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                tmpc = _combo5Img[2].color;
                _combo5Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                tmpc = _combo5Img[3].color;
                _combo5Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                tmpc = _combo5Img[4].color;
                _combo5Img[4].color = new Color(tmpc.r, tmpc.g, tmpc.b, 1.0f);
                combo5Done = true;
                possibru = possibru | 4;
            }
            else
            {
                if (combo5Done)
                {
                    combo5Done = false;
                    _combo[4] = -1;
                    _combo[3] = -1;
                    _combo[2] = -1;
                    _combo[1] = -1;
                    _combo[0] = -1;
                    score += combo5Reward;
                    _scoreText.text = score.ToString();
                    Shoot(2);
                    tmpc = _combo5Img[0].color;
                    _combo5Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[1].color;
                    _combo5Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[2].color;
                    _combo5Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[3].color;
                    _combo5Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                    tmpc = _combo5Img[4].color;
                    _combo5Img[4].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
                }
                tmpc = _combo5Img[4].color;
                _combo5Img[4].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
            }
        }
        
        if (possibru == 1 && combo3Done)
        {
            _combo[4] = -1;
            _combo[3] = -1;
            _combo[2] = -1;
            _combo[1] = -1;
            _combo[0] = -1;
            score += combo3Reward;
            Shoot(0);
            tmpc = _combo3Img[0].color;
            _combo3Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
            tmpc = _combo3Img[1].color;
            _combo3Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
            tmpc = _combo3Img[2].color;
            _combo3Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
        }

        if (possibru == 2 && combo4Done)
        {
            _combo[4] = -1;
            _combo[3] = -1;
            _combo[2] = -1;
            _combo[1] = -1;
            _combo[0] = -1;
            score += combo4Reward;
            Shoot(1);
            tmpc = _combo4Img[0].color;
            _combo4Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
            tmpc = _combo4Img[1].color;
            _combo4Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
            tmpc = _combo4Img[2].color;
            _combo4Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
            tmpc = _combo4Img[3].color;
            _combo4Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
        }

        if (possibru == 4 && combo5Done)
        {
            _combo[4] = -1;
            _combo[3] = -1;
            _combo[2] = -1;
            _combo[1] = -1;
            _combo[0] = -1;
            score += combo5Reward;
            Shoot(2);
            tmpc = _combo5Img[0].color;
            _combo5Img[0].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
            tmpc = _combo5Img[1].color;
            _combo5Img[1].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
            tmpc = _combo5Img[2].color;
            _combo5Img[2].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
            tmpc = _combo5Img[3].color;
            _combo5Img[3].color = new Color(tmpc.r, tmpc.g, tmpc.b, 0.2f);
        }
    }

    void testCd()
    {
        if (retardCd > 0.0f)
        {
            retardCd -= Time.deltaTime;
        }
        if (chaosCd > 0.0f)
        {
            chaosCd -= Time.deltaTime;
        }
        if (poopCd > 0.0f)
        {
            poopCd -= Time.deltaTime;
        }
        if (slowCd > 0.0f)
        {
            slowCd -= Time.deltaTime;
        }
        if (speedCd > 0.0f)
        {
            speedCd -= Time.deltaTime;
        }
        if (hpCd > 0.0f)
        {
            hpCd -= Time.deltaTime;
        }
        if (powerCd > 0.0f)
        {
            powerCd -= Time.deltaTime;
        }
        if (tierCd > 0.0f)
        {
            tierCd -= Time.deltaTime;
        }
        if (powerCd <= 0.0f)
        {
            Instantiate(Resources.Load("Prefabs/GenericPowerup", typeof(GameObject)));
            powerCd = Random.Range(15.0f, 20.0f);
        }
        else if ((powerCd > 5.0f) && (tierCd <= 0.0f))
        {
            if (!_combo3Active)
            {
                GameObject gp = (GameObject) Instantiate(Resources.Load("Prefabs/GenericPowerup", typeof (GameObject)));
                gp.GetComponent<PowerupScript>().power = PowerupScript.PowerType.TierOne;
                tierCd = 30.0f;
            }
            else if (!_combo4Active)
            {
                GameObject gp = (GameObject) Instantiate(Resources.Load("Prefabs/GenericPowerup", typeof (GameObject)));
                gp.GetComponent<PowerupScript>().power = PowerupScript.PowerType.TierTwo;
                tierCd = 50.0f;
            }
            else if (!_combo5Active)
            {
                GameObject gp = (GameObject) Instantiate(Resources.Load("Prefabs/GenericPowerup", typeof (GameObject)));
                gp.GetComponent<PowerupScript>().power = PowerupScript.PowerType.TierThree;
                tierCd = 80.0f;
            }
            else
            {
                //don't bother too much
                tierCd = 474747.0f;
            }

        }
        if (countCd > 0.0f)
        {
            countCd -= Time.deltaTime;
        }
        if (rangeCd > 0.0f)
        {
            rangeCd -= Time.deltaTime;
        }

        Text tmp = (GameObject.FindGameObjectWithTag("GG")).GetComponent<Text>();
        if ((retardCheck) && (retardCd <= 0.0f))
        {
            retardCheck = false;
            retardCd = 0.0f;
            diffRange = origRange;
            diffCount = origCount;
            tmp.text = "";
        }
        if ((chaosCheck) && (chaosCd <= 0.0f))
        {
            chaosCheck = false;
            chaosCd = 0.0f;
            diffRange = origRange;
            diffCount = origCount;
            tmp.text = "";
        }
        if ((hpCheck) && (hpCd <= 0.0f))
        {
            hpCheck = false;
            hpCd = 0.0f;
            healingSpeed = 0.0f;
            tmp.text = "";
        }
        if ((speedCheck) && (speedCd <= 0.0f))
        {
            speedCheck = false;
            speedCd = 0.0f;
            speed = origSpeed;
            tmp.text = "";
        }
        if ((slowCheck) && (slowCd <= 0.0f))
        {
            slowCheck = false;
            slowCd = 0.0f;
            speed = origSpeed;
            tmp.text = "";
        }
        if ((poopCheck) && (poopCd <= 0.0f))
        {
            poopCheck = false;
            poopCd = 0.0f;
            //todo
            tmp.text = "";
        }
        if (countCd <= 0.0f)
        {
            diffCount = (diffCount + 1);
            if (diffCount > (diffRange * diffRange))
                diffCount = diffRange * diffRange;
            countCd = 5.0f;
            tmp.text = "";
        }
        if (rangeCd <= 0.0f)
        {
            diffRange++;
            if (diffRange > 6)
                diffRange = 6;
            rangeCd = 20.0f;
            tmp.text = "";
        }
    }

    //block of powers

    public void powerSpeed(float t)
    {
        origSpeed = speed;
        speed = speed * 1.7f;
        speedCd = t;
        speedCheck = true;
    }

    public void powerSlow(float t)
    {
        origSpeed = speed;
        speed = speed * 0.6f;
        slowCd = t;
        slowCheck = true;
    }

    public void powerHp(float t)
    {
        healingSpeed = 1.0f;
        hpCd = t;
        hpCheck = true;
    }

    public void powerPoop(float t)
    {
        //todo shoot poop
    }

    public void powerRetard(float t)
    {
        origRange = diffRange;
        origCount = diffCount;
        diffRange = 3;
        diffCount = 3;
        retardCd = t;
        retardCheck = true;
    }

    public void powerChaos(float t)
    {
        origRange = diffRange;
        origCount = diffCount;
        diffRange = 6;
        diffCount = 6;
        retardCd = t;
        retardCheck = true;
    }
}
