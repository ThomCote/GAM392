using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : EnemyController {

	int punchDmg = 10;

	// Behavior per-subdivision, driven by rhythm system.
	// Assuming 16th note subdivisions.
	public override void OnSubdivision(int sub)
	{
		if (!attacking)
		{
			return;
		}

		// For testing, just attack every other quarter note.
		if (sub == 5 || sub == 13)
		{
			DealDamage(punchDmg);
		}
	}
}
