using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour {

	// Unity singleton
	static ComboManager instance;

	// Highest number of subdivisions we'll wait for another input to an ongoing combo.
	public int maxComboWaitTime;

	bool comboOngoing = false;

	bool acceptSixteenths;

	List<int> currentCombo = new List<int>();
	List<int> comboNoteLengths = new List<int>();
	int currentComboDamage;
	long lastHitSubdivisionCount = 0; // The last subdivision overall that a hit was hit on

	public Text comboText;
	public Text subsSinceLastText;
	public Text curHitIndexText;

	bool started = false;

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

	}

	public static void Begin(bool acceptSixteenths)
	{
		instance.acceptSixteenths = acceptSixteenths;
		instance.started = true;
	}

	// Update is called once per frame
	void Update () {
		if (!started)
		{
			return;
		}

		long currentSubTotal = RhythmManager.GetTotalSubdivisionCount();

		if (comboOngoing && currentSubTotal - lastHitSubdivisionCount > maxComboWaitTime)
		{
			// End ongoing combo successfully
			FinishCombo();
		}
	}

	// HitIndex = 'index' of hit within measure (i.e. 16th note number 8)
	public static void AddHit(int hitIndex, long hitTotalSubdivisionCount)
	{
		instance.AddHit_Private(hitIndex, hitTotalSubdivisionCount);
	}

	void AddHit_Private(int hitIndex, long hitTotalSubdivisionCount)
	{
		comboOngoing = true;

		// Update takes care of combo time-outs / finishes
		long subsSinceLastHit;
		if (lastHitSubdivisionCount == -1)
		{
			subsSinceLastHit = -1;
		}
		else
		{
			subsSinceLastHit = hitTotalSubdivisionCount - lastHitSubdivisionCount;
		}

		subsSinceLastText.text = "Subs Since Last Hit: " + subsSinceLastHit;
		curHitIndexText.text = "Current Hit Index: " + hitIndex;

		// Evaluate this hit's damage and add it to the total
		AddHitDamage(subsSinceLastHit);

		lastHitSubdivisionCount = hitTotalSubdivisionCount;
	}

	void AddHitDamage(long subsSinceLastHit)
	{
		// Add note length to list - can be used to analyze sequences of notes
		if (subsSinceLastHit != -1)
		{
			comboNoteLengths.Add((int)subsSinceLastHit);
		}

		// Analyze special cases?
		// (Prescribed sequences of notes tied to specific values/actions?)
		// TODO

		// Evaluate this hit's damage
		// This scoring algorithm may change

		// Quarter note = 4pts
		// Eighth note = 2.5pts
		// Sixteenth note = 1.5pts

		// Weird, not ideal system where we're actually scoring the previous hit.
		// Because we don't know the length of a note until the next one is hit.
		// Thus, the first one in a combo isn't scored until the next hit,
		// and if a combo ends successfully the last one automatically gets 4 points.
		// (Maybe give extra points for finishing a particularly long combo?)

		int hitValue = 0;

		if (acceptSixteenths)
		{
			switch (subsSinceLastHit)
			{
				case -1:
					// First hit, don't score yet
					hitValue = 0;
					break;
				case 4:
					hitValue = 8;
					break;
				case 2:
					hitValue = 5;
					break;
				default:
					hitValue = 3;
					break;
			}
		}
		else
		{
			switch (subsSinceLastHit)
			{
				case -1:
					// First hit, don't score yet
					hitValue = 0;
					break;
				case 2:
					hitValue = 8;
					break;
				default:
					hitValue = 5;
					break;
			}
		}

		// Update debug text
		comboText.text += "\n" + hitValue;

		currentComboDamage += hitValue;
	}

	void FinishCombo()
	{
		comboOngoing = false;

		// Add an extra 4 points for the last note
		currentComboDamage += 8;

		// TODO - This'll involve applying the damage you've built up for your attack
		GameManager.DamageEnemy(currentComboDamage);

		// Reset text
		comboText.text = "Current Combo:";

		// Clear current combo lists
		currentCombo.Clear();
		comboNoteLengths.Clear();

		currentComboDamage = 0;

		lastHitSubdivisionCount = -1;
	}

	public static void FailCombo()
	{
		instance.FailCombo_Private();
	}

	void FailCombo_Private()
	{
		// Reset text
		comboText.text = "Current Combo:";

		// Clear current combo lists
		currentCombo.Clear();
		comboNoteLengths.Clear();

		currentComboDamage = 0;

		lastHitSubdivisionCount = -1;

		// TODO - Other effects
	}
}
