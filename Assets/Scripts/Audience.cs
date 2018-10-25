using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audience : MonoBehaviour {

	public Text satisLevelText;
	public Text inputTrackingText;

	public SoundPlayer cheerSoundPlayer;
	public SoundPlayer booSoundPlayer;
	public SoundPlayer boringSoundPlayer;
	public SoundPlayer applauseSoundPlayer;

	int[] satisLevelThresholds = new int[]
	{
		-30, // Angry, doing damage
		-1, // Bored
		0, // Neutral
		50, // Excited
		100, // Wild
		150 // Fucking insane
	};

	int[] satisLevelBonuses = new int[]
	{
		0,
		0,
		0,
		20,
		40,
		60 // Also will get finisher
	};

	int numLevels = 6;

	int prevInputFreqMargin = 0;

	int curSatisValue = 0; // Precise satisfaction value
	int curSatisLevel = 2; // Level (index in level-related arrays)

	// If one input is ahead by more than this, that's bad.
	public int maxGoodInputFreq;

	// Tracking how frequently each attack input is hit, so we know whether we're bored.
	Dictionary<string, int> inputFrequencies = new Dictionary<string, int>();

	static Audience instance;

	// Use this for initialization
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
		// Starting these at 3 gives the player some padding.
		// If they were 0, the very first input would trigger boredom,
		// since it would be 100% of the inputs so far.
		inputFrequencies.Add("Left", 3);
		inputFrequencies.Add("Right", 3);
		inputFrequencies.Add("Up", 3);
		inputFrequencies.Add("Down", 3);

		UpdateText();
	}

	// Update is called once per frame
	void Update () {
		
	}

	void UpdateText()
	{
		satisLevelText.text = "SatisLevel: " + curSatisLevel + "\nSatisValue: " + curSatisValue;
		inputTrackingText.text = inputFrequencies["Left"] + "     " + inputFrequencies["Down"] + "     "
			+ inputFrequencies["Up"] + "     " + inputFrequencies["Right"];
	}

	// Damage from a successful (non-boring) combo adds to the satisfaction value
	public static void OnSuccessfulCombo(int dmg)
	{
		instance.OnSuccessfulCombo_Internal(dmg);
	}

	void OnSuccessfulCombo_Internal(int dmg)
	{
		// If we're still bored, we dont' care about your boring combo.
		// (If the player's combo was cool, the audience shouldn't be bored right here)
		// The input tracking resets when they turn bored, so the player should
		// be able to do one non-boring combo to make them happy.
		Debug.Log("OnSuccessfulCombo_Internal");

		EvaluateBoredom();

		if (curSatisLevel < 2)
		{
			OnBored(prevInputFreqMargin);

			return;
		}

		curSatisValue += dmg;

		UpdateText();

		// Update satisfaction level
		// If we just got a successful combo, we can only possibly be updating to good levels
		for (int i = 5; i >= 2; --i)
		{
			if (curSatisValue >= satisLevelThresholds[i])
			{
				// Debug.Log("upping satisfaction level!");
				ChangeSatisfactionLevel(i);
				break;
			}
		}
	}

	// Track the latest input and determine if we're bored.
	public static void RegisterAttackInput(string input)
	{
		instance.RegisterAttackInput_Internal(input);
	}

	void RegisterAttackInput_Internal(string input)
	{
		if (!inputFrequencies.ContainsKey(input))
		{
			return;
		}

		inputFrequencies[input] += 1;

		EvaluateBoredom();

		UpdateText();
	}

	// This is just a great function name.
	void EvaluateBoredom()
	{
		int totalInputs = inputFrequencies["Up"]
			+ inputFrequencies["Down"]
			+ inputFrequencies["Left"]
			+ inputFrequencies["Right"];

		int[] freqs = new int[4]
		{
			inputFrequencies["Up"],
			inputFrequencies["Down"],
			inputFrequencies["Left"],
			inputFrequencies["Right"]
		};

		// Get bored if any input is used too many times more than any other.
		int maxMargin = 0;
		int minMarginThatOffends = 99999;
		for (int i = 0; i < 4; ++i)
		{
			for (int j = i + 1; j < 4; ++j)
			{
				int diff = freqs[i] - freqs[j];
				Debug.Log("diff: " + diff);
				int margin = Mathf.Abs(diff);
				if (margin > maxMargin)
				{
					maxMargin = margin;
				}
				if (margin > maxGoodInputFreq && margin < minMarginThatOffends)
				{
					minMarginThatOffends = margin;
				}
			}
		}
		if (maxMargin > maxGoodInputFreq)
		{
			// We're bored
			OnBored(minMarginThatOffends);
			return;
		}

		// Not bored
		if (curSatisLevel <= 1)
		{
			curSatisLevel = 2;
			curSatisValue = 0;
		}
	}

	// I'm on bored with this system.
	void OnBored(int inputFreqMargin)
	{
		if (curSatisValue >= 0)
		{
			curSatisValue = -1;
			ChangeSatisfactionLevel(1);
		}
		else
		{
            //Debug.Log("IFM: " + inputFreqMargin);
            //// If the player's making progress, don't do this...
            //if (inputFreqMargin != 99999 && inputFreqMargin >= prevInputFreqMargin)
            //{
            //    // Set the input frequency counts such that the player can fix it relatively easily.
            //    // If they get way ahead with one input we don't want it to take forever to
            //    // get the rest back up.

            //    // Get max input
            //    string maxInputName = "";
            //    int maxInputVal = 0;
            //    foreach (KeyValuePair<string, int> kvp in inputFrequencies)
            //    {
            //        if (kvp.Value > maxInputVal)
            //        {
            //            maxInputVal = kvp.Value;
            //            maxInputName = kvp.Key;
            //        }
            //    }
            //    Dictionary<string, int> otherInputs = new Dictionary<string, int>();
            //    foreach (KeyValuePair<string, int> kvp in inputFrequencies)
            //    {
            //        if (kvp.Key != maxInputName && kvp.Value != maxInputVal)
            //        {
            //            otherInputs.Add(kvp.Key, kvp.Value);
            //        }
            //    }
            //    // Now - set the offending input to be just a little higher than the other 3
            //    foreach (KeyValuePair<string, int> kvp in otherInputs)
            //    {
            //        inputFrequencies[kvp.Key] = 3;
            //    }
            //    inputFrequencies[maxInputName] = 12;
            //}

            // If we're below 0 but not at bored yet
            if (curSatisValue > satisLevelThresholds[0])
			{
				// Decrease satisfaction.
				curSatisValue -= 10;

				// If we're now below angry level, change to that.
				if (curSatisValue <= satisLevelThresholds[0])
				{
					ChangeSatisfactionLevel(0);
				}
			}
		}

		UpdateText();

		prevInputFreqMargin = inputFreqMargin;
	}

	// Change current satisfaction level
	void ChangeSatisfactionLevel(int level)
	{
		if (level < 0 || level > 5)
		{
			// No exception, just don't do anything bc I'm lazy 
			// and have a week to do this entire system.
			return;
		}

		curSatisLevel = level;

		if (level <= 1)
		{
			// If we're moving into a 'bad ' state, reset the input tracking.
			// Giving a chance to immediately appease audience by doing a good combo.
			//inputFrequencies["Up"] = 3;
			//inputFrequencies["Down"] = 3;
			//inputFrequencies["Left"] = 3;
			//inputFrequencies["Right"] = 3;

			// Tell the player to kick it up a notch
			// TODO: Play 'Change it up!' or 'Boooooring!' voice clip
			boringSoundPlayer.PlaySound();

			if (level == 0)
			{
				// For when they're reeeally angry
				// TODO
				booSoundPlayer.PlaySound();
			}
		}
		else
		{
			switch (level)
			{
				case 2: // Neutral
					break;
				case 3: // Excited
					applauseSoundPlayer.PlaySound();
					break;
				case 4: // Wild
					cheerSoundPlayer.PlaySound();
					break;
				case 5: // Fucking mental (give finisher)
					cheerSoundPlayer.PlaySound();
					break;
			}
		}

		UpdateText();
	}
}