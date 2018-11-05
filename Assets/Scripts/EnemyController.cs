using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyController : MonoBehaviour {

	public int maxHP;
	int curHP;

    public EnemyFSM enemyFsm;

    public Text healthText;

	public SoundPlayer attackSoundPlayer;

    public Slider EnemyHealthBar;
	public Slider greyHealthBar;

	protected bool attacking;

	// Use this for initialization
	protected virtual void Start () {
		//EnemyHealthBar.maxValue = maxHP;
		InitializeHealthBar();
        curHP = maxHP;
		UpdateHPText();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HandleInput(int moveIndex)
    {
        enemyFsm.HandleInput(moveIndex);
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

    public void GetHurt()
    {
        enemyFsm.GetHurt();
    }

	public void UpdateGreyHealth(int curComboDmg)
	{

	}

	protected void DealDamage(int dmg)
	{
		GameManager.DamagePlayer(dmg);
	}

	void Die()
	{
        RhythmManager.StopMusicAndRhythm();
        ComboManager.SetStarted(false);
        GameManager.SetCountdownText("Victory", Color.green);
        GameManager.WinGame();
    }

    void InitializeHealthBar()
    {
        EnemyHealthBar.minValue = 0;
        EnemyHealthBar.maxValue = maxHP;
        EnemyHealthBar.value = curHP;

		greyHealthBar.minValue = 0;
		greyHealthBar.maxValue = maxHP;
		greyHealthBar.value = curHP;
    }

    public void UpdateHPText()
	{
		//healthText.text = "Enemy HP: " + curHP;
        if (curHP <= 0)
		{
			EnemyHealthBar.value = 0;
			greyHealthBar.value = 0;
		}
        else
		{
			greyHealthBar.value = curHP;
			EnemyHealthBar.value = curHP - ComboManager.GetCurrentComboDamage();
		} 
    }

	public void SetAttacking(bool b)
	{
		attacking = b;
	}

	public bool IsAttacking()
	{
		return attacking;
	}
}
