using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    IState currentState;
    Dictionary<string, IState> stateDict = new Dictionary<string, IState>();

    public IState GetCurrent { get { return currentState; } }

    public void AddState(string key, IState state) { stateDict.Add(key, state); }
    public void Remove(string key) { stateDict.Remove(key); }
    public void Clear() { stateDict.Clear(); }


    public void ChangeState(string key, params object[] args)
    {
        if (currentState != null)
            currentState.Exit();

        IState newState = stateDict[key];

        currentState = newState;
        currentState.Enter();
    }

    //TODO: Use Null State pattern to eliminate null checks

    public void Update()
    {
        if (currentState != null) currentState.Execute();
    }
    
    public void HandleInput(string inputName)
    {
       if (currentState != null) currentState.HandleInput(inputName);
    }

    public void HandleInput(int moveIndex)
    {
        if (currentState != null) currentState.HandleInput(moveIndex);
    }

    public void GetHurt()
    {
        if (currentState != null) currentState.GetHurt();
    }
}

