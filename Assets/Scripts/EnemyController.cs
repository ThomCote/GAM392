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

	protected bool attacking;

	// Use this for initialization
	protected virtual void Start () {
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

	public bool IsAttacking()
	{
		return attacking;
	}
}
