using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputTester : MonoBehaviour {

	public string inputButton;

	public SoundPlayer soundPlayer;

	public bool onlyPlayersTurn = true;
	public bool onlyDefense = false;

	PlayerController playerController;
    EnemyController currEnemy;

	Image img;

	float goodMargin;
	float perfectMargin;

	// Use this for initialization
	void Start () {
		img = GetComponent<Image>();
		RhythmManager.GetInputMargins(out goodMargin, out perfectMargin);
		playerController = GameManager.GetPlayerController();
        currEnemy = GameManager.GetCurrentEnemyController();
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
			// If this input is currently 'overused' don't accept it and give some feedback
			if (Audience.IsInputOverdone(inputButton))
			{
				// Play a whiff sound I Guess
				GameManager.PlayWhiffSound();

                //Play player animation
                playerController.HandleInput(inputButton);

                // Enemy Block animation, 5 is block index
                currEnemy.HandleInput(5);

				return;
			}

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
					// StartCoroutine(FlashColor(Color.green));
				}
				else
				{
					// Good
					// StartCoroutine(FlashColor(Color.yellow));
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

                //Alert current enemy fsm its been hurt if its players turn
                if (GameManager.GetIsPlayersTurn() == true)
                {
                    currEnemy.GetHurt();
                }

				// Alert the audience input tracking
				Audience.RegisterAttackInput(inputButton);
			}
			else
			{
				// Failed
				// StartCoroutine(FlashColor(Color.red));

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
