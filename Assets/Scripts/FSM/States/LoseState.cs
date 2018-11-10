using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseState : IState {

	public LoseState(BaseFSM chrctr)
	{
		character = chrctr;
		sM = chrctr.stateMachine;
	}

	public override void Enter()
	{
		Debug.Log("Entering Lose State");

		ani = character.GetComponent<Animator>();
		// ani.SetTrigger("attacktoidle");
	}

	public override void Execute()
	{
		// Debug.Log("Execute Idle State");

		ani.SetTrigger("lose");
	}

	public override void HandleInput(string inputStr)
	{

	}

	public override void HandleInput(int moveIndex)
	{

	}

	public override void GetHurt()
	{
		// sM.ChangeState("hurt");
	}

	public override void Exit()
	{
		Debug.Log("Exiting IdleState");
	}
}
