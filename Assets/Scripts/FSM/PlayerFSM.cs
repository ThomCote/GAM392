using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour {

   public StateMachine stateMachine = new StateMachine();

    void Start()
    {
        initializeStates();
        stateMachine.ChangeState("idle");
    }

    void initializeStates()
    {
        stateMachine.AddState("idle", new IdleState(this));
        stateMachine.AddState("walk", new WalkState(this));
        stateMachine.AddState("kick", new KickState(this));
        stateMachine.AddState("upper", new UpperCutState(this));
        stateMachine.AddState("lpunch", new LightPunchState(this));
        stateMachine.AddState("block", new BlockState(this));
    }

    void Update()
    {
        stateMachine.Update();
    }

    public void HandleInput(string inputName)
    {
        stateMachine.HandleInput(inputName);
    }
}


