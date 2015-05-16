using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class PowerupScript : MonoBehaviour
{
    private Transform _transform;
    private MasterScript _masterScript;

    public bool isEvil = true;

    public int _platform_id;
    public float _platform_vert_dist;

    private float _float_speed = 0.1f;
    private float _throw_speed = 2.0f;

    private enum PowerType
    {
        SpeedUp, SpeedDown, Hp, EndlessPoop, ColorRetard, ColorChaos
    }

    //distrib SpeedUp=0.2, slow=0.2,Hp=0.2,Poop=0.2,c=0.1

    private PowerType power;

    // Use this for initialization
    void Start()
    {
        isEvil = false;
        _transform = GetComponent<Transform>();
        GameObject master = GameObject.FindGameObjectWithTag("GameController");
        _masterScript = master.GetComponent<MasterScript>();
        if (isEvil)
        {
            _float_speed = 0.5f;
            _transform.position = master.GetComponent<Transform>().position +
                                  new Vector3(0.0f, Random.Range(0.6f, 2.0f), 0.0f);
        }
        else
        {
            _throw_speed = 0.0f;
            _transform.position = master.GetComponent<Transform>().position +
                                  new Vector3(0.0f, Random.Range(0.7f, 0.8f), 0.0f);
        }
        float powerRange = Random.Range(0.0f, 1.0f);
        if (powerRange < 0.2f)
        {
            power = PowerType.SpeedUp;
        }
        else if (powerRange < 0.4f)
        {
            power = PowerType.SpeedDown;
        }
        else if (powerRange < 0.6f)
        {
            power = PowerType.Hp;
        }
        else if (powerRange < 0.8f)
        {
            power = PowerType.EndlessPoop;
        }
        else if (powerRange < 0.9f)
        {
            power = PowerType.ColorRetard;
        }
        else
        {
            power = PowerType.ColorChaos;
        }
        //nastavit initial platform
        _platform_id = 0;
        updateCurrentPlatform();
        transform.position = new Vector3(_masterScript.queue[_platform_id].GetComponent<Transform>().position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        updateCurrentPlatform();
        if (Mathf.Abs(GameObject.FindGameObjectWithTag("Retard").GetComponent<Transform>().position.x - transform.position.x) < 1.0f)
        {
            switch (power)
            {
                case PowerType.SpeedUp:
                {
                    int pes = 47;
                }
            }
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (isEvil)
        {
            //mumbojumbo
            _float_speed -= _masterScript.speed / 3.0f * Time.deltaTime;
        }
        else
        {
            if ((_transform.position.y > 1.0f) || (_transform.position.y < 0.5f)) _float_speed *= -1;
        }
        _transform.position += new Vector3(-(_masterScript.speed + _throw_speed) * Time.deltaTime, _float_speed * Time.deltaTime, 0.0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var ss = other.gameObject.GetComponent<SquareScript>();
        if ((ss != null) && (isEvil))
        {
            _float_speed = Random.Range(1.0f, 2.0f);
        }
    }

    void updateCurrentPlatform()
    {
        //if null pointers its from this
        int i = _platform_id + 1;
        Transform nextTrans = _masterScript.queue[i].GetComponent<Transform>();
        Transform currentTrans = _masterScript.queue[_platform_id].GetComponent<Transform>();
        while (Mathf.Abs(nextTrans.position.x - transform.position.x) < Mathf.Abs(currentTrans.position.x - transform.position.x))
        {
            //if null pointers its from this
            Debug.Log(i);
            _platform_id = i;
            i++;
            currentTrans = nextTrans;
            nextTrans = _masterScript.queue[i].GetComponent<Transform>();
        }
        _platform_vert_dist = Mathf.Abs(_transform.position.y - currentTrans.position.y);
    }
}
