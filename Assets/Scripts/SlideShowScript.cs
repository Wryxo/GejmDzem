using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SlideShowScript : MonoBehaviour {
    private GameObject[] _slides;
    private int activeSlide = 1;
	// Use this for initialization
	void Start () {
        _slides = GameObject.FindGameObjectsWithTag("introPanel");
        ShowActivePanel();
    }

    private void ShowActivePanel()
    {
        foreach (var slide in _slides)
        {
            slide.SetActive(slide.name.Contains("" + activeSlide));
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetMouseButtonDown(0)) { 
            activeSlide++;
            if (activeSlide > _slides.Length)
            {
                Application.LoadLevel("MenuScene");
                return;
            }
            ShowActivePanel();
        }
    }
}
