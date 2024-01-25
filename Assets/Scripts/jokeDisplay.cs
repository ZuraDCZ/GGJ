using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class jokeDisplay : MonoBehaviour
{
    public Jokes joke;

    public List<Jokes> jokesToGet;
    public TextMeshProUGUI jokeSaid;
    // Start is called before the first frame update
    void Start()
    {
        jokeSaid.text = joke.dialogue;
    }

}
