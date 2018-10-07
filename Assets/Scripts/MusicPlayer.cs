using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

	public float volume = 0.7f;

	AudioSource audSrcLowPerc;
	AudioSource audSrcHighPerc;

	// Use this for initialization
	void Start () {
		AudioSource[] auds = GetComponents<AudioSource>();
		audSrcLowPerc = auds[0];
		audSrcHighPerc = auds[1];

		EnableLowPercMusic();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayMusic()
	{
		audSrcLowPerc.Play();
		audSrcHighPerc.Play();
	}

	public void EnableLowPercMusic()
	{
		audSrcLowPerc.volume = this.volume;
		audSrcHighPerc.volume = 0.0f;
	}

	public void EnableHighPercMusic()
	{
		audSrcHighPerc.volume = this.volume;
		audSrcLowPerc.volume = 0.0f;
	}

	public void SwapMusic()
	{
		float oldVolume = audSrcLowPerc.volume;
		audSrcLowPerc.volume = audSrcHighPerc.volume;
		audSrcHighPerc.volume = oldVolume;
	}

	public void PlayMusic(float scheduleTime)
	{
		double playTime = AudioSettings.dspTime + scheduleTime;
		audSrcLowPerc.PlayScheduled(playTime);
		audSrcHighPerc.PlayScheduled(playTime);
	}
}
