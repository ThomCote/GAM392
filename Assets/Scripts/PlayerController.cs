using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public int maxHP;
	int curHP;

	PlayerFSM fsm;

	// Use this for initialization
	void Start () {
		fsm = GetComponent<PlayerFSM>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HandleInput(string inputName)
	{
		fsm.HandleInput(inputName);
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
}
