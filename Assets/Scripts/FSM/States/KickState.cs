using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickState : IState
{
    public KickState(BaseFSM chrctr)
    {
        character = chrctr;
        sM = character.stateMachine;
    }

    public override void Enter()
    {
        Debug.Log("Entering KickState");

        ani = character.GetComponent<Animator>();
    }

    public override void Execute()
    {
        Debug.Log("Execute KickState");

        //Going to have to check for combo inputs eventually
        ani.SetTrigger("kick");
        sM.ChangeState("idle");
    }

    public override void HandleInput(string inputStr)
    {

    }

    public override void HandleInput(int moveIndex)
    {

    }

    public override void Exit()
    {
        Debug.Log("Exiting KickState");
    }
}