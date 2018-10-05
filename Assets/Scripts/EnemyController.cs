using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

	public int maxHP;
	int curHP;

	public Text healthText;

	// Use this for initialization
	void Start () {
		curHP = maxHP;
		UpdateHPText();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TakeDamage(int dmg)
	{
		curHP -= dmg;

		UpdateHPText();

		if (curHP <= 0)
		{
			Die();
		}
	}

	void Die()
	{

	}

	void UpdateHPText()
	{
		healthText.text = "Enemy HP: " + curHP;
	}
}
