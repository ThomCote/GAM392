﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputTester : MonoBehaviour {

	public string inputButton;

	public SoundPlayer soundPlayer;

	public bool onlyPlayersTurn = true;
	public bool onlyDefense = false;

	PlayerController playerController;

	Image img;

	float goodMargin;
	float perfectMargin;

	// Use this for initialization
	void Start () {
		img = GetComponent<Image>();
		RhythmManager.GetInputMargins(out goodMargin, out perfectMargin);
		playerController = GameManager.GetPlayerController();
	}
	
	// Update is called once per frame
	public void TriggeredUpdate ()
	{
		if (onlyDefense && !playerController.GetDefenseInputActive())
		{
			return;
		}
		if (onlyPlayersTurn && !playerController.GetAttackInputActive())
		{
			return;
		}
		if (Input.GetButtonDown(inputButton))
		{
			float timePastSubdivision = RhythmManager.GetTimePastCurrentSubdivision();
			float timeToNext = RhythmManager.GetTimeToNextSubdivision();

			if (timePastSubdivision < goodMargin || timeToNext < goodMargin)
			{
				// Play the sound associated with this input.
				if (soundPlayer)
				{
					soundPlayer.PlaySound();
				}

				if (timePastSubdivision < perfectMargin || timeToNext < perfectMargin)
				{
					// Perfect
					StartCoroutine(FlashColor(Color.green));
				}
				else
				{
					// Good
					StartCoroutine(FlashColor(Color.yellow));
				}

				// Determine what subdivision we've hit and callback to RhythmManager
				// Only do this for attacks, not blocks.
				if (timePastSubdivision < timeToNext)
				{
					if (!onlyDefense)
					{
						// If this is the case, we're closer to the current subdivision number
						RhythmManager.OnInputSuccess(false);
					}
				}
				else
				{
					if (!onlyDefense)
					{
						// We're closer to the next subdivision number
						RhythmManager.OnInputSuccess(true);
					}
				}

				// Alert the player character FSM
				playerController.HandleInput(inputButton);
			}
			else
			{
				// Failed
				StartCoroutine(FlashColor(Color.red));

				ComboManager.FailCombo();
			}
		}
	}

	IEnumerator FlashColor(Color c)
	{
		img.color = c;

		yield return new WaitForSeconds(0.1f);

		img.color = Color.white;

		yield return null;
	}
}
