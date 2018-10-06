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

	public static void DamageEnemy(int dmg)
	{
		instance.currentEnemy.TakeDamage(dmg);
	}

	public static void IncrementMeasure()
	{
		++instance.currentMeasure;

		instance.currentMeasureText.text = "Current Measure: " + instance.currentMeasure;
	}
}
