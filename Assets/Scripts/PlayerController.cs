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

	bool isBlocking = false;

	public SoundPlayer damageAudioSrc;

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

			// Only do a block if we're not already blocking - prevent stacking them.
			if (!isBlocking)
			{
				StartCoroutine(DoBlock());
			}
		}
		else if (attackInputActive)
		{
			fsm.HandleInput(inputName);
		}
	}

	public void TakeDamage(int dmg)
	{
		if (isBlocking)
		{
			return;
		}

		curHP -= dmg;

		UpdateHPText();

		damageAudioSrc.PlaySound();

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

	IEnumerator DoBlock()
	{
		isBlocking = true;

		yield return new WaitForSeconds(RhythmManager.GetSubdivisionLength() * 2.0f);

		isBlocking = false;

		yield return null;
	}
}
