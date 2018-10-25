﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : BaseFSM {

    // Use this for initialization
    void Start ()
    {
        stateMachine = new StateMachine();
        initializeStates();
        stateMachine.ChangeState("idle");
    }

    public override void initializeStates()
    {
        stateMachine.AddState("idle", new IdleState(this));
        stateMachine.AddState("hurt", new HurtState(this));
        stateMachine.AddState("kick", new KickState(this));
        stateMachine.AddState("upper", new UpperCutState(this));
        stateMachine.AddState("lpunch", new LightPunchState(this));
        stateMachine.AddState("hpunch", new HeavyAttackState(this));
    }

    // Update is called once per frame
    void Update ()
    {
        stateMachine.Update();
	}

    public override void HandleInput(int moveIndex)
    {
        stateMachine.HandleInput(moveIndex);
    }

    public override void GetHurt()
    {
        base.GetHurt();
    }
}