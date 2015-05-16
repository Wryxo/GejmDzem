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
    public float speed = 1.0f;
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
        cube.GetComponent<Transform>().position = new Vector3(prevPos.x + (2*ssc.childLeft.bounds.size.x), 0, 0);
        SquareScript ssq = previousCube.GetComponent<SquareScript>();
        ssc.predecessor = ssq;
        ssq.ancestor = ssc;
        queue.Add(cube);
    }

}
