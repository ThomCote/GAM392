using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	static GameManager instance;

	public PlayerController playerController;

	public EnemyController currentEnemy;

	public int combatPhaseLength = 8;

	public Text currentMeasureText;

	public Text turnText;

	public Text turnCountdownText;
	bool nextMeasureSwitch = false;

	public MusicPlayer musicPlayer;

	int currentMeasure = 1;

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
		instance.currentMeasureText.text = "Current Measure: " + instance.currentMeasure;
		instance.turnCountdownText.text = "";
	}

	public static PlayerController GetPlayerController()
	{
		return instance.playerController;
	}

	public static void DamagePlayer(int dmg)
	{
		instance.playerController.TakeDamage(dmg);
	}

	public static void DamageEnemy(int dmg)
	{
		instance.currentEnemy.TakeDamage(dmg);
	}

	// Called every subdivision of a measure
	public static void OnSubdivision(int subCount)
	{
		// Just accepting 16th notes by default for now

		// Trigger any enemy behaviors
		instance.currentEnemy.OnSubdivision(subCount);

		if (instance.nextMeasureSwitch)
		{
			switch (subCount)
			{
				case 4:
					instance.turnCountdownText.text = "3";
					break;
				case 8:
					instance.turnCountdownText.text = "2";
					break;
				case 12:
					instance.turnCountdownText.text = "1";
					break;
			}
		}
	}

	public static void IncrementMeasure()
	{
		instance.nextMeasureSwitch = false;

		++instance.currentMeasure;

		instance.currentMeasureText.text = "Current Measure: " + instance.currentMeasure;

		// Current - 1 because it'll switch on measures 9, 17, etc not on 8, 16, etc.
		if ((instance.currentMeasure - 1) % instance.combatPhaseLength == 0)
		{
			instance.ChangeCombatPhase();
		}
		else if (instance.currentMeasure % instance.combatPhaseLength == 0)
		{
			// If we're in the measure right before the switch
			instance.nextMeasureSwitch = true;
		}
	}

	void ChangeCombatPhase()
	{
		playerController.ToggleInputActive();

		currentEnemy.ToggleAttacking();

		musicPlayer.SwapMusic();

		if (turnText.text == "Your turn!")
		{
			turnText.text = "Enemy's turn!";
			StartCoroutine(ShowSwitchText("Defend!"));
		}
		else if (turnText.text == "Enemy's turn!")
		{
			turnText.text = "Your turn!";
			StartCoroutine(ShowSwitchText("Fight!"));
		}
	}

	IEnumerator ShowSwitchText(string txt)
	{
		turnCountdownText.text = txt;

		yield return new WaitForSeconds(0.8f);

		turnCountdownText.text = "";

		yield return null;
	}
}
