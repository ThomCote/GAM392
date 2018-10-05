using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	static GameManager instance;

	public PlayerController playerController;

	public EnemyController currentEnemy;

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

	public static PlayerController GetPlayerController()
	{
		return instance.playerController;
	}

	public static void DamageEnemy(int dmg)
	{
		instance.currentEnemy.TakeDamage(dmg);
	}
}
