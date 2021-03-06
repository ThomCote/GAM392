﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindUpPunchState : IState {

    public WindUpPunchState(BaseFSM chrctr)
    {
        character = chrctr;
        sM = chrctr.stateMachine;
    }
    public override void Enter()
    {
        Debug.Log("Entering WindUpPunchState");

        ani = character.GetComponent<Animator>();
    }

    public override void Execute()
    {
        Debug.Log("Execute WindUpPunchState");

        ani.SetTrigger("winduppunch");
        sM.ChangeState("idle");

    }

    public override void HandleInput(string inputStr)
    {

    }

    public override void HandleInput(int moveIndex)
    {
		if (moveIndex == 1)
		{
			sM.ChangeState("lpunch");
		}
		else
		{
			ani.SetTrigger("attacktoidle");
			sM.ChangeState("idle");
		}
    }

    public override void Exit()
    {
		// ani.SetTrigger("attacktoidle");
		Debug.Log("Exiting WindUpPunchState");
    }
}
