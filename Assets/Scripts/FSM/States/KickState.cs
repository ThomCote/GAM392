using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickState : IState
{
    PlayerFSM character;
    StateMachine sM;
    IState nextState;
    Animator ani;

    public KickState(PlayerFSM chrctr)
    {
        character = chrctr;
        sM = character.stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Entering KickState");

        ani = character.GetComponent<Animator>();
    }

    public void Execute()
    {
        Debug.Log("Execute KickState");

        //Going to have to check for combo inputs eventually
        ani.SetTrigger("kick");
        sM.ChangeState("idle");
    }

    public void HandleInput()
    {

    }

    public void Exit()
    {
        Debug.Log("Exiting KickState");
    }
}