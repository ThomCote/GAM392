using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockState : IState
{

    public BlockState(BaseFSM chrctr)
    {
        character = chrctr;
        sM = chrctr.stateMachine;
    }

    public override void Enter()
    {
        Debug.Log("Entering BlockState");
        ani = character.GetComponent<Animator>();
    }

    public override void Execute()
    {
        Debug.Log("Execute BlockState");

        ani.SetTrigger("blocked");
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
        Debug.Log("Exiting BlockState");
    }
}
