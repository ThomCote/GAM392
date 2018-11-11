using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{

    public IdleState(BaseFSM chrctr)
    {
        character = chrctr;
        sM = chrctr.stateMachine;
    }

    public override void Enter()
    {
        Debug.Log("Entering Idle State");

        ani = character.GetComponent<Animator>();
		// ani.SetTrigger("attacktoidle");
		// ani.SetTrigger("attacktoidle");
    }

    public override void Execute()
    {
        // Debug.Log("Execute Idle State");

        ani.SetBool("isWalking", false);
    }

    public override void HandleInput(string inputStr)
    {
		switch (inputStr)
		{
			case "Up":
				sM.ChangeState("upper");
				break;
			case "Down":
				sM.ChangeState("kick");
				break;
			case "Left":
				sM.ChangeState("lpunch");
				break;
			case "Right":
                sM.ChangeState("hpunch");
				break;
			case "Space":
                sM.ChangeState("block");
				break;
			case "lose":
				sM.ChangeState("lose");
				break;
			case "win":
				sM.ChangeState("win");
				break;
			default:
				break;
		}
    }

    public override void HandleInput(int moveIndex)
    {
        switch(moveIndex)
        {
            case 0:
                sM.ChangeState("winduppunch");
                break;
            case 1:
                sM.ChangeState("lpunch");
                break;
            case 2:
                sM.ChangeState("upper");
                break;
            case 3:
                sM.ChangeState("hpunch");
                break;
            case 4:
                sM.ChangeState("kick");
                break;
            case 5:
                sM.ChangeState("block");
                break;
			case 6:
				sM.ChangeState("idle");
				break;
        }
    }

    public override void GetHurt()
    {
        sM.ChangeState("hurt");
}

    public override void Exit()
    {
        Debug.Log("Exiting IdleState");
    }
}
