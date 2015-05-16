using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MasterScript : MonoBehaviour {

    public List<GameObject> queue=new List<GameObject>();
    public bool active = false;
    public int numCubes = 50;
    public float cubeSize;
    public float horzExtent;
    public bool pause = false;
    public float speed = 1.0f;
    public Color[] colors = new Color[] { Color.red, Color.green, Color.blue, Color.magenta, Color.yellow, Color.cyan };
    public int diffRange = 6;
    public int diffCount = 20;

    private int _cubeCount;
    private float _leftBound;
    private int _sanityLeft;
    private int _sanityRight;

    public float GetHorzExtent()
    {
        return Camera.main.orthographicSize * Screen.width / Screen.height;
    }

    private Transform _playerTransform;

    //powerup stuff
    private float powerCd = 0.0f;
    private float retardCd = 0.0f;
    private float chaosCd = 0.0f;
    private float hpCd = 0.0f;
    private float speedCd = 0.0f;
    private float slowCd = 0.0f;
    private float poopCd = 0.0f;

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
    private Text _diffText;
    private int _score;
    private List<int> _combo = new List<int>();
    private float _diffInc = 0.0f;

    // Use this for initialization
    void Start ()
    {
        powerCd = Random.Range(10.0f,15.0f);
        _playerTransform = (GameObject.FindGameObjectWithTag("Retard")).GetComponent<Transform>();
        _scoreText = (GameObject.FindGameObjectWithTag("Score")).GetComponent<Text>();
        _diffText = (GameObject.FindGameObjectWithTag("Difficulty")).GetComponent<Text>();

        setupCubes();
        _diffText.text = diffRange.ToString();
        //pregen. enough cubes
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (!pause)
        {
            _diffInc += Time.deltaTime;
            if (_diffInc > 10.0f)
            {
                _diffInc = 0.0f;
                diffRange++;
                if (diffRange > colors.Length)
                    diffRange = colors.Length;
                _diffText.text = diffRange.ToString();
            }
        }
    }

    void Update()
    {
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
        ssc = cube.GetComponent<SquareScript>();
        ssc.colorsLeft = new int[size];
        ssc.colorsRight = new int[size];
        left.CopyTo(ssc.colorsLeft, 0);
        right.CopyTo(ssc.colorsRight, 0);
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

    public void Shoot() {
        GameObject zivot = (GameObject)Instantiate(Resources.Load("Prefabs/zivot", typeof(GameObject)));
        Transform trans = zivot.GetComponent<Transform>();
        trans.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y + 0.6f);
        Rigidbody2D test = zivot.GetComponent<Rigidbody2D>();
        test.AddForce(new Vector2(-4.0f, 8.0f), ForceMode2D.Impulse);
        _score += evaluateCombo();
        _scoreText.text = _score.ToString();
    }

    private int evaluateCombo()
    {
        return 1;
    }

    public void addCombo(int left, int right)
    {
        if (_combo.Count > 8) { 
            _combo.RemoveAt(0);
            _combo.RemoveAt(1);
        }
        _combo.Add(left);
        _combo.Add(right);
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
        if (powerCd <= 0.0f)
        {
            Instantiate(Resources.Load("Prefabs/GenericPowerup", typeof(GameObject)));
            powerCd = Random.Range(15.0f, 20.0f);
        }
        if ((retardCheck) && (retardCd <= 0.0f))
        {
            retardCheck = false;
            retardCd = 0.0f;
            diffRange = origRange;
            diffCount = origCount;
        }
        if ((chaosCheck) && (chaosCd <= 0.0f))
        {
            chaosCheck = false;
            chaosCd = 0.0f;
            diffRange = origRange;
            diffCount = origCount;
        }
        if ((hpCheck) && (hpCd <= 0.0f))
        {
            hpCheck = false;
            hpCd = 0.0f;
            healingSpeed = 0.0f;
        }
        if ((speedCheck) && (speedCd <= 0.0f))
        {
            speedCheck = false;
            speedCd = 0.0f;
            speed = origSpeed;
        }
        if ((slowCheck) && (slowCd <= 0.0f))
        {
            slowCheck = false;
            slowCd = 0.0f;
            speed = origSpeed;
        }
        if ((poopCheck) && (poopCd <= 0.0f))
        {
            poopCheck = false;
            poopCd = 0.0f;
            //todo
        }
    }

    //block of powers

    public void powerSpeed(float t)
    {
        origSpeed = speed;
        speed = 4.0f;
        speedCd = t;
        speedCheck = true;
    }

    public void powerSlow(float t)
    {
        origSpeed = speed;
        speed = 0.8f;
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
