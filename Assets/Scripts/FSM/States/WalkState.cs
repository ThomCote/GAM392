using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : IState
{
    PlayerFSM character;
    StateMachine sM;
    IState nextState;
    Animator ani;

    public WalkState(PlayerFSM chrctr)
    {
        character = chrctr;
        sM = chrctr.stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Entering WalkState");
        ani = character.GetComponent<Animator>();
    }

    public void Execute()
    {
        Debug.Log("Execute WalkState");
        
        ani.SetBool("isWalking", true);
    }

    public void HandleInput(string inputStr)
    {
        if (Input.GetKeyUp(KeyCode.D))
        {
            sM.ChangeState("idle");
        }

    }

    public void Exit()
    {
        Debug.Log("Exiting WalkState");
    }
}
