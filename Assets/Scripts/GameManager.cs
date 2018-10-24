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

	public Text turnCountdownText;
	bool nextMeasureSwitch = false;

	bool isPlayersTurn = true;

	public MusicPlayer musicPlayer;

	public SpriteRenderer leftSpotlight;
	public SpriteRenderer rightSpotlight;

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
		instance.currentMeasureText.text = "Measure: " + instance.currentMeasure;
		instance.turnCountdownText.text = "";

		leftSpotlight.color = Color.white;
		rightSpotlight.color = Color.clear;
	}

	public static PlayerController GetPlayerController()
	{
		return instance.playerController;
	}

    public static EnemyController GetCurrentEnemyController()
    {
        return instance.currentEnemy;
    }

	public static void DamagePlayer(int dmg)
	{
		instance.playerController.TakeDamage(dmg);
	}

	public static void DamageEnemy(int dmg)
	{
		instance.currentEnemy.TakeDamage(dmg);
	}

    public static void PlayerHurt()
    {
        instance.currentEnemy.GetHurt();
    }

    public static void EnemyHurt()
    {
        instance.playerController.GetHurt();
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

		instance.currentMeasureText.text = "Measure: " + instance.currentMeasure;

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

		isPlayersTurn = !isPlayersTurn;

		currentEnemy.ToggleAttacking();

		musicPlayer.SwapMusic();

		if (isPlayersTurn)
		{
			instance.StartCoroutine(instance.DelayedSwapEvents("Attack!"));
		}
		else
		{
			instance.StartCoroutine(instance.DelayedSwapEvents("Block!"));
		}
	}

	void SwapSpotlights()
	{
		if (leftSpotlight.color == Color.clear)
		{
			leftSpotlight.color = Color.white;
			rightSpotlight.color = Color.clear;
		}
		else
		{
			leftSpotlight.color = Color.clear;
			rightSpotlight.color = Color.white;
		}
	}

    public static bool GetIsPlayersTurn()
    {
        return instance.isPlayersTurn;
    }

	IEnumerator DelayedSwapEvents(string swapText)
	{
		yield return new WaitForSeconds(RhythmManager.GetSubdivisionLength());

		turnCountdownText.text = swapText;
		SwapSpotlights();

		yield return new WaitForSeconds(0.8f);

		turnCountdownText.text = "";

		yield return null;
	}
}
