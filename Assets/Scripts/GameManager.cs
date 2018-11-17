using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	static GameManager instance;

	public PlayerController playerController;

	public EnemyController currentEnemy;

	public SoundPlayer whiffSoundPlayer;

	public SoundPlayer[] crowdSoundPlayers;
	public SoundPlayer metronomePlayer;

	public int combatPhaseLength = 8;

	public Text currentMeasureText;

	public Text turnCountdownText;
	bool nextMeasureSwitch = false;

	bool isPlayersTurn = true;

    bool hasWon = false;

	public MusicPlayer musicPlayer;

	public SpriteRenderer leftSpotlight;
	public SpriteRenderer rightSpotlight;

    public float spotLightFlashTime = 0.01f;


    public float speakerScaleTime = 0.1f;

    public float speakerScaleValue = 0.1f;

    Color DefaultSpotLightColor = new Color(1, 0.7948295f, 0, 0.5529412f);

	int currentMeasure = 1;

	string defaultSignText = "";

	public static void SetDefaultSignText(string s)
	{
		instance.defaultSignText = s;
	}

    public static void ScaleSpeaker(SpriteRenderer speaker)
    {
        instance.StartCoroutine(instance.ScaleSprite(speaker, instance.speakerScaleValue, instance.speakerScaleTime));
    }

    public void StopSpeakerShake()
    {

    }

    IEnumerator ScaleSprite(SpriteRenderer spr, float scale, float duration)
    {
        Vector3 vector = new Vector3(scale, 0, 0);

        spr.transform.localScale += vector;

        yield return new WaitForSeconds(duration);

        spr.transform.localScale -= vector;

        //StartCoroutine(ScaleSprite(spr, scale, duration));
    }

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

	public static void IntroCountdown(float beatLength)
	{
		instance.StartCoroutine(instance.IntroCountdown_Coroutine(beatLength));
	}

	IEnumerator IntroCountdown_Coroutine(float beatLength)
	{
		for (int i = 0; i < 5; ++i)
		{
			metronomePlayer.PlaySound();

			yield return new WaitForSeconds(beatLength);
		}

		instance.turnCountdownText.text = "3";
		instance.crowdSoundPlayers[0].PlaySound();
		metronomePlayer.PlaySound();

		yield return new WaitForSeconds(beatLength);

		instance.turnCountdownText.text = "2";
		instance.crowdSoundPlayers[1].PlaySound();
		metronomePlayer.PlaySound();

		yield return new WaitForSeconds(beatLength);

		instance.turnCountdownText.text = "1";
		instance.crowdSoundPlayers[2].PlaySound();
		metronomePlayer.PlaySound();

		yield return new WaitForSeconds(beatLength);

		turnCountdownText.text = defaultSignText;

		yield return null;
	}

    private void Update()
    {
        if (hasWon)
        {
            SetCountdownText("Victory", Color.green);
            leftSpotlight.color = DefaultSpotLightColor;
            rightSpotlight.color = Color.clear;
        }
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

	public static bool NotNextMeasureSwitch()
	{
		return !instance.nextMeasureSwitch;
	}

	// Called every subdivision of a measure
	public static void OnSubdivision(int subCount)
	{
		// Just accepting 16th notes by default for now

		if (instance.nextMeasureSwitch)
		{
            switch (subCount)
            {
                case 4:
					// instance.currentEnemy.GoToIdleAnimation();
					instance.currentEnemy.SetAttacking(false); // Stop attacking during countdown
					
                    instance.turnCountdownText.text = "3";
					instance.crowdSoundPlayers[0].PlaySound();
                    //instance.StartCoroutine(instance.FlashSprite(instance.leftSpotlight, Color.clear, instance.spotLightFlashTime));
                    break;
                case 8:
					// instance.currentEnemy.GoToIdleAnimation();
					instance.currentEnemy.SetAttacking(false); // Stop attacking during countdown
					instance.turnCountdownText.text = "2";
					instance.crowdSoundPlayers[1].PlaySound();
                    //instance.StartCoroutine(instance.FlashSprite(instance.leftSpotlight, Color.clear, instance.spotLightFlashTime));
                    break;
                case 12:
					// instance.currentEnemy.GoToIdleAnimation();
					instance.currentEnemy.SetAttacking(false); // Stop attacking during countdown
					instance.turnCountdownText.text = "1";
					instance.crowdSoundPlayers[2].PlaySound();
                    //instance.StartCoroutine(instance.FlashSprite(instance.leftSpotlight, Color.clear, instance.spotLightFlashTime));
                    break;
            }
		}

		// Trigger any enemy behaviors
		instance.currentEnemy.OnSubdivision(subCount);
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

		// Regen player HP a lil bit
		instance.playerController.Heal(4);
	}

	void ChangeCombatPhase()
	{
        if (hasWon)
        {
            Debug.Log("Game won~");
            return;
        }

        playerController.ToggleInputActive();

		isPlayersTurn = !isPlayersTurn;

		// instance.currentEnemy.GoToIdleAnimation();
		currentEnemy.SetAttacking(!isPlayersTurn);

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

    public static void SetCountdownText(string txt, Color col)
    {
        instance.turnCountdownText.text = txt;
        instance.turnCountdownText.color = col;
    }

	IEnumerator DelayedSwapEvents(string swapText)
	{
        if (hasWon)
        {
            yield return null;
        }

		yield return new WaitForSeconds(RhythmManager.GetSubdivisionLength());

		turnCountdownText.text = swapText;
		SwapSpotlights();

		yield return new WaitForSeconds(0.8f);

		if (swapText != "Block!")
		{
			turnCountdownText.text = defaultSignText;
		}

		yield return null;
	}

    public static bool GetHasWon()
    {
        return instance.hasWon;
    }

    public static void WinGame()
    {
        instance.hasWon = true;

		instance.playerController.HandleInput("win");
        StageLightManager.ActivateWinLights();
    }

    public static void LoseGame()
    {
        instance.currentEnemy.HandleInput("win");
    }
}
