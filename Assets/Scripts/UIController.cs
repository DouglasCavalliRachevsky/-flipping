using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum State
{
    Menu,
    Gameplay,
    Victory
}

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject menuGameObject;
    [SerializeField]
    private GameObject victoryGameObject;
    [SerializeField]
    private GameObject gameplayGameObject;

    public State CurrentState = State.Menu;
    
    public void ChangeState(State nextState)
    {
        ChangeState((int) nextState);
    }
    
    public void ChangeState(int nextState)
    {
        CurrentState = (State)nextState;
        switch (CurrentState)
        {
            case State.Menu:
                menuGameObject.SetActive(true);
                victoryGameObject.SetActive(false);
                gameplayGameObject.SetActive(false);
                break;
            case State.Victory:
                menuGameObject.SetActive(false);
                victoryGameObject.SetActive(true);
                gameplayGameObject.SetActive(true);
                break;
            case State.Gameplay:
            default:
                menuGameObject.SetActive(false);
                victoryGameObject.SetActive(false);
                gameplayGameObject.SetActive(true);
                break;
        }
    }
}
