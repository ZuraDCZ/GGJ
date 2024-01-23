using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    // Current difficulty level
    public Difficulty currentDifficulty;

    // Difficulty may variate based on 
    public float easyWeight = 1f;
    public float mediumWeight = 2f;
    public float heavyWeight = 3f;

    // Funcion para elegir la dificultad
    public void SetDifficulty(Difficulty newDifficulty)
    {
        currentDifficulty = newDifficulty;

        // Adjust game parameters based on difficulty
        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                SetEasyDifficulty();
                break;
            case Difficulty.Medium:
                SetMediumDifficulty();
                break;
            case Difficulty.Hard:
                SetHardDifficulty();
                break;
        }
    }

    private void SetEasyDifficulty()
    {
        Debug.Log("Easy difficulty set");
        // Adjust other parameters as needed
    }

    private void SetMediumDifficulty()
    {
        Debug.Log("Medium difficulty set");
        // Adjust other parameters as needed
    }

    private void SetHardDifficulty()
    {
        Debug.Log("Hard difficulty set");
        // Adjust other parameters as needed
    }

    public void IncreaseDifficulty()
    {        
        // or after the player reaches a certain score
        // For simplicity, we're just going through the difficulties in order
        int nextDifficultyIndex = ((int)currentDifficulty + 1) % System.Enum.GetValues(typeof(Difficulty)).Length;
        SetDifficulty((Difficulty)nextDifficultyIndex);
    }
}