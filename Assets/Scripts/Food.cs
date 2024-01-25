using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private int id;

    public Food(int p_id)
    {
        id = p_id;
    }

    public int GetID()
    {
        return id;
    }

    public static Food GenerateRandomFood()
    {
        int randomID = Random.Range(0,3);
        Food newFood = new Food(randomID);
        return newFood;
    }
}
