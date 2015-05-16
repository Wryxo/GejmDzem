using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterScript : MonoBehaviour {

    List<GameObject> queue=new List<GameObject>();
    public bool active = false;
    public int numCubes = 30;
    public float cubeSize;
    public float speed = 0.0f;

    public float horzExtent;

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
	        Vector3 pos = queue[queue.Count-1].GetComponent<Transform>().position;
            if (pos.x < _leftBound)
            {
                Destroy(queue[queue.Count-1]);
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
        cube.GetComponent<Transform>().position=new Vector3(_leftBound,0,0);
        cube.GetComponent<Transform>().localScale = new Vector3(cubeSize, cubeSize, 0);
        queue.Add(cube);
        for (int i = 0; i < _cubeCount; i++)
        {
            addNextCube();
        }
    }

    void addNextCube()
    {
        GameObject previousCube = queue[queue.Count - 1];
        Vector3 prevPos = previousCube.GetComponent<Transform>().position;
        GameObject cube = (GameObject) Instantiate(Resources.Load("Prefabs/Square", typeof (GameObject)));
        cube.GetComponent<Transform>().position=new Vector3(prevPos.x+cubeSize,0,0);
        cube.GetComponent<Transform>().localScale = new Vector3(cubeSize, cubeSize, 0);
        queue.Add(cube);
    }

}
