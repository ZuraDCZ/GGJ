using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CammeraShaker : MonoBehaviour
{
    [SerializeField] private float shakeVibrato = 10f;//intensity
    [SerializeField] private float shakeRandomness = 0.1f;//direction of the shake
    [SerializeField] private float shakeTime = 0.01f;//duration for the shake

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Shake();
        }
    }

    public void Shake()
    {
        StartCoroutine(IEShake());
    }

    private IEnumerator IEShake()
    {
        Vector3 currentPosition = transform.position;
        for (int i = 0; i < shakeVibrato; i++)
        {
            Vector3 shakePosition = currentPosition + Random.onUnitSphere * shakeRandomness;
            yield return new WaitForSeconds(shakeTime);
            transform.position = shakePosition;
        }
    }

}
