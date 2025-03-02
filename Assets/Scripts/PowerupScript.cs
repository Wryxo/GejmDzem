﻿using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine.UI;

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
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/tier_power");
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
            /*else if (powerRange < 0.8f)
            {
                power = PowerType.EndlessPoop;
                isEvil = false;
            }*/
            else if (powerRange < 0.8f)
            {
                power = PowerType.ColorRetard;
                isEvil = false;
            }
            else
            {
                power = PowerType.ColorChaos;
                isEvil = true;
            }
            if (isEvil)
            {
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/evil_power");
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/good_power");
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
                                  new Vector3(0.0f, Random.Range(0.6f, 1.0f), 0.0f);
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
        if (!_masterScript.pause)
        {
            updateCurrentPlatform();
            if (Mathf.Abs(_retard.GetComponent<Transform>().position.x - transform.position.x) < 1.0f)
            {
                Text tmp = (GameObject.FindGameObjectWithTag("GG")).GetComponent<Text>();
                switch (power)
                {
                    case PowerType.SpeedUp:
                        _masterScript.powerSpeed(4.0f);
                        tmp.text = "Speed" + Environment.NewLine + "Up";
                        break;
                    case PowerType.SpeedDown:
                        _masterScript.powerSlow(4.0f);
                        tmp.text = "Slow" + Environment.NewLine + "down";
                        break;
                    case PowerType.Hp:
                        _masterScript.powerHp(5.0f);
                        tmp.text = "Position" + Environment.NewLine + "Reset";
                        break;
                    case PowerType.EndlessPoop:
                        _masterScript.powerPoop(5.0f);
                        break;
                    case PowerType.ColorRetard:
                        _masterScript.powerRetard(6.0f);
                        tmp.text = "Easy" + Environment.NewLine + "colors";
                        break;
                    case PowerType.ColorChaos:
                        _masterScript.powerChaos(6.0f);
                        tmp.text = "Rainbow";
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
                if (isEvil)
                {
                    AudioSource astmp = GameObject.Find("PowerDown").GetComponent<AudioSource>();
                    astmp.Play();
                } else
                {
                    AudioSource astmp = GameObject.Find("PowerUp").GetComponent<AudioSource>();
                    astmp.Play();
                }
                Destroy(gameObject);
            } 
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
        if (!_masterScript.pause)
        {
            var ss = other.gameObject.GetComponent<SquareScript>();
            if ((ss != null) && (isEvil))
            {
                _float_speed = Random.Range(1.0f, 2.0f);
            } 
        }
    }

    public void checkPlatform()
    {
        if (!_masterScript.pause)
        {
            if (_transform.position.y < 1.0f)
            {
                //todo evaporate anim
                if (isEvil)
                {
                    _masterScript.score += 10;
                }
                if (isEvil)
                {
                    AudioSource astmp = GameObject.Find("PowerDown").GetComponent<AudioSource>();
                    astmp.Play();
                }
                else
                {
                    AudioSource astmp = GameObject.Find("PowerUp").GetComponent<AudioSource>();
                    astmp.Play();
                }
                Destroy(gameObject);
            } 
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
            if (i >= 0)
                nextTrans = _masterScript.queue[i].GetComponent<Transform>();
        }
        _masterScript.queue[_platform_id].GetComponent<SquareScript>().power = GetComponent<PowerupScript>();
        _platform_vert_dist = Mathf.Abs(_transform.position.y - currentTrans.position.y);
    }
}
