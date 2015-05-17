using UnityEngine;
using System.Collections;

public class SoundScript : MonoBehaviour
{

    public bool inMenu;

    private AudioClip music;
    private AudioClip menu_click;
    private AudioClip pop;
    private AudioClip match;
    private AudioClip unmatch;
    

	// Use this for initialization
	void Start () {
	    if (inMenu)
	    {
            music = (AudioClip)Resources.Load("Sounds/menu", typeof(AudioClip));
	    }
	    else
	    {
            DontDestroyOnLoad(this);
            music = (AudioClip)Resources.Load("Sounds/game2", typeof(AudioClip));
            AudioSource tmp = (GameObject.Find("SoundController")).GetComponent<AudioSource>();
            tmp.clip = music;
            tmp.loop = true;
            if (!tmp.isPlaying)
                tmp.Play();
        }
	}
	
	// Update is called once per frame
	void Update () {
	}
}
