using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackState : IState
{
    PlayerFSM character;
    StateMachine sM;
    IState nextState;
    Animator ani;

    public HeavyAttackState(PlayerFSM chrctr)
    {
        character = chrctr;
        sM = chrctr.stateMachine;
    }
    public void Enter()
    {
        Debug.Log("Entering HeavyAttackState");

        ani = character.GetComponent<Animator>();
    }

    public void Execute()
    {
        Debug.Log("Execute HeavyAttackState");

        ani.SetTrigger("heavy");
        sM.ChangeState("idle");
    }

    public void HandleInput(string inputStr)
    {
        //Going to have to check for combo inputs eventually

    }

    public void Exit()
    {
        Debug.Log("Exiting HeavyAttackState");
    }
}
