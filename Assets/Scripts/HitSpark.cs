using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSpark : MonoBehaviour {

	SpriteRenderer spr;

	public float rotSpeed = 0.1f;
	public float scaleSpeed = 0.1f;
	public float duration = 0.1f;

	private void Awake()
	{
		spr = GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Execute(Color color)
	{
		StartCoroutine(Execute_Coroutine(color));
	}

	IEnumerator Execute_Coroutine(Color inColor)
	{
		spr.color = inColor;
		spr.sortingOrder = 5;

		float curTime = 0.0f;

		float rand = Random.value;
		if (rand < 0.5f)
		{
			rotSpeed *= -1.0f;
		}

		while (curTime < duration)
		{
			// Rotate, scale, and fade tick
			transform.Rotate(0.0f, 0.0f, transform.rotation.z + rotSpeed);

			Vector3 newScale = transform.lossyScale * (1.0f + scaleSpeed);
			transform.localScale = newScale;

			float newAlpha = curTime / duration;
			spr.color = new Color(inColor.r, inColor.g, inColor.b, newAlpha);

			curTime += Time.deltaTime;

			yield return null;
		}

		Destroy(gameObject);

		yield return null;
	}
}
