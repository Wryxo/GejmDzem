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
            DontDestroyOnLoad(transform.gameObject);
            music = (AudioClip)Resources.Load("Sounds/menu", typeof(AudioClip));
            pop = (AudioClip)Resources.Load("Sounds/pop", typeof(AudioClip));
	    }
	}
	
	// Update is called once per frame
	void Update () {
	}
}
