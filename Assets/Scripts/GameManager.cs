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
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //Singleton
    private GameState state;

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
        ChangeGameState(GameState.LoadMainMenu);
    }

    public void ChangeGameState(GameState newState)
    {
        state = newState;

        switch (state)
        {
            case GameState.None:
                break;
            case GameState.LoadMainMenu:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
                ChangeGameState(GameState.MainMenu);
                break;
            case GameState.MainMenu:
                break;
            case GameState.LoadGame:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
                ChangeGameState(GameState.Playing);
                break;
            case GameState.Playing:
                break;
            case GameState.Pause:
                break;
            case GameState.GameOver:
                OnGameOver();
                break;
            default:
                break;
        }
    }

    public GameState GetGameState()
    {
        return state;
    }

    private void OnGameOver()
    {

    }
}
