using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : EnemyController {

	int punchDmg = 10;

    protected override void Start()
    {
        base.Start();
        enemyFsm = GetComponent<EnemyFSM>();
    }

    // Behavior per-subdivision, driven by rhythm system.
    // Assuming 16th note subdivisions.
    public override void OnSubdivision(int sub)
	{
		if (!attacking)
		{
			return;
		}

		// Telegraph before each attack.
		if (sub == 0 || sub == 10)
		{
			// Play warmup sound
			attackSoundPlayer.PlaySound();
		}

		// For testing, just attack every other quarter note.
		if (sub == 4 || sub == 12)
		{
			StartCoroutine(WaitThenDealDamage());
		}
	}

    //Choose Random int between 0-3
    // 0 = light punch, 1 = uppercut, 2 = heavy punch, 3 = kick
    public int ChooseRandomMove()
    {
        //max range is exclusive so max is 4 for now
        return Random.Range(0, 4);
    }

    IEnumerator WaitThenDealDamage()
	{
		// Give a small margin AFTER the beat for the player to hit block a lil late.
		yield return new WaitForSeconds(RhythmManager.GetSubdivisionLength() / 2.0f);
        //Alert Enemy FSM
        HandleInput(ChooseRandomMove());
		DealDamage(punchDmg);
	}
}
