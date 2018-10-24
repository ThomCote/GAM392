using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : IState
{ 

    public WalkState(BaseFSM chrctr)
    {
        character = chrctr;
        sM = chrctr.stateMachine;
    }

    public override void Enter()
    {
        Debug.Log("Entering WalkState");
        ani = character.GetComponent<Animator>();
    }

    public override void Execute()
    {
        Debug.Log("Execute WalkState");
        
        ani.SetBool("isWalking", true);
    }

    public override void HandleInput(string inputStr)
    {
        if (Input.GetKeyUp(KeyCode.D))
        {
            sM.ChangeState("idle");
        }

    }

    public override void HandleInput(int moveIndex)
    {

    }

    public override void Exit()
    {
        Debug.Log("Exiting WalkState");
    }
}
