using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyController : MonoBehaviour {

	public int maxHP;
	protected int curHP;

    public EnemyFSM enemyFsm;

    public Text healthText;

	public SoundPlayer attackSoundPlayer;

    public Slider EnemyHealthBar;
	public Slider greyHealthBar;

	Animator ani;

	protected bool attacking;

	// Use this for initialization
	protected virtual void Start () {
		//EnemyHealthBar.maxValue = maxHP;
		InitializeHealthBar();
        curHP = maxHP;
		UpdateHPText();
		ani = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HandleInput(int moveIndex)
    {
		if (attacking)
		{
			enemyFsm.HandleInput(moveIndex);
		}
		else
		{
			enemyFsm.HandleInput(6);
		}
    }

	// Behavior per-subdivision, driven by rhythm system.
	// Assuming 16th note subdivisions.
	public abstract void OnSubdivision(int sub);

	public void TakeDamage(int dmg)
	{
		int oldHP = curHP;

		curHP -= dmg;

		UpdateHPText();

		// StartCoroutine(HPUpdateEffect(oldHP));

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

	IEnumerator HPUpdateEffect(int oldHP)
	{
		for (int i = 0; i < 6; ++i)
		{
			if (curHP <= 0)
			{
				EnemyHealthBar.value = 0;
				greyHealthBar.value = 0;
			}
			else
			{
				if (i % 2 == 0)
				{
					// greyHealthBar.value = curHP;
					EnemyHealthBar.value = oldHP;// - ComboManager.GetCurrentComboDamage();
				}
				else
				{
					// greyHealthBar.value = curHP;
					EnemyHealthBar.value = curHP;// - ComboManager.GetCurrentComboDamage();
				}
			}

			for (int j = 0; j < 5; ++j)
			{
				yield return new WaitForEndOfFrame();
				yield return new WaitForEndOfFrame();
			}
		}

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

		yield return null;
	}

	public void SetAttacking(bool b)
	{
		attacking = b;
		if (!attacking)
		{
			enemyFsm.HandleInput(6);
			// enemyFsm.HandleInput("idle");
			
			// ani.SetTrigger("attacktoidle");
		}
	}

	public bool IsAttacking()
	{
		return attacking;
	}
}
