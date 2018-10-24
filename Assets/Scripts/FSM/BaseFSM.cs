using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseFSM : MonoBehaviour {

    public StateMachine stateMachine;

    public virtual void initializeStates()
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
        stateMachine.GetHurt();
    }
}


