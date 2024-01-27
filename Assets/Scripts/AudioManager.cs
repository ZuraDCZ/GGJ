using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //Audio manager Singleton
    public static AudioManager instance;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] float volume;


    public void Awake()
    {
        //Initialize singleton
        if (GameObject.Find("AudioManager") == gameObject)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
