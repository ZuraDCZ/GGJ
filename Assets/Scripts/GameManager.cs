using UnityEngine;

/// <summary>
/// Enum for game states
/// </summary>
public enum GameState
{
    None,
    LoadMainMenu,
    MainMenu,
    LoadGame,
    Playing,
    Pause,
    Victory,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //Singleton
    public GameState state;

    private void Awake()
    {
        //Initialize singleton
        if (GameObject.Find("GameManager") == gameObject)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        ChangeGameState(GameState.None);
    }

    private void Start()
    {
        ChangeGameState(GameState.MainMenu);
    }

    public void ChangeGameState(GameState newState)
    {
        state = newState;

        switch (state)
        {
            case GameState.None:
                break;
            case GameState.LoadMainMenu:
                break;
            case GameState.MainMenu:
                break;
            case GameState.LoadGame:
                break;
            case GameState.Playing:
                break;
            case GameState.Pause:
                break;
            case GameState.Victory:
                break;
            case GameState.GameOver:
                break;
            default:
                break;
        }
    }
}
