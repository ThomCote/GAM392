using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public int maxHP;
	int curHP;

	PlayerFSM fsm;

	bool inputActive = true;

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
		if (inputActive)
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
		inputActive = !inputActive;
	}

	public bool GetInputActive()
	{
		return inputActive;
	}

	void UpdateHPText()
	{
		hpText.text = "Player HP: " + curHP;
	}
}
