using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public int maxHP;
	int curHP;

	PlayerFSM fsm;

	bool inputActive;

	// Use this for initialization
	void Start () {
		fsm = GetComponent<PlayerFSM>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HandleInput(string inputName)
	{
		if (inputActive)
		{
			fsm.HandleInput(inputName);
		}
	}

	void TakeDamage(int dmg)
	{
		curHP -= dmg;

		if (curHP <= 0)
		{
			Die();
		}
	}

	void Die()
	{

	}

	public void ToggleInputActive()
	{
		inputActive = !inputActive;
	}

	public bool GetInputActive()
	{
		return inputActive;
	}
}
