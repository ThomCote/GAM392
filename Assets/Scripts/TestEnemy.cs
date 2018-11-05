using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : EnemyController {

	int punchDmg = 10;

	int hpThreeFourths;
	int hpHalf;
	int hpOneFourth;

    protected override void Start()
    {
        base.Start();
        enemyFsm = GetComponent<EnemyFSM>();

		hpThreeFourths = (int)(maxHP * 0.75f);
		hpHalf = (int)(maxHP / 2);
		hpOneFourth = (int)(maxHP * 0.25f);
    }

    // Behavior per-subdivision, driven by rhythm system.
    // Assuming 16th note subdivisions.
    public override void OnSubdivision(int sub)
	{
		if (!attacking)
		{
			return;
		}

		if (curHP > hpThreeFourths)
		{
			AttackPattern1(sub);
		}
		else if (curHP > hpHalf)
		{
			AttackPattern2(sub);
		}
		else if (curHP > hpOneFourth)
		{
			AttackPattern3(sub);
		}
		else
		{
			AttackPattern4(sub);
		}
	}

	void AttackPattern1(int sub)
	{
		// Telegraph before each attack.
		if (sub == 0 || sub == 8)
		{
			// Play warmup sound
			attackSoundPlayer.PlaySound();
			HandleInput(0); //windup;
		}

		// For testing, just attack every other quarter note.
		if (sub == 4 || sub == 12)
		{
			StartCoroutine(WaitThenDealDamage());
		}
	}

	void AttackPattern2(int sub)
	{
		// Telegraph before each attack.
		if (sub == 0 || sub == 8)
		{
			// Play warmup sound
			attackSoundPlayer.PlaySound();
			HandleInput(0); //windup;
		}

		// For testing, just attack every other quarter note.
		if (sub == 4 || sub == 10)
		{
			StartCoroutine(WaitThenDealDamage());
		}
	}

	void AttackPattern3(int sub)
	{
		// Telegraph before each attack.
		if (sub == 0 || sub == 4 || sub == 12)
		{
			// Play warmup sound
			attackSoundPlayer.PlaySound();
			HandleInput(0); //windup;
		}

		// For testing, just attack every other quarter note.
		if (sub == 2 || sub == 6 || sub == 14)
		{
			StartCoroutine(WaitThenDealDamage());
		}
	}

	void AttackPattern4(int sub)
	{
		// Telegraph before each attack.
		if (sub == 0 || sub == 6 || sub == 12)
		{
			// Play warmup sound
			attackSoundPlayer.PlaySound();
			HandleInput(0); //windup;
		}

		// For testing, just attack every other quarter note.
		if (sub == 2 || sub == 8 || sub == 14)
		{
			StartCoroutine(WaitThenDealDamage());
		}
	}

    //Choose Random int between 0-3
    // 0 = light punch, 1 = uppercut, 2 = heavy punch, 3 = kick
    public int ChooseRandomMove()
    {
        //max range is exclusive so max is 4 for now
        //Range set to 0 because we only have 1 enemy attack
        return Random.Range(1, 1);
    }

    IEnumerator WaitThenDealDamage()
	{
		// Give a small margin AFTER the beat for the player to hit block a lil late.
		yield return new WaitForSeconds(RhythmManager.GetSubdivisionLength() / 1.5f);
        //Alert Enemy FSM
        HandleInput(ChooseRandomMove());
		DealDamage(punchDmg);
	}
}
