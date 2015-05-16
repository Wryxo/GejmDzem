using UnityEngine;
using System.Collections;

public class PowerupScript : MonoBehaviour
{
    private Transform _transform;
    private MasterScript _masterScript;

    public bool isEvil;

    private int _platform_id;

	// Use this for initialization
	void Start () {
        _transform = GetComponent<Transform>();
        _masterScript = (GameObject.FindGameObjectWithTag("GameController")).GetComponent<MasterScript>();
        //nastavit initial platform
	}
	
	// Update is called once per frame
	void Update () {
	    updateCurrentPlatform();
	}

    void FixedUpdate()
    {
        if (isEvil)
        {
            //mumbojumbo
            
        }
        else
        {
            _transform.position += new Vector3(-_masterScript.speed * Time.deltaTime, 0.0f, 0.0f);
            //float up-down
        }
    }

    void updateCurrentPlatform()
    {
        //if null pointers its from this
        int i = _platform_id+1;
        Transform nextTrans=_masterScript.queue[i].GetComponent<Transform>();
        Transform currentTrans =_masterScript.queue[_platform_id].GetComponent<Transform>();
        while (Mathf.Abs(nextTrans.position.x - transform.position.x) < Mathf.Abs(currentTrans.position.x - transform.position.x))
        {
            //if null pointers its from this
            _platform_id = i;
            i++;
        }
    }
}
