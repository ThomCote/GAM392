using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperCutState : IState
{
    PlayerFSM character;
    StateMachine sM;
    IState nextState;
    Animator ani;

    public UpperCutState(PlayerFSM chrctr)
    {
        character = chrctr;
        sM = chrctr.stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Entering UpperCutState");

        ani = character.GetComponent<Animator>();
    }

    public void Execute()
    {
        Debug.Log("Execute UpperCutState");

        ani.SetTrigger("upper");
        sM.ChangeState("idle");
    }

    public void HandleInput()
    {
        //Going to have to check for combo inputs eventually

    }

    public void Exit()
    {
        Debug.Log("Exiting UpperCutState");
    }
}