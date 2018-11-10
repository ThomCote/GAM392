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

    public SpriteRenderer DamageAura1;
    public SpriteRenderer DamageAura2;
    public SpriteRenderer DamageAura3;

    public Slider healthBar;

	public Text hpText;

    public Color AuraColor1;
    public Color AuraColor2;
	public Color AuraColor3;

	bool hasFinisher = false;
	bool awaitSecondInputForFinisher = false;
	public float finisherInputWaitTime = 0.05f;

	public GameObject hitSparkPrefab;

	// Order: Up, Down, Left, Right
	public Transform[] hitSparkLocations;
	public Color[] hitSparkInputColors;

	// Use this for initialization
	void Start () {
		fsm = GetComponent<PlayerFSM>();
		curHP = maxHP;
        InitializeHealthBar();
        DeactivateAuras();
		//UpdateHPText();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GetHurt()
    {
        fsm.GetHurt();
    }

    public void ActivateAura(int auraNum)
    {
        //Lol dont care anymore just want it done
        if (auraNum == 1)
            DamageAura1.color = AuraColor1;
        else if (auraNum == 2)
            DamageAura2.color = AuraColor2;
        else if (auraNum == 3)
            DamageAura3.color = AuraColor3;
    }

    public void DeactivateAuras()
    {
        DamageAura1.color = Color.clear;
        DamageAura2.color = Color.clear;
        DamageAura3.color = Color.clear;
    }

	public void SetFinisherActive(bool b)
	{
		hasFinisher = b;

		if (b)
		{
			GameManager.SetCountdownText("Finish Him!", Color.red);
			GameManager.SetDefaultSignText("Finish Him!");
		}
		else
		{
			GameManager.SetCountdownText("", Color.red);
			GameManager.SetDefaultSignText("");
		}
	}

	public bool HasFinisher()
	{
		return hasFinisher;
	}

	public bool AwaitingSecondFinisherInput()
	{
		return awaitSecondInputForFinisher;
	}

	public IEnumerator FinisherInputWait()
	{
		awaitSecondInputForFinisher = true;

		yield return new WaitForSeconds(finisherInputWaitTime);

		awaitSecondInputForFinisher = false;

		yield return null;
	}

	public void HandleInput(string inputName)
	{
		if (inputName == "win")
		{
			fsm.HandleInput("win");
		}
		else if (defenseInputActive && inputName == "Space")
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
			if (hasFinisher && inputName == "Finisher")
			{
				PerformFinisher();
			}
			else
			{
				fsm.HandleInput(inputName);

				GameObject hitSpark;

				// Create a hit spark
				switch (inputName)
				{
					case "Up":
						hitSpark = Instantiate(hitSparkPrefab, hitSparkLocations[0].position, Quaternion.identity);
						hitSpark.GetComponent<HitSpark>().Execute(hitSparkInputColors[0]);
						break;
					case "Down":
						hitSpark = Instantiate(hitSparkPrefab, hitSparkLocations[1].position, Quaternion.identity);
						hitSpark.GetComponent<HitSpark>().Execute(hitSparkInputColors[1]);
						break;
					case "Left":
						hitSpark = Instantiate(hitSparkPrefab, hitSparkLocations[2].position, Quaternion.identity);
						hitSpark.GetComponent<HitSpark>().Execute(hitSparkInputColors[2]);
						break;
					case "Right":
						hitSpark = Instantiate(hitSparkPrefab, hitSparkLocations[3].position, Quaternion.identity);
						hitSpark.GetComponent<HitSpark>().Execute(hitSparkInputColors[3]);
						break;
				}
			}
		}
	}

	void PerformFinisher()
	{
		// Instantly kill enemy
		GameManager.DamageEnemy(GameManager.GetCurrentEnemyController().maxHP);

		GameManager.WinGame();
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
		fsm.HandleInput("lose");
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

	public void Heal(int hp)
	{
		curHP += hp;
		if (curHP > maxHP)
		{
			curHP = maxHP;
		}

		UpdateHPText();
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
