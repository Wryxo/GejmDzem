using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterScript : MonoBehaviour {

    List<GameObject> queue=new List<GameObject>();
    public bool active = false;
    public int numCubes = 50;
    public float cubeSize;
    public float speed = 1.0f;
    public float horzExtent;
    public bool pause = false;

    private int _cubeCount;
    private float _leftBound;

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
        GameObject cube = (GameObject) Instantiate(Resources.Load("Prefabs/Square", typeof (GameObject)));
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
        GameObject cube = (GameObject) Instantiate(Resources.Load("Prefabs/Square", typeof (GameObject)));
        SquareScript ssc = cube.GetComponent<SquareScript>();
        cube.GetComponent<Transform>().localScale = new Vector3(cubeSize, cubeSize, 0);
        cube.GetComponent<Transform>().position = new Vector3(prevPos.x + (2*ssc.childLeft.bounds.size.x), 0, 0);
        SquareScript ssq = previousCube.GetComponent<SquareScript>();
        ssc.predecessor = ssq;
        ssq.ancestor = ssc;
        queue.Add(cube);
    }

}
