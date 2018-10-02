using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPunchState : IState
{
    PlayerFSM character;
    StateMachine sM;
    IState nextState;
    Animator ani;

    public LightPunchState(PlayerFSM chrctr)
    {
        character = chrctr;
        sM = chrctr.stateMachine;
    }
    public void Enter()
    {
        Debug.Log("Entering LightPunchState");

        ani = character.GetComponent<Animator>();
    }

    public void Execute()
    {
        Debug.Log("Execute LightPunchState");

        ani.SetTrigger("lightpunch");
        sM.ChangeState("idle");
    }

    public void HandleInput()
    {
        //Going to have to check for combo inputs eventually

    }

    public void Exit()
    {
        Debug.Log("Exiting LightPunchState");
    }
}
