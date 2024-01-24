using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    private bool occupied; // Tracks if the table is occupied
    private List<Transform> sits = new List<Transform>(); //The amount of sits the table has


    private void Awake()
    {
        AddSits();
    }

    /// <summary>
    /// Adds sits to alist to track their position
    /// </summary>
    private void AddSits()
    {
        foreach (Transform child in transform)
        {
            sits.Add(child);
        }
    }

    public bool isOccupied() //Checks if the table is occupied
    {
        return occupied;
    }

    public void Occupy() //Occupies table
    {
        occupied = true;
    }

    public List<Transform> Sits() //Tracks sits list
    {
        return sits;
    }
}
