using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class PowerupScript : MonoBehaviour
{
    private Transform _transform;
    private MasterScript _masterScript;
    private GameObject _master,_retard;

    public bool isEvil = true;

    public int _platform_id;
    public float _platform_vert_dist;

    private float _float_speed = 0.1f;
    private float _throw_speed = 2.0f;

    public enum PowerType
    {
        SpeedUp, SpeedDown, Hp, EndlessPoop, ColorRetard, ColorChaos, TierOne, TierTwo, TierThree,Unset
    }

    //distrib SpeedUp=0.2, slow=0.2,Hp=0.2,Poop=0.2,c=0.1

    public PowerType power=PowerType.Unset;

    // Use this for initialization
    void Start()
    {
        if (power != PowerType.Unset)
        {
            isEvil = false;
        }
        else
        {
            float powerRange = Random.Range(0.0f, 1.0f);
            if (powerRange < 0.2f)
            {
                power = PowerType.SpeedUp;
                isEvil = true;
            }
            else if (powerRange < 0.4f)
            {
                power = PowerType.SpeedDown;
                isEvil = false;
            }
            else if (powerRange < 0.6f)
            {
                power = PowerType.Hp;
                isEvil = false;
            }
            else if (powerRange < 0.8f)
            {
                power = PowerType.EndlessPoop;
                isEvil = false;
            }
            else if (powerRange < 0.9f)
            {
                power = PowerType.ColorRetard;
                isEvil = false;
            }
            else
            {
                power = PowerType.ColorChaos;
                isEvil = true;
            }
        }
        _transform = GetComponent<Transform>();
        _master = GameObject.FindGameObjectWithTag("GameController");
        _retard = GameObject.FindGameObjectWithTag("Retard");
        _masterScript = _master.GetComponent<MasterScript>();
        if (isEvil)
        {
            _float_speed = 0.5f;
            _transform.position = _master.GetComponent<Transform>().position +
                                  new Vector3(0.0f, Random.Range(0.6f, 2.0f), 0.0f);
        }
        else
        {
            _throw_speed = 0.0f;
            _transform.position = _master.GetComponent<Transform>().position +
                                  new Vector3(0.0f, Random.Range(0.7f, 0.8f), 0.0f);
        }
        //nastavit initial platform
        _platform_id = _masterScript.queue.Count-1;
        updateCurrentPlatform();
        transform.position = new Vector3(_masterScript.queue[_platform_id].GetComponent<Transform>().position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        updateCurrentPlatform();
        if (Mathf.Abs(_retard.GetComponent<Transform>().position.x - transform.position.x) < 1.0f)
        {
            switch (power)
            {
                case PowerType.SpeedUp:
                    _masterScript.powerSpeed(4.0f);
                    break;
                case PowerType.SpeedDown:
                    _masterScript.powerSlow(4.0f);
                    break;
                case PowerType.Hp:
                    _masterScript.powerHp(5.0f);
                    break;
                case PowerType.EndlessPoop:
                    _masterScript.powerPoop(5.0f);
                    break;
                case PowerType.ColorRetard:
                    _masterScript.powerRetard(6.0f);
                    break;
                case PowerType.ColorChaos:
                    _masterScript.powerChaos(6.0f);
                    break;
                case PowerType.TierOne:
                    _masterScript.setCombo3();
                    break;
                case PowerType.TierTwo:
                    _masterScript.setCombo4();
                    break;
                case PowerType.TierThree:
                    _masterScript.setCombo5();
                    break;
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

    public void checkPlatform()
    {
        if (_transform.position.y < 1.0f)
        {
            //todo evaporate anim
            if (isEvil)
            {
                _masterScript.score += 10;
            }
            Destroy(gameObject);
        }
    }

    void updateCurrentPlatform()
    {
        //if null pointers its from this
        int i = _platform_id -1;
        if (i < 0) return;
        Transform nextTrans = _masterScript.queue[i].GetComponent<Transform>();
        Transform currentTrans = _masterScript.queue[_platform_id].GetComponent<Transform>();
        _masterScript.queue[_platform_id].GetComponent<SquareScript>().power = null;
        while (Mathf.Abs(nextTrans.position.x - transform.position.x) < Mathf.Abs(currentTrans.position.x - transform.position.x))
        {
            //if null pointers its from this
            _masterScript.queue[_platform_id].GetComponent<SquareScript>().power = null;
            _platform_id = i;
            i--;
            currentTrans = nextTrans;
            nextTrans = _masterScript.queue[i].GetComponent<Transform>();
        }
        _masterScript.queue[_platform_id].GetComponent<SquareScript>().power = GetComponent<PowerupScript>();
        _platform_vert_dist = Mathf.Abs(_transform.position.y - currentTrans.position.y);
    }
}
