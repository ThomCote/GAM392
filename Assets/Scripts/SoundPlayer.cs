using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour {

	public AudioClip clip;
	public float volume = 1.0f;

	List<AudioSource> audSources;

	// Use this for initialization
	void Start () {
		audSources = new List<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlaySound()
	{
		AudioSource aud = null;

		// Find first inactive sound player
		foreach (AudioSource src in audSources)
		{
			if (!src.isPlaying)
			{
				aud = src;
				break;
			}
		}

		// Create a new one if there isn't a free one
		if (aud == null)
		{
			aud = gameObject.AddComponent<AudioSource>();
			aud.clip = this.clip;
			aud.volume = this.volume;
		}

		aud.Play();
	}
}
