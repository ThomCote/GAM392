using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public int maxHP;
	int curHP;

	PlayerFSM fsm;

	bool attackInputActive = true;
	bool defenseInputActive = false;

	public Text hpText;

	// Use this for initialization
	void Start () {
		fsm = GetComponent<PlayerFSM>();
		curHP = maxHP;
		UpdateHPText();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HandleInput(string inputName)
	{
		if (defenseInputActive && inputName == "Space")
		{
			fsm.HandleInput(inputName);
		}
		else if (attackInputActive)
		{
			fsm.HandleInput(inputName);
		}
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

	public void ToggleInputActive()
	{
		attackInputActive = !attackInputActive;
		defenseInputActive = !defenseInputActive;
	}

	public bool GetAttackInputActive()
	{
		return attackInputActive;
	}

	public bool GetDefenseInputActive()
	{
		return defenseInputActive;
	}

	void UpdateHPText()
	{
		hpText.text = "Player HP: " + curHP;
	}
}
