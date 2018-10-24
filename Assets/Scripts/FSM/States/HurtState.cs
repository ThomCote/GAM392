using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : IState
{
    public HurtState(BaseFSM chrctr)
    {
        character = chrctr;
        sM = chrctr.stateMachine;
    }

    public override void Enter()
    {
        Debug.Log("Entering HurtState");
        ani = character.GetComponent<Animator>();
    }

    public override void Execute()
    {
        Debug.Log("Execute HurtState");

        ani.SetTrigger("hit");
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
        Debug.Log("Exiting HurtState");
    }

}
