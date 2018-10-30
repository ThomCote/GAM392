using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public int maxHP;
	public float blockCooldown;
	int curHP;

	PlayerFSM fsm;

	bool attackInputActive = true;
	bool defenseInputActive = false;

	bool isBlocking = false;
	bool isCoolingDown = false;

	public SoundPlayer damageAudioSrc;

    public Slider healthBar;

	public Text hpText;

	// Use this for initialization
	void Start () {
		fsm = GetComponent<PlayerFSM>();
		curHP = maxHP;
        InitializeHealthBar();
		//UpdateHPText();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GetHurt()
    {
        fsm.GetHurt();
    }

	public void HandleInput(string inputName)
	{
		if (defenseInputActive && inputName == "Space")
		{
			// Only do a block if we're not already blocking and noton cooldown - prevent stacking them.
			if (!isBlocking && !isCoolingDown)
			{
				fsm.HandleInput(inputName);
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
        
        //Alert fsm that player has been hurt
        fsm.GetHurt();

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
        GameManager.SetCountdownText("Defeat", Color.red);
        RhythmManager.StopMusicAndRhythm();
        ComboManager.SetStarted(false);
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
		//hpText.text = "Player HP: " + curHP;
        if (curHP <= 0)
            healthBar.value = 0;
        else
            healthBar.value = curHP;
	}

    void InitializeHealthBar()
    {
        healthBar.minValue = 0;
        healthBar.maxValue = maxHP;
        healthBar.value = curHP;
    }

	IEnumerator DoBlock()
	{
		isBlocking = true;

		yield return new WaitForSeconds(RhythmManager.GetSubdivisionLength() * 2.5f);

		isBlocking = false;

		StartCoroutine(BlockCooldown());

		yield return null;
	}

	IEnumerator BlockCooldown()
	{
		isCoolingDown = true;

		yield return new WaitForSeconds(blockCooldown);

		isCoolingDown = false;

		yield return null;
	}
}
