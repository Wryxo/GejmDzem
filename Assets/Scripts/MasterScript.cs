using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterScript : MonoBehaviour {

    public List<GameObject> queue=new List<GameObject>();
    public bool active = false;
    public int numCubes = 50;
    public float cubeSize;
    public float horzExtent;
    public bool pause = false;
    public float speed;
    public Color[] colors = new Color[] { Color.red, Color.green, Color.blue, Color.magenta, Color.yellow, Color.cyan };
    public int diffRange = 3;
    public int diffCount = 10;

    private int _cubeCount;
    private float _leftBound;
    private int _sanityLeft;
    private int _sanityRight;


    // Use this for initialization
    void Start () {
        setupCubes();
        speed = 1.5f;
        diffCount = 10;
        diffRange = 3;
	    //pregen. enough cubes
	}
	
	// Update is called once per frame
	void Update () {
	    if (active)
	    {
	        Vector3 pos = queue[0].GetComponent<Transform>().position;
            if (pos.x < _leftBound)
            {
                Destroy(queue[0]);
                queue.RemoveAt(0);
                addNextCube();
            }
	    }
	}

    void setupCubes()
    {
        horzExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
        cubeSize=horzExtent*2.0f/numCubes;
        queue.Clear();
        _cubeCount = numCubes + 4;
        _leftBound = -horzExtent-2;
        int[] left = new int[diffCount];
        int[] right = new int[diffCount];
        for (int i = 0; i < diffCount; i++)
        {
            int l = Random.Range(0, diffRange);
            int r = Random.Range(0, diffRange);
            if (Random.value > 0.5)
            {
                _sanityLeft = l;
            }
            if (Random.value < 0.5)
            {
                _sanityLeft = r;
            }
            left[i] = l;
            right[i] = r;
        }
        GameObject cube = (GameObject) Instantiate(Resources.Load("Prefabs/Square", typeof (GameObject)));
        SquareScript ssc = cube.GetComponent<SquareScript>();
        ssc.colorsLeft = new int[diffCount];
        ssc.colorsRight = new int[diffCount];
        left.CopyTo(ssc.colorsLeft, 0);
        right.CopyTo(ssc.colorsRight, 0);
        cube.GetComponent<Transform>().position=new Vector3(1,0,0);
        cube.GetComponent<Transform>().localScale = new Vector3(cubeSize, cubeSize, 0);
        queue.Add(cube);
        for (int i = 0; i < _cubeCount / 2; i++)
        {
            addNextCube();
        }
    }

    void addNextCube()
    {
        GameObject previousCube = queue[queue.Count - 1];
        Vector3 prevPos = previousCube.GetComponent<Transform>().position;
        int[] left = new int[diffCount];
        int[] right = new int[diffCount];
        for (int i = 0; i < diffCount; i++)
        {
            int l = Random.Range(0, diffRange);
            int r = Random.Range(0, diffRange);
            if (Random.value > 0.5)
            {
                _sanityLeft = l;
            }
            if (Random.value < 0.5)
            {
                _sanityLeft = r;
            }
            left[i] = l;
            right[i] = r;
        }
        GameObject cube = (GameObject) Instantiate(Resources.Load("Prefabs/Square", typeof (GameObject)));
        SquareScript ssc = cube.GetComponent<SquareScript>();
        ssc.colorsLeft = new int[diffCount];
        ssc.colorsRight = new int[diffCount];
        left.CopyTo(ssc.colorsLeft, 0);
        right.CopyTo(ssc.colorsRight, 0);
        cube.GetComponent<Transform>().localScale = new Vector3(cubeSize, cubeSize, 0);
        cube.GetComponent<Transform>().position = new Vector3(prevPos.x + (2*ssc.childLeft.bounds.size.x), 0, 0);
        SquareScript ssq = previousCube.GetComponent<SquareScript>();
        ssc.predecessor = ssq;
        ssq.ancestor = ssc;
        queue.Add(cube);
    }

}
