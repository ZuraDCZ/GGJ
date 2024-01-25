using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Joke", menuName = "Jokes")]
public class Jokes : ScriptableObject
{
    [SerializeField] public string dialogue;
    [SerializeField] public int casesTrigger;
    //los cases trigger necesitan una representación de
}
