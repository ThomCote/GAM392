using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmManager : MonoBehaviour {

	// Unity singleton.
	static RhythmManager instance;

	public bool acceptSixteenths = true;

	public float musicStartDelay;
	public float rhythmStartDelay;

	public int beatsPerMeasure = 4;

	public InputTester[] inputTesters;

	public float tempo;
	public float goodMargin;
	public float perfectMargin;
	float quarterNoteLength; // Length of one full beat
	float eighthNoteLength;
	float sixteenthNoteLength;
	float curSubdivisionTime = 0.0f; // Current progress into the current subdivision of the beat (8th or 16th)
	float timeToNextSubdivision;
	int currentSubdivisionCount = -1; // Goes 0 - 15 for sixteenth notes, 0 - 7 for eighth notes

	AudioSource audSrc;
	public MusicPlayer musicPlayer;

	bool startedMusicAndRhythm = false;

	public Image sixteenthIndicatorImg;
	public Image eighthIndicatorImg;
	public Image quarterIndicatorImg;

	int quarterNoteCount;
	public Text quarterNoteCounter;
	int eighthNoteCount;
	public Text eighthNoteCounter;
	public Text sixteenthNoteCounter;

	long totalSubdivisionCount = 0;

	void Awake()
	{
		if (!instance)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
			instance.Startup();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Startup()
	{
		audSrc = GetComponent<AudioSource>();

		SetTempo(tempo);

		quarterIndicatorImg.color = Color.clear;
		eighthIndicatorImg.color = Color.clear;
		sixteenthIndicatorImg.color = Color.clear;

		quarterNoteCounter.text = "";
		eighthNoteCounter.text = "";
		sixteenthNoteCounter.text = "";

		if (!acceptSixteenths)
		{
			// hacky shit to make it sync up right to the bar
			// May have to fix it (or might not have to deal with it at all)
			// once I put in music that doens't have a leading beat.
			currentSubdivisionCount = 0; 
		}

		StartCoroutine(Begin());
	}

	IEnumerator Begin()
	{
		// Wait for stuff to load up if it needs to elsewhere
		yield return new WaitForSeconds(0.3f);

		musicPlayer.PlayMusic(musicStartDelay);
		yield return new WaitForSeconds(rhythmStartDelay);

		startedMusicAndRhythm = true;

		ComboManager.Begin(acceptSixteenths);

		yield return null;
	}
	
	// Update is called once per frame
	void Update () {

		if (!startedMusicAndRhythm)
		{
			return;
		}

		// Update beat timing tracking
		curSubdivisionTime += Time.deltaTime;

		if (acceptSixteenths)
		{
			// Base rhythm on sixteenth notes
			timeToNextSubdivision = sixteenthNoteLength - curSubdivisionTime;

			// On a sixteenth note
			if (curSubdivisionTime >= sixteenthNoteLength)
			{
				// Increment total subdivisions
				++totalSubdivisionCount;

				// Reset to beginning of next beat
				curSubdivisionTime = curSubdivisionTime - sixteenthNoteLength;

				++currentSubdivisionCount;

				if (currentSubdivisionCount == 15)
				{
					// We've moved onto a new measure
					GameManager.IncrementMeasure();
				}

				if (currentSubdivisionCount == 16)
				{
					currentSubdivisionCount = 0;
				}

				sixteenthNoteCounter.text = (currentSubdivisionCount + 1).ToString();

				if (currentSubdivisionCount % 2 == 0)
				{
					eighthNoteCount = (currentSubdivisionCount / 2) + 1;
					eighthNoteCounter.text = eighthNoteCount.ToString();
				}
				
				if (currentSubdivisionCount % 4 == 0)
				{
					quarterNoteCount = (currentSubdivisionCount / 4) + 1;
					quarterNoteCounter.text = quarterNoteCount.ToString();

				}

				OnSubdivision();
			}
		}
		else
		{
			// Base rhythm on eighth notes
			timeToNextSubdivision = eighthNoteLength - curSubdivisionTime;

			// On an eighth note
			if (curSubdivisionTime >= eighthNoteLength)
			{
				// Increment total subdivisions
				++totalSubdivisionCount;

				// Reset to beginning of next beat
				curSubdivisionTime = curSubdivisionTime - eighthNoteLength;

				++currentSubdivisionCount;
				if (currentSubdivisionCount == 7)
				{
					// We've moved onto a new measure
					GameManager.IncrementMeasure();
				}

				if (currentSubdivisionCount == 8)
				{
					currentSubdivisionCount = 0;
				}

				eighthNoteCounter.text = (currentSubdivisionCount + 1).ToString();

				if (currentSubdivisionCount % 2 == 0)
				{
					quarterNoteCount = (currentSubdivisionCount / 2) + 1;
					quarterNoteCounter.text = quarterNoteCount.ToString();
				}

				OnSubdivision();
			}
		}

		// Debug.Log("TotalSubCount: " + totalSubdivisionCount);

		// Update input testers
		foreach (InputTester tester in inputTesters)
		{
			tester.TriggeredUpdate();
		}
	}

	void OnSubdivision()
	{
		if (acceptSixteenths)
		{
			StartCoroutine(FlashImage(sixteenthIndicatorImg, 0.02f));

			// Check if it's an eighth note (0, 2, 4... sixteenth note)
			if (currentSubdivisionCount % 2 == 0)
			{
				StartCoroutine(FlashImage(eighthIndicatorImg));

				// Check if it's a quarter note
				if (currentSubdivisionCount % 4 == 0)
				{
					StartCoroutine(FlashImage(quarterIndicatorImg));

					audSrc.Play();
				}
			}
		}
		else
		{
			StartCoroutine(FlashImage(eighthIndicatorImg));

			// Check if it's a quarter note
			if (currentSubdivisionCount % 2 == 0)
			{
				StartCoroutine(FlashImage(quarterIndicatorImg));

				audSrc.Play();
			}
		}

		// Stuff that happens on all beats
		GameManager.OnSubdivision(currentSubdivisionCount);
	}

	public static void OnInputSuccess(bool nextSubdivision)
	{
		// nextSubdivision defines whether the beat we hit is the current subdivisionCount or the next one
		int hitSubdivision;

		// Measure-independent count of subdivisions so far
		long hitTotalSubdivision;
		
		if (nextSubdivision)
		{
			hitSubdivision = instance.currentSubdivisionCount + 1;

			hitTotalSubdivision = instance.totalSubdivisionCount + 1;

			if (instance.acceptSixteenths)
			{
				if (hitSubdivision == 16)
				{
					hitSubdivision = 0;
				}
			}
			else
			{
				if (hitSubdivision == 8)
				{
					hitSubdivision = 0;
				}
			}
		}
		else
		{
			hitSubdivision = instance.currentSubdivisionCount;

			hitTotalSubdivision = instance.totalSubdivisionCount;
		}

		// Now we know what subdivision it's hit.  Trigger any actions based on what was hit.
		if (instance.acceptSixteenths)
		{
			// hitSubdivision will be between 0 and 15
			ComboManager.AddHit(hitSubdivision, hitTotalSubdivision);
		}
		else
		{
			// hitSubdivision will be between 0 and 7
			ComboManager.AddHit(hitSubdivision, hitTotalSubdivision); // 8th note base true
		}
	}

	IEnumerator FlashImage(Image img, float duration = 0.04f)
	{
		img.color = Color.white;

		yield return new WaitForSeconds(duration);

		img.color = Color.clear;
	}

	public static float GetTimeToNextSubdivision()
	{
		return instance.timeToNextSubdivision;
	}

	public static float GetTimePastCurrentSubdivision()
	{
		return instance.curSubdivisionTime;
	}

	public static void GetInputMargins(out float _goodMargin, out float _perfectMargin)
	{
		_goodMargin = instance.goodMargin;
		_perfectMargin = instance.perfectMargin;
	}

	public static int GetCurrentSubdivisionCount()
	{
		return instance.currentSubdivisionCount;
	}

	public static long GetTotalSubdivisionCount()
	{
		return instance.totalSubdivisionCount;
	}

	public static float GetSubdivisionLength()
	{
		if (instance.acceptSixteenths)
		{
			return instance.sixteenthNoteLength;
		}
		else
		{
			return instance.eighthNoteLength;
		}
	}

	public static float GetQuarterNoteLength()
	{
		return instance.quarterNoteLength;
	}

	public static void SetTempo(float bpm)
	{
		if (bpm <= 0.0f)
		{
			throw new System.Exception("Invalid tempo value passed to RhythmManager.SetTempo()");
		}

		instance.tempo = bpm;
		instance.quarterNoteLength = 60.0f / bpm;
		instance.eighthNoteLength = instance.quarterNoteLength / 2.0f;
		instance.sixteenthNoteLength = instance.eighthNoteLength / 2.0f;
	}
}
