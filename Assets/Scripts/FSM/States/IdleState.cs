using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    PlayerFSM character;
    StateMachine sM;
    IState nextState;
    Animator ani;

    public IdleState(PlayerFSM chrctr)
    {
        character = chrctr;
        sM = chrctr.stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Entering Idle State");

        ani = character.GetComponent<Animator>();
    }

    public void Execute()
    {
        Debug.Log("Execute Idle State");

        ani.SetBool("isWalking", false);
    }

    public void HandleInput(string inputStr)
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
				break;
			case "Space":
				break;
			default:
				break;
		}

        //TODO: Decouple inputs to controller class
        //pass in Controller class triggers here instead of GetKey()

        //&& (!ani.GetCurrentAnimatorStateInfo(0).IsName("Kick") || !ani.GetCurrentAnimatorStateInfo(0).IsName("TigerUppercut"))
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    sM.ChangeState("walk");
        //}

        ////&& !ani.GetCurrentAnimatorStateInfo(0).IsName("Kick") :this code prevents from using the same animation state twice in a row
        ////                                                      :may implement counter for number of times state has been used
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    sM.ChangeState("kick");
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha2) && !ani.GetCurrentAnimatorStateInfo(0).IsName("TigerUppercut"))
        //{
        //    sM.ChangeState("upper");
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha3) && !ani.GetCurrentAnimatorStateInfo(0).IsName("LightPunch"))
        //{
        //    sM.ChangeState("lpunch");
        //}
    }

    public void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}
