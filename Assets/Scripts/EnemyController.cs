using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyController : MonoBehaviour {

	public int maxHP;
	int curHP;

	public Text healthText;

	protected bool attacking;

	// Use this for initialization
	void Start () {
		curHP = maxHP;
		UpdateHPText();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Behavior per-subdivision, driven by rhythm system.
	// Assuming 16th note subdivisions.
	public abstract void OnSubdivision(int sub);

	public void TakeDamage(int dmg)
	{
		curHP -= dmg;

		UpdateHPText();

		if (curHP <= 0)
		{
			Die();
		}
	}

	protected void DealDamage(int dmg)
	{
		GameManager.DamagePlayer(dmg);
	}

	void Die()
	{

	}

	void UpdateHPText()
	{
		healthText.text = "Enemy HP: " + curHP;
	}

	public void ToggleAttacking()
	{
		attacking = !attacking;
	}
}
