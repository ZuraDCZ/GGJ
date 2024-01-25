using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] private float timer = 5f;
    private float CurrentTimer = 5f;

    private void Start()
    {
        CurrentTimer = timer;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTimer -= Time.deltaTime;
        if (CurrentTimer < 0)
        {
            Instantiate(prefab, transform.position, Quaternion.identity);
            CurrentTimer = timer;
        }
    }
}
