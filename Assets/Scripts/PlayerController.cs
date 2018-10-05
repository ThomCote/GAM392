using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

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
}
