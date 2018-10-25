using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	static GameManager instance;

	public PlayerController playerController;

	public EnemyController currentEnemy;

	public SoundPlayer whiffSoundPlayer;

	public int combatPhaseLength = 8;

	public Text currentMeasureText;

	public Text turnCountdownText;
	bool nextMeasureSwitch = false;

	bool isPlayersTurn = true;

	public MusicPlayer musicPlayer;

	public SpriteRenderer leftSpotlight;
	public SpriteRenderer rightSpotlight;

    public float spotLightFlashTime = 0.01f;

    Color DefaultSpotLightColor = new Color(1, 0.7948295f, 0, 0.5529412f);

	int currentMeasure = 1;

	void Awake()
	{
		if (!instance)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
			instance.Startup();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Startup()
	{
		instance.currentMeasureText.text = "Measure: " + instance.currentMeasure;
		instance.turnCountdownText.text = "";

//<<<<<<< HEAD
		leftSpotlight.color = DefaultSpotLightColor;
//=======
		//leftSpotlight.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
//>>>>>>> 9475b744fe88b6d7816ed39f2aaeb3b340af0625
		rightSpotlight.color = Color.clear;
	}

	public static PlayerController GetPlayerController()
	{
		return instance.playerController;
	}

    public static EnemyController GetCurrentEnemyController()
    {
        return instance.currentEnemy;
    }

	public static void DamagePlayer(int dmg)
	{
		instance.playerController.TakeDamage(dmg);
	}

	public static void DamageEnemy(int dmg)
	{
		instance.currentEnemy.TakeDamage(dmg);
	}

	public static void PlayWhiffSound()
	{
		instance.whiffSoundPlayer.PlaySound();
	}

    public static void PlayerHurt()
    {
        instance.currentEnemy.GetHurt();
    }

    public static void EnemyHurt()
    {
        instance.playerController.GetHurt();
    }

	// Called every subdivision of a measure
	public static void OnSubdivision(int subCount)
	{
		// Just accepting 16th notes by default for now

		// Trigger any enemy behaviors
		instance.currentEnemy.OnSubdivision(subCount);

		if (instance.nextMeasureSwitch)
		{
            switch (subCount)
            {
                case 4:
                    instance.turnCountdownText.text = "3";
                    //instance.StartCoroutine(instance.FlashSprite(instance.leftSpotlight, Color.clear, instance.spotLightFlashTime));
                    break;
                case 8:
                    instance.turnCountdownText.text = "2";
                    //instance.StartCoroutine(instance.FlashSprite(instance.leftSpotlight, Color.clear, instance.spotLightFlashTime));
                    break;
                case 12:
                    instance.turnCountdownText.text = "1";
                    //instance.StartCoroutine(instance.FlashSprite(instance.leftSpotlight, Color.clear, instance.spotLightFlashTime));
                    break;
            }
		}
	}

    IEnumerator FlashSprite(SpriteRenderer spr, Color col, float duration)
    {
        spr.color = col;

        yield return new WaitForSeconds(duration);

        spr.color = Color.clear;
    }

    public static void IncrementMeasure()
	{
		instance.nextMeasureSwitch = false;

		++instance.currentMeasure;

		instance.currentMeasureText.text = "Measure: " + instance.currentMeasure;

		// Current - 1 because it'll switch on measures 9, 17, etc not on 8, 16, etc.
		if ((instance.currentMeasure - 1) % instance.combatPhaseLength == 0)
		{
			instance.ChangeCombatPhase();
		}
		else if (instance.currentMeasure % instance.combatPhaseLength == 0)
		{
			// If we're in the measure right before the switch
			instance.nextMeasureSwitch = true;
		}
	}

	void ChangeCombatPhase()
	{
		playerController.ToggleInputActive();

		isPlayersTurn = !isPlayersTurn;

		currentEnemy.ToggleAttacking();

		musicPlayer.SwapMusic();

		if (isPlayersTurn)
		{
//<<<<<<< HEAD
//=======
			Audience.ResetInputTracking();
            instance.StartCoroutine(instance.DelayedSwapEvents("Go!"));
            //instance.StartCoroutine(instance.DelayedSwapEvents("Attack!"));
            //>>>>>>> 9475b744fe88b6d7816ed39f2aaeb3b340af0625
        }
		else
		{
			instance.StartCoroutine(instance.DelayedSwapEvents("Block!"));
		}
	}

	void SwapSpotlights()
	{
		if (leftSpotlight.color == Color.clear)
		{
//<<<<<<< HEAD
			leftSpotlight.color = DefaultSpotLightColor;
//=======
			//leftSpotlight.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
//>>>>>>> 9475b744fe88b6d7816ed39f2aaeb3b340af0625
			rightSpotlight.color = Color.clear;
		}
		else
		{
			leftSpotlight.color = Color.clear;
//<<<<<<< HEAD
			rightSpotlight.color = DefaultSpotLightColor;
//=======
			//rightSpotlight.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
//>>>>>>> 9475b744fe88b6d7816ed39f2aaeb3b340af0625
		}
	}

    public static bool GetIsPlayersTurn()
    {
        return instance.isPlayersTurn;
    }

	IEnumerator DelayedSwapEvents(string swapText)
	{
		yield return new WaitForSeconds(RhythmManager.GetSubdivisionLength());

		turnCountdownText.text = swapText;
		SwapSpotlights();

		yield return new WaitForSeconds(0.8f);

		turnCountdownText.text = "";

		yield return null;
	}
}
