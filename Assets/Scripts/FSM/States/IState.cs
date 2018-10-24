﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IState
{
    public BaseFSM character;
    public StateMachine sM;
    //IState nextState;
    public Animator ani;

    public virtual void Enter()
    {

    }
    public virtual void Execute()
    {

    }
    public virtual void HandleInput(string inputName)
    {

    }
    public virtual void HandleInput(int moveIndex)
    {

    }
    public virtual void GetHurt()
    {
        
    }

    public virtual void Exit()
    {

    }
}
