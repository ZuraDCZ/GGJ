using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsecuenceManager : MonoBehaviour
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
    public Consecuence CurrentCC;

    // Difficulty may variate based on 
    public float easyWeight = 1f;
    public float mediumWeight = 2f;
    public float heavyWeight = 3f;

    // Funcion para elegir la dificultad
    public void SetDifficulty(Consecuence consecuence)
    {
        CurrentCC = consecuence;

        // Adjust game parameters based on difficulty
        switch (CurrentCC)
        {
            case Consecuence.LaughVibration:
                SetLaughVibration();
                break;
            case Consecuence.MadMonster:
                SetMadMonster();
                break;
            case Consecuence.MonsterDiedLaugh:
                SetDyingMonster();
                break;
        }
    }

    private void SetLaughVibration()
    {
        Debug.Log("Easy difficulty set");
        // Adjust other parameters as needed
    }

    private void SetMadMonster()
    {
        Debug.Log("Medium difficulty set");
        // Adjust other parameters as needed
    }

    private void SetDyingMonster()
    {
        Debug.Log("Hard difficulty set");
        // Adjust other parameters as needed
    }

   
}