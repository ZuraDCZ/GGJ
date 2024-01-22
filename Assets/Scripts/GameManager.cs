using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MENU,
    GAMEPLAY,
    PAUSE
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState state;

    private void Awake()
    {
        instance = this;
        if (GameObject.Find("GameManager") != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        state = GameState.MENU;
    }

    public void SetGameState(GameState newState)
    {
        state = newState;
    }
}
