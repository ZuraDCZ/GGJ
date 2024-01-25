using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsecuencesManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public enum Consecuence
    {
        LaughVibration,
        MadMonster,
        MonsterDiedLaugh
    }

    // Current difficulty level
    public Consecuence currentDifficulty;

    // Difficulty may variate based on 
    public float easyWeight = 1f;
    public float mediumWeight = 2f;
    public float heavyWeight = 3f;

    // Funcion para elegir la dificultad
    public void SetDifficulty(Consecuence consecuence)
    {
        currentDifficulty = consecuence;

        // Adjust game parameters based on difficulty
        switch (currentDifficulty)
        {
            case Consecuence.LaughVibration:
                setLaughingConsecuence();
                break;
            case Consecuence.MadMonster:
                setMadConsecuence();
                break;
            case Consecuence.MonsterDiedLaugh:
                SetDieConsecuence();
                break;
        }
    }

    private void setLaughingConsecuence()
    {
        Debug.Log("Easy difficulty set");
        // Adjust other parameters as needed
    }

    private void setMadConsecuence()
    {
        Debug.Log("Medium difficulty set");
        // Adjust other parameters as needed
    }

    private void SetDieConsecuence()
    {
        Debug.Log("Hard difficulty set");
        // Adjust other parameters as needed
    }

    public void IncreaseDifficulty()
    {        
        // or after the player reaches a certain score
        // For simplicity, we're just going through the difficulties in order
        int nextDifficultyIndex = ((int)currentDifficulty + 1) % System.Enum.GetValues(typeof(Consecuence)).Length;
        SetDifficulty((Consecuence)nextDifficultyIndex);
    }
}