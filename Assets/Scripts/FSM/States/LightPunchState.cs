using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPunchState : IState
{
    public LightPunchState(BaseFSM chrctr)
    {
        character = chrctr;
        sM = chrctr.stateMachine;
    }
    public override void Enter()
    {
        Debug.Log("Entering LightPunchState");

        ani = character.GetComponent<Animator>();
    }

    public override void Execute()
    {
        Debug.Log("Execute LightPunchState");

        ani.SetTrigger("lightpunch");
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
        Debug.Log("Exiting LightPunchState");
    }
}
