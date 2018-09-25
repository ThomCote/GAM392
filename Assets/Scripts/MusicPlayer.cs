using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

	AudioSource audSrc;

	// Use this for initialization
	void Start () {
		audSrc = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayMusic()
	{
		audSrc.Play();
	}

	public void PlayMusic(float scheduleTime)
	{
		audSrc.PlayScheduled(scheduleTime);
	}
}
