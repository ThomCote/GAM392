using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockState : IState
{
    PlayerFSM character;
    StateMachine sM;
    IState nextState;
    Animator ani;

    public BlockState(PlayerFSM chrctr)
    {
        character = chrctr;
        sM = chrctr.stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Entering BlockState");
        ani = character.GetComponent<Animator>();
    }

    public void Execute()
    {
        Debug.Log("Execute BlockState");

        ani.SetTrigger("block");
        sM.ChangeState("idle");
    }

    public void HandleInput(string inputStr)
    {

    }

    public void Exit()
    {
        Debug.Log("Exiting BlockState");
    }
}
