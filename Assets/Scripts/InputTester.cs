using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputTester : MonoBehaviour {

	public string inputButton;

	Image img;

	float goodMargin;
	float perfectMargin;

	// Use this for initialization
	void Start () {
		img = GetComponent<Image>();
		RhythmManager.GetInputMargins(out goodMargin, out perfectMargin);
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown(inputButton))
		{
			float timePastSubdivision = RhythmManager.GetTimePastCurrentSubdivision();
			float timeToNext = RhythmManager.GetTimeToNextSubdivision();

			if (timePastSubdivision < goodMargin || timeToNext < goodMargin)
			{
				if (timePastSubdivision < perfectMargin || timeToNext < perfectMargin)
				{
					// Perfect
					StartCoroutine(FlashColor(Color.green));
				}
				else
				{
					// Good
					StartCoroutine(FlashColor(Color.yellow));
				}

				// Determine what subdivision we've hit and callback to RhythmManager
				if (timePastSubdivision < timeToNext)
				{
					// If this is the case, we're closer to the current subdivision number
					RhythmManager.OnInputSuccess(false);
				}
				else
				{
					// We're closer to the next subdivision number
					RhythmManager.OnInputSuccess(true);
				}
			}
			else
			{
				// Failed
				StartCoroutine(FlashColor(Color.red));

				ComboManager.FailCombo();
			}
		}
	}

	IEnumerator FlashColor(Color c)
	{
		img.color = c;

		yield return new WaitForSeconds(0.1f);

		img.color = Color.white;

		yield return null;
	}
}
