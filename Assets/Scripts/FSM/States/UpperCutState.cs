﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperCutState : IState
{
    public UpperCutState(BaseFSM chrctr)
    {
        character = chrctr;
        sM = chrctr.stateMachine;
    }

    public override void Enter()
    {
        Debug.Log("Entering UpperCutState");

        ani = character.GetComponent<Animator>();
    }

    public override void Execute()
    {
        Debug.Log("Execute UpperCutState");

        ani.SetTrigger("upper");
        sM.ChangeState("idle");
    }

    public override void HandleInput(string inputStr)
    {
        //Going to have to check for combo inputs eventually

    }

    public override void HandleInput(int moveIndex)
    {

    }

    public override void Exit()
    {
        Debug.Log("Exiting UpperCutState");
    }
}