using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlideShowScript : MonoBehaviour {
    private GameObject[] _slides;
    private int activeSlide = 1;
	// Use this for initialization
	void Start () {
        _slides = GameObject.FindGameObjectsWithTag("introPanel");
        Debug.Log("hello");
        ShowActivePanel();
    }

    void OnMouseDown()
    {
        
        activeSlide++;
        if (activeSlide > _slides.Length)
        {
            Application.LoadLevel("MenuScene");
            return;
        }
        ShowActivePanel();
    }

    private void ShowActivePanel()
    {
        foreach (var slide in _slides)
        {
            Debug.Log(slide.name);
            slide.SetActive(slide.name.Contains("" + activeSlide));
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
