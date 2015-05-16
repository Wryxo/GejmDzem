using UnityEngine;
using System.Collections;

public class PowerupScript : MonoBehaviour
{
    private Transform _transform;
    private MasterScript _masterScript;

    public bool isEvil=true;

    public int _platform_id;
    public float _platform_vert_dist;

    private float _float_speed=0.5f;
    private float throw_speed = 1.0f;

	// Use this for initialization
	void Start ()
	{
	    isEvil = true;
        _transform = GetComponent<Transform>();
        GameObject master=GameObject.FindGameObjectWithTag("GameController");
        _masterScript = master.GetComponent<MasterScript>();
	    _transform.position = master.GetComponent<Transform>().position + new Vector3(0.0f,2.0f,0.0f);
        Debug.Log(_transform.position);
        //nastavit initial platform
	    _platform_id = 0;
        updateCurrentPlatform();
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
            _float_speed -= 0.5f*Time.deltaTime;
        }
        else
        {
            if ((_transform.position.y > 2.5) || (_transform.position.y < 0.5)) _float_speed *= -1;
        }
        _transform.position += new Vector3(-(_masterScript.speed+throw_speed) * Time.deltaTime, _float_speed * Time.deltaTime, 0.0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var ss = other.gameObject.GetComponent<SquareScript>();
        if (ss != null)
        {
            _float_speed *=-1;
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
            Debug.Log(i);
            _platform_id = i;
            i++;
            currentTrans = nextTrans;
            nextTrans=_masterScript.queue[i].GetComponent<Transform>();
        }
        _platform_vert_dist = Mathf.Abs(_transform.position.y - currentTrans.position.y);
    }
}
