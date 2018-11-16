using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinisherState : IState
{

    public FinisherState(BaseFSM chrctr)
    {
        character = chrctr;
        sM = chrctr.stateMachine;
    }

    public override void Enter()
    {
        Debug.Log("Entering Finisher State");

        ani = character.GetComponent<Animator>();
    }

    public override void Execute()
    {

        ani.SetTrigger("finish");
    }

    public override void HandleInput(string inputStr)
    {

    }

    public override void HandleInput(int moveIndex)
    {

    }

    public override void GetHurt()
    {
        
    }

    public override void Exit()
    {
        Debug.Log("Exiting IdleState");
    }
}
