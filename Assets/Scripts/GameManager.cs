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
	}

	public static void IncrementMeasure()
	{
		++instance.currentMeasure;

		instance.currentMeasureText.text = "Current Measure: " + instance.currentMeasure;

		// Current - 1 because it'll switch on measures 9, 17, etc not on 8, 16, etc.
		if ((instance.currentMeasure - 1) % instance.combatPhaseLength == 0)
		{
			instance.ChangeCombatPhase();
		}
	}

	void ChangeCombatPhase()
	{
		playerController.ToggleInputActive();

		currentEnemy.ToggleAttacking();

		musicPlayer.SwapMusic();
	}
}
