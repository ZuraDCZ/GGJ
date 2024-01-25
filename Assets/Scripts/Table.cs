using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Table : MonoBehaviour
{
    private bool occupied; // Tracks if the table is occupied
    private Dictionary<Transform, bool> sits = new Dictionary<Transform, bool>(); //The amount of sits the table has, and if they are occupied


    private void Awake()
    {
        AddSits();
    }

    /// <summary>
    /// Adds sits to a list to track their position and occupied status
    /// </summary>
    private void AddSits()
    {
        foreach (Transform child in transform)
        {
            sits.Add(child, false);
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

    public void Unoccupy()
    {
        occupied = false;
    }

    public Dictionary<Transform, bool> Sits() //Tracks sits list
    {
        return sits;
    }
}
